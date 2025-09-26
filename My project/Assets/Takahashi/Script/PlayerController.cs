using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputMove;
    [Header("ナイフの初期値")]
    [SerializeField] public int possessionNumber = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float defaultSpeed = 10f;
    [SerializeField] private float maxSpeed = 13f;//ダッシュ時のスピード
    [SerializeField] private float jump = 10f;
    [SerializeField] private float translateSpeed = 10f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform cameraTransform;
    //[SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform translatePosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject knifeObject;//ナイフオブジェクト
    //プレイヤーから見た相対的な位置（距離・角度）を表す
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -4);
    //[SerializeField] private Vector3 cameraPosition;
    public bool onGround = true;
    private bool isRightTrigger = false;//攻撃判定
    private bool isNomalAttack = false;
    private bool isDush = false;//ダッシュ判定
    private bool isInputRightTrigger = false;//右トリガーの入力判定
    private bool isInputRB = false;//RBの入力判定
    private bool isInputLB = false;//LBの入力判定
    public float rayLength = 0.2f;
    public float distance = 5f; // カメラとプレイヤー間の距離
    private float height = 2f;//カメラの高さ
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw, pitch;
    //[SerializeField] private InputActionReference _lookActionReference;
    //[SerializeField] private InputActionReference _moveActionReference;
    //[SerializeField] private InputActionReference _jumpActionReference;
    //[SerializeField] private InputActionReference _attackActionReference;
    //[SerializeField] private InputActionReference _weakSkillActionReference;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    [SerializeField] private List<GameObject> knifeObjectList = new List<GameObject>();
    private bool isInitialGenerate = false;//初期生成する際のbool
    [SerializeField] private PlayerInput playerInput;
    private Gamepad gamepad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void Domove(InputAction.CallbackContext context)
    {
        //performed、canceledコールバックを受け取る
        if (context.started) return;
        inputMove = context.ReadValue<Vector2>();

        //Moveアクションの入力取得
        //var inputMove = context.ReadValue<Vector2>();
        var look = context.ReadValue<Quaternion>();
    }

    private void OnEnable()
    {
        //// InputActionを有効化
        //// これをしないと入力を受け取れないことに注意
        ////playerInput.onActionTriggered += OnAction;
        //_lookActionReference.action.Enable();
        //_moveActionReference.action.Enable();
        //_jumpActionReference.action.Enable();
        //_attackActionReference.action.Enable();

        //_jumpActionReference.action.performed += OnJump;
        //_attackActionReference.action.performed += OnNomalAttack;

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["NomalAttack"];
        var lbAction = playerInput.actions["WeakSkill"];
        var rbAction = playerInput.actions["StrongSkill"];

        jumpAction.performed += OnJump;
        attackAction.performed += OnNomalAttack;
        attackAction.canceled += OnNomalAttack; // 押し離し両方見る
        lbAction.performed += OnLB;
        lbAction.canceled += OnLB;
    }

    // 無効化
    private void OnDisable()
    {
        //// 自身が無効化されるタイミングなどで
        //// Actionを無効化する必要がある
        //_lookActionReference.action.Disable();
        //_moveActionReference.action.Disable();
        //_jumpActionReference.action.Disable();
        //_attackActionReference.action.Disable();

        //_jumpActionReference.action.performed -= OnJump;
        //_attackActionReference.action.performed -= OnNomalAttack;

    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
            if (isGrounded())
            {
                Debug.Log("処理");
                //playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
                Jump();
            }
        }
    }

    private void OnLB(InputAction.CallbackContext callbackContext)
    {
        isInputLB = callbackContext.performed; // true/false 自動で更新
        Debug.Log(isInputLB);
        //if (callbackContext.performed)//弱スキルコマンドを押したとき
        //{
        //    isInputLB = true;
        //}
        //else if (callbackContext.canceled)//弱スキルコマンドを離したとき
        //{
        //    isInputLB = false;
        //}
    }

    private void OnNomalAttack(InputAction.CallbackContext callbackContext)
    {
        ////Debug.Log(callbackContext);
        //if (callbackContext.performed)//右トリガーを押したとき
        //{
        //    isRightTrigger = true;
        //    if (isInputLB)
        //    {
        //        WeakSkill();
        //    }
        //    else
        //    {
        //        normalAttack();
        //    }
        //}
        //else if (callbackContext.canceled)//右トリガーを離したとき
        //{
        //    isRightTrigger = false;
        //}
        ////normalAttack();

        if (callbackContext.performed)
        {
            isRightTrigger = true;

            if (isInputLB)
            {
                WeakSkill(); // LBと同時押しなら弱スキル
            }
            else
            {
                normalAttack(); // 単発なら通常攻撃
            }
        }
        else if (callbackContext.canceled)
        {
            isRightTrigger = false;
        }
    }

    void Start()
    {
        isDush = false;
        GenerateKnife();
        var gamepads = Gamepad.all;
        //現在接続されているgemepadのリストを取得
        //for(int i = 0; i < playerInput.Length; i++)//プレイヤーの数分ループする
        //{
        //    if (i < gamepads.Count)
        //    {
        //        playerInput[i].SwitchCurrentControlScheme(gamepads[i]);//gamepad割り当て
        //        Debug.Log($"Player {i} assigned to {gamepads[i].displayName}");
        //    }
        //}

        if (playerInput.devices.Count > 0)
        {
            gamepad = playerInput.devices[0] as Gamepad;
            Debug.Log(gamepads.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        // 現在接続されているGamepadを取得
        //gamepad = Gamepad.current;
        //gamepad = Gamepad.all[0];//複数接続の時に使用する
        //Vector2 lookvalue = _lookActionRef.action.ReadValue<Vector2>();

        //左スティックで移動
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //Debug.Log("Move入力: " + moveValue);
        //if (moveValue.sqrMagnitude > 0.01f)
        //{
        //    // 「カメラの前方向」を基準に移動
        //    Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);

        //    Vector3 targetPos = playerRigidbody.position + moveDir * speed * Time.deltaTime;
        //    playerRigidbody.MovePosition(targetPos);
        //}

        if (moveValue.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);
            Vector3 targetPos = playerRigidbody.position + moveDir * speed * Time.deltaTime;
            playerRigidbody.MovePosition(targetPos);
        }

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;

        //if (isRightTrigger)
        //{
        //    normalAttack();//通常攻撃
        //    //Debug.Log("RT 押された: " + rt);
        //    isRightTrigger = true;
        //}
        ////RTとRBの同時入力
        ////else if (Input.GetKeyUp(KeyCode.JoystickButton5) && rt > 0.5f)
        //else if (isInputLB && isRightTrigger)
        //{
        //    WeakSkill();//弱スキル
        //    isNomalAttack = false;
        //}
        //// 右トリガーの値（0.0～1.0）
        //float rt = Gamepad.current.rightTrigger.ReadValue();
        //float lt = Gamepad.current.leftTrigger.ReadValue();

        ////右トリガーの入力をして離した時//処理の重複を防ぐためisNomalAttackがtrueの時
        ////if (Gamepad.current.rightTrigger.wasReleasedThisFrame&&isNomalAttack)
        //if (isRightTrigger)
        //{
        //    normalAttack();//通常攻撃
        //    Debug.Log("RT 押された: " + rt);
        //    isRightTrigger = true;
        //}
        ////RTとRBの同時入力
        ////else if (Input.GetKeyUp(KeyCode.JoystickButton5) && rt > 0.5f)
        //else if (isInputLB && isRightTrigger)
        //{
        //    WeakSkill();//弱スキル
        //    isNomalAttack = false;
        //}
        //else if (Input.GetKeyUp(KeyCode.JoystickButton4) && rt > 0.5f)
        //{
        //    StrongSkill();//強スキル
        //    isNomalAttack = false;
        //}
        //if (rt == 0)
        //{
        //    isRightTrigger = false;
        //    isNomalAttack = true;
        //}

        //if (lt > 0.5f && !isDush)//ダッシュ
        //{
        //    speed = maxSpeed;
        //    //playerTransform.position = newPos;
        //    if (speed > maxSpeed)
        //    {
        //        speed = maxSpeed;
        //    }
        //    isDush = true;
        //    Debug.Log(maxSpeed);
        //}
        //else
        //{
        //    speed = defaultSpeed;
        //    //playerTransform.position = newPos;
        //    isDush = false;
        //}

    }

    void LateUpdate()
    {
        //// 右スティック入力を取得
        //Vector2 lookValue = moveAction.ReadValue<Vector2>();

        //// Y軸回転だけ反映（左右回転）
        //yaw += lookValue.x * rotationSpeed * Time.deltaTime;//左右回転を計算

        //// プレイヤーを回転させる
        //playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);
        //pitch -= lookValue.y * rotationSpeed * Time.deltaTime;//上下回転を計算
        //pitch = Mathf.Clamp(pitch, -20f, 60f); // 上下の制限
        //// カメラの回転を計算
        //Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);

        //Vector3 playerCenter = playerTransform.position + Vector3.up * height;//プレイヤーの中心位置を計算
        //Vector3 targetPosition = playerCenter - playerTransform.forward * distance;

        //cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
        //cameraTransform.rotation = cameraRot;

        ////プレイヤー基準でオフセットを加える
        //cameraTransform.position = playerTransform.position + playerTransform.rotation * cameraOffset;
        //// カメラはプレイヤーを見る
        //cameraTransform.LookAt(playerTransform.position + Vector3.up);


        // 右スティック入力を取得
        Vector2 lookValue = lookAction.ReadValue<Vector2>();

        // 回転を更新
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        // プレイヤーの向き（Y軸のみ）
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // カメラの回転と位置
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 playerCenter = playerTransform.position + Vector3.up * height;
        Vector3 targetPosition = playerCenter - playerTransform.forward * distance;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
        cameraTransform.rotation = cameraRot;
    }

    private bool isGrounded()//地面に足がついているかの判定に使われる
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength, groundLayer);
    }

    //RayをScene上だけ可視化
    private void OnDrawGizmosSelected()
    {
        // SceneビューでRayを可視化
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position + Vector3.down * rayLength);
    }

    public void Jump()
    {
        Debug.Log("jump");
        if (isGrounded())
        {
            playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }

    private void GenerateKnife()//初期設定したナイフの数だけリストに加える
    {
        if (!isInitialGenerate)
        {
            for (int i = 0; i <= possessionNumber - 1; i++)
            {
                knifeObjectList.Add(knifeObject);
            }
            isInitialGenerate = true;
        }
    }

    public void normalAttack()
    {
        Debug.Log("通常攻撃！");
        Vector3 translatePos = playerTransform.position;
        translatePos.z += 1.5f;
        if (knifeObjectList.Count > 0)
        {
            //ナイフを指定したpositionに生成して飛ばす
            GameObject knife = Instantiate(knifeObject, translatePosition.position, translatePosition.rotation);
            knife.tag = "Knife";
            Rigidbody rigidbody = knife.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
                Debug.Log(knife.tag);
            }
            KnifeControllertr knifeControllertr = knife.GetComponent<KnifeControllertr>();
            knifeControllertr.owner = this.gameObject;
        }
    }

    public void WeakSkill()
    {
        Debug.Log("弱スキル");
    }

    public void StrongSkill()
    {
        Debug.Log("強スキル");
    }
}
