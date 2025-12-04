using System.Collections.Generic;
using TMPro;
using UnityEditor;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager = null;

    private Vector2 inputMove;
    [Header("ナイフの初期値")]
    [SerializeField] public int possessionNumber = 5;
    //プレイヤーの準備完了時に表示させるCanvasオブジェクト
    [SerializeField] private GameObject whiteScreenObject;

    //プレイヤーの移動速度とRotation速度
    [SerializeField] private float speed = 10f;

    //プレイヤーが走る前のデフォルトスピード
    [SerializeField] private float defaultSpeed = 10f;

    //ダッシュ時のスピード
    [SerializeField] private float maxSpeed = 13f;

    //ジャンプするときの力
    [SerializeField] private float jumpPower = 10f;

    //ナイフを投げる時の速度
    [SerializeField] private float translateSpeed = 10f;

    //プレイヤーのTransformとRigidbody
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;

    //カメラのTransform
    [SerializeField] private Transform cameraTransform;

    //ナイフを生成して飛ばすときのposition
    [SerializeField] private Transform translatePosition;

    //地面についているかを判定するLayer
    [SerializeField] private LayerMask groundLayer;

    //ナイフを生成するために参照するGameObject
    [SerializeField] public GameObject knifeObject;

    //プレイヤーの移動入力(moveInput)と視点回転入力(lookValue)
    private Vector2 moveInput;
    private Vector2 lookValue;

    //地面についているかの判定
    public bool onGround = true;
    private bool isRightTrigger = false;//攻撃判定
    private bool isDush = false;//ダッシュ判定
    private bool isInputRB = false;//RBの入力判定
    private bool isInputLB = false;//LBの入力判定
    public float rayLength = 0.2f;
    public float distance = 5f; // カメラとプレイヤー間の距離
    private float height = 2f;//カメラの高さ
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;

    //カメラの縦横回転の数値
    private float yaw, pitch;

    //新InputActionの割り当てしたそれぞれのActions
    //それぞれのActionsのメソッドを作ることで入力したときの処理ができたり
    //代入したりするとInputActionに設定した入力を取得できる
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction sceneMoveAction;

    //プレイヤーが所持しているナイフを格納するリスト
    [SerializeField] public List<GameObject> knifeObjectList = new List<GameObject>();
    //開始時ナイフを設定した数生成する際に使うbool
    private bool isInitialGenerate = false;

    //InputActionを自動で読み込むコンポーネントを取得
    [SerializeField] private PlayerInput playerInput;

    //ゲームパッドの取得
    private Gamepad gamepad;

    //複数人でやる際のプレイヤーの番号割り当てに使う変数
    public int playerNumber = 0;
    //public string groupName = "";
    //右トリガーを押して通常攻撃発動する値
    [SerializeField] private float triggerValue = 1.0f;

    // Animtorを取得
    //[SerializeField] private Animator animator = null;

    // プレイヤーの準備完了の判定
    public bool isReady = false;

    /// <summary>
    /// それぞれのプレイヤーをチームごとに割り当てる
    /// </summary>
    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();

        

        // 割り当てられたデバイス（コントローラ）を取得
        if (playerInput.devices.Count > 0)
        {
            gamepad = playerInput.devices[0] as Gamepad;
            Debug.Log("接続されました");
        }

        // プレイヤー番号を自動で割り当て
        playerNumber = playerInput.playerIndex;
        Debug.Log($"Player {playerNumber + 1} が参加しました！ 使用コントローラ: {gamepad?.displayName}");
        if (playerNumber <= 1)
        {
            this.gameObject.tag = "RedPlayer";//チーム分けするためtagを変更
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);//見分けやすくするためチームの色に変更
            Debug.Log($"{playerNumber}はTeamRedです");
        }
        else
        {
            this.gameObject.tag = "BluePlayer";
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);//見分けやすくするためチームの色に変更
            Debug.Log($"{playerNumber}はTeamBlueです");
        }
    }

    public void Domove(InputAction.CallbackContext context)
    {
        //performed、canceledコールバックを受け取る
        if (context.started) return;
        inputMove = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        sceneMoveAction = playerInput.actions["SceneMove"];
        attackAction = playerInput.actions["NomalAttack"];
        var lbAction = playerInput.actions["WeakSkill"];
        var rbAction = playerInput.actions["StrongSkill"];

        jumpAction.performed += OnJump;
        sceneMoveAction.performed += OnSceneMove;
        attackAction.performed += OnNomalAttack;
        attackAction.canceled += OnNomalAttack; // 押し離し両方見る
        lbAction.performed += OnLB;
        lbAction.canceled += OnLB;

        rbAction.performed += OnRB;
        rbAction.canceled += OnRB;

    }


    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

            if (isGrounded())
            {
                Debug.Log("処理");
                playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                //animator.SetTrigger("isJump");
                Jumping();
            }
        }
    }

    private void OnSceneMove(InputAction.CallbackContext callbackContext)
    {
        if (tutorialManager.isInTutorial)
        {
            if (callbackContext.performed)
            {
                isReady = !isReady;

                // Canvasオブジェクトの表示・非表示を切り替え
                if (isReady)
                {
                    whiteScreenObject.SetActive(true);
                }
                else if (!isReady)
                {
                    whiteScreenObject.SetActive(false);
                }
            }
        }
    }

    private void OnLB(InputAction.CallbackContext callbackContext)
    {
        isInputLB = callbackContext.performed; // true/false 自動で更新
        Debug.Log(isInputLB);
        if (callbackContext.performed)//強スキルコマンドを押したとき
        {
            isInputLB = true;
            StrongSkill();
            Debug.Log("StrongSkill");
        }
        else if (callbackContext.canceled)//弱スキルコマンドを離したとき
        {
            isInputLB = false;
        }
    }

    private void OnRB(InputAction.CallbackContext callbackContext)
    {
        isInputRB = callbackContext.performed;
        Debug.Log(isInputRB);
        if (callbackContext.performed)
        {
            isInputRB = true;
            WeakSkill();
            Debug.Log("WeakSkill");
        }
        else if (callbackContext.canceled)
        {
            isInputRB = false;
        }
    }

    private void OnNomalAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            isRightTrigger = true;
            var lbPressed = playerInput.actions["WeakSkill"].IsPressed();
            Debug.Log("LB押下状態: " + lbPressed);
            //Debug.Log("hit");
            if (lbPressed)
            {
                //WeakSkill(); // LBと同時押しなら弱スキル
                //Debug.Log("WeakSkill");
            }
            else
            {
                normalAttack(); // 単発なら通常攻撃
                Debug.Log("normalAttack");
            }
        }
        else if (callbackContext.canceled)
        {
            isRightTrigger = false;
        }
    }

    void Start()
    {
        whiteScreenObject.SetActive(false);


        isDush = false;
        GenerateKnife();
        //var gamepads = Gamepad.all;
        // デバイスが登録されているか確認
        if (playerInput != null && playerInput.devices.Count > 0)
        {
            gamepad = playerInput.devices[0] as Gamepad;
            if (gamepad != null)
            {
                Debug.Log($"Player {playerNumber + 1} が {gamepad.displayName} を使用中");
            }
            else
            {
                Debug.LogWarning($"Player {playerNumber + 1}: デバイスはあるがGamepadではありません。");
            }
        }
        else
        {
            Debug.LogWarning($"Player {playerNumber + 1}: PlayerInputにデバイスが割り当てられていません。");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // デバイス別に処理
        if (gamepad == null) return;

        //左スティック入力の更新
        //左スティック入力の値が入る
        moveInput = moveAction.ReadValue<Vector2>();

        // Animatorに値を渡す
        // 前後移動
        //animator.SetFloat("Vertical", moveInput.y);

        // 左右移動
        //animator.SetFloat("Horizontal", moveInput.x);

        //右トリガーを押したときの数値
        //右トリガーを押すほど数値が大きくなる(最大１)
        float rightTriggerValue = attackAction.ReadValue<float>();

        //右トリガーを押した値が一定数以上ならば通常攻撃する
        if (rightTriggerValue > triggerValue && !isRightTrigger)
        {
            normalAttack();
            Debug.Log("通常攻撃");
            isRightTrigger = true;
            //animator.SetTrigger("isThrow");
        }
        //Debug.Log($"Move入力: {moveAction.ReadValue<Vector2>()}");
        CheckKnifePickup();
    }

    private void FixedUpdate()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("playerRigidbodyが設定されていません！");
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        if (moveDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(moveDir);

        playerRigidbody.MoveRotation(Quaternion.Slerp(playerRigidbody.rotation, targetRot, speed * Time.fixedDeltaTime));

        //位置の更新
        //Vector3 move = moveDir * speed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDir * speed * Time.fixedDeltaTime);
        if (gamepad == null)
        {
            Debug.Log("gamepadがnullです");
            return;
        }
    }

    private void LateUpdate()
    {
        // 右スティック入力を取得
        lookValue = lookAction.ReadValue<Vector2>();

        // 回転を更新
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        // カメラの回転と位置
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 playerCenter = playerTransform.position + Vector3.up * height;

        //カメラの位置
        Vector3 targetPosition = playerCenter - cameraRot * Vector3.forward * distance;

        //カメラをプレイヤーに即座に追従
        cameraTransform.position = targetPosition;
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

    public void Jumping()
    {
        Debug.Log("jump");
        if (isGrounded())
        {
            // playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);

        }
    }

    private void GenerateKnife()//初期設定したナイフの数だけリストに加える
    {
        if (!isInitialGenerate)
        {
            for (int i = 0; i <= possessionNumber - 1; i++)
            {
                knifeObjectList.Add(knifeObject.gameObject);
            }
            isInitialGenerate = true;
        }
    }

    public void normalAttack()
    {
        //Debug.Log("通常攻撃！");
        Vector3 translatePos = playerTransform.position;
        translatePos.z += 1.5f;
        if (knifeObjectList.Count > 0)
        {
            //ナイフを指定したpositionに生成して飛ばす
            GameObject knife = Instantiate(knifeObject, translatePosition.position, translatePosition.rotation);

            if (this.gameObject.tag == "RedPlayer")
            {
                knife.tag = "RedKnife";
                Debug.Log(knife.tag);
            }
            else if (this.gameObject.tag == "BluePlayer")
            {
                knife.tag = "Blueknife";
                Debug.Log(knife.tag);
            }

            Rigidbody rigidbody = knife.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
                Debug.Log(knife.tag);
            }
            KnifeControllertr knifeControllertr = knife.GetComponent<KnifeControllertr>();

            knifeObjectList.RemoveAt(0);//ナイフを投げたらリストから削除
        }
    }

    protected virtual void WeakSkill()
    {
        Debug.Log("弱スキル");
    }

    protected virtual void StrongSkill()
    {
        Debug.Log("強スキル");
    }

    //当たり判定はtagを使ってhitしたらrayを飛ばして当たり判定を使う

    void CheckKnifePickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f); // 半径2.0の範囲を調べる
        foreach (var hit in hits)
        {
            if (hit.CompareTag("NotPossessionKnife") && knifeObjectList.Count < 5)
            {
                Debug.Log("ナイフを回収");
                knifeObjectList.Add(knifeObject.gameObject);
                Destroy(hit.gameObject);
                // 所持数を増やす処理もここで
            }
        }
    }
}
