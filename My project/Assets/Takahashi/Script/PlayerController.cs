using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform translatePosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject knifeObject;//ナイフオブジェクト
    //プレイヤーから見た相対的な位置（距離・角度）を表す
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -4);
    //[SerializeField] private Vector3 cameraPosition;
    public bool onGround = true;
    public Transform groundCheck;   // 足元の判定位置
    private Gamepad gamepad;
    private bool isRightTrigger = false;//攻撃判定
    private bool isNomalAttack = false;
    private bool isGround = false;
    private bool isDush = false;//ダッシュ判定
    private float groundCheckOffsetY = 0.2f;
    private float groundCheckRadius = 0.2f;
    private float groundCheckDistance = 0.2f;
    public float groundDistance = 0.2f;
    public float rayLength = 0.2f;
    public float distance=5f; // カメラとプレイヤー間の距離
    private float height=2f;//カメラの高さ
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw,pitch;
    [SerializeField] private InputActionReference _lookActionRef;
    [SerializeField] private InputActionReference _moveActionRef;
    [SerializeField] private List<GameObject> knifeObjectList = new List<GameObject>();
    private bool isInitialGenerate = false;//初期生成する際のbool


    public void Domove(InputAction.CallbackContext context)
    {
        //performed、canceledコールバックを受け取る
        if (context.started) return;

        //Moveアクションの入力取得
        var inputMove = context.ReadValue<Vector2>();
        var look= context.ReadValue<Quaternion>();
    }

    private void OnEnable()
    {
        // InputActionを有効化
        // これをしないと入力を受け取れないことに注意
        _lookActionRef.action?.Enable();
        _moveActionRef.action.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        // Actionを無効化する必要がある
        _lookActionRef.action.Disable();
        _moveActionRef.action.Disable();
    }

    void Start()
    {
        isDush = false;
        GenerateKnife();
    }

    // Update is called once per frame
    void Update()
    {
        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        //gamepad = Gamepad.all[0];//複数接続の時に使用する
        if (gamepad == null) return;

        //左スティックで移動
        Vector2 moveValue = _moveActionRef.action.ReadValue<Vector2>();

        if (moveValue.sqrMagnitude > 0.01f)
        {
            // 「カメラの前方向」を基準に移動
            Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);

            Vector3 targetPos = playerRigidbody.position + moveDir * speed * Time.deltaTime;
            playerRigidbody.MovePosition(targetPos);
        }

        if (Gamepad.current != null)
        {
            // 右トリガーの値（0.0～1.0）
            float rt = Gamepad.current.rightTrigger.ReadValue();
            float lt = Gamepad.current.leftTrigger.ReadValue();

            //右トリガーの入力をして離した時//処理の重複を防ぐためisNomalAttackがtrueの時
            if (Gamepad.current.rightTrigger.wasReleasedThisFrame&&isNomalAttack)
            {
                normalAttack();//通常攻撃
                Debug.Log("RT 押された: " + rt);
                isRightTrigger = true;
            }
            //RTとRBの同時入力
            else if (Input.GetKeyUp(KeyCode.JoystickButton5) && rt > 0.5f)
            {
                WeakSkill();//弱スキル
                isNomalAttack = false;
            }
            else if (Input.GetKeyUp(KeyCode.JoystickButton4) && rt > 0.5f)
            {
                StrongSkill();//強スキル
                isNomalAttack = false;
            }
            if (rt == 0)
            {
                isRightTrigger = false;
                isNomalAttack = true;
            }

            if (lt > 0.5f&&!isDush)//ダッシュ
            {
                speed = maxSpeed;
                //playerTransform.position = newPos;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                isDush = true;
                Debug.Log(maxSpeed);
            }
            else
            {
                speed = defaultSpeed;
                //playerTransform.position = newPos;
                isDush = false;
            }

            //ジャンプ処理
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                Debug.Log("jump");
                if (isGrounded())
                {
                    playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
                }
            }
        }
    }

    void LateUpdate()
    {
        // 右スティック入力を取得
        Vector2 lookValue = _lookActionRef.action.ReadValue<Vector2>();

        // Y軸回転だけ反映（左右回転）
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;//左右回転を計算

        // プレイヤーを回転させる
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;//上下回転を計算
        pitch = Mathf.Clamp(pitch, -20f, 60f); // 上下の制限
        // カメラの回転を計算
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 playerCenter = playerTransform.position + Vector3.up * height;//プレイヤーの中心位置を計算
        Vector3 targetPosition = playerCenter - playerTransform.forward * distance;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
        cameraTransform.rotation= cameraRot;

        //プレイヤー基準でオフセットを加える
        cameraTransform.position = playerTransform.position + playerTransform.rotation * cameraOffset;
        // カメラはプレイヤーを見る
        cameraTransform.LookAt(playerTransform.position + Vector3.up);

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

    private void GenerateKnife()//初期設定したナイフの数だけリストに加える
    {
        if (!isInitialGenerate)
        {
            for (int i = 0; i <= possessionNumber-1; i++)
            {
                knifeObjectList.Add(knifeObject);
            }
            isInitialGenerate = true;
        }
    }

    public void normalAttack()
    {
        Debug.Log("通常攻撃！");
        if (knifeObjectList.Count > 0)
        {
            //ナイフを指定したpositionに生成して飛ばす
            GameObject knife = Instantiate(knifeObject, translatePosition.position, translatePosition.rotation);
            Rigidbody rigidbody = knife.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
            }
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
