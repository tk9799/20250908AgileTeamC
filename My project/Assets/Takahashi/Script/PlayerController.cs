using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //private Vector2 inputMove;
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
    [SerializeField] private float translateSpeed = 1f;

    //プレイヤーのTransformとRigidbody
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;

    //カメラのTransform
    [SerializeField] private Transform cameraTransform;

    // チュートリアルマネージャーの取得
    [SerializeField] private TutorialManager tutorial;

    //ナイフを生成して飛ばすときのposition
    [SerializeField] private Transform translatePosition;

    //地面についているかを判定するLayer
    [SerializeField] private LayerMask groundLayer;

    //ナイフを生成するために参照するGameObject
    [SerializeField] public GameObject knifeObject;

    //プレイヤーの移動入力(moveInput)と視点回転入力(lookValue)
    private Vector2 moveInput;
    private Vector2 lookValue;

    private ResultSceneManager resultSceneManager = new ResultSceneManager();

    //地面についているかの判定
    public bool onGround = true;

    //攻撃判定
    private bool isRightTrigger = false;

    //ダッシュ判定
    private bool isDush = false;

    //RBの入力判定
    private bool isInputRB = false;

    //LBの入力判定
    private bool isInputLB = false;
    public float rayLength = 0.2f;

    // カメラとプレイヤー間の距離
    public float distance = 5f;

    //カメラの高さ
    private float height = 2f;

    //プレイヤーの回転速度
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

    //ナイフを回収する範囲
    private float knifeCollectRange = 2.0f;

    //ナイフの最大所持数
    //Listで所持数を数えているため処理ではListの配列の数が６未満で処理される
    private int maxKnifePossessionsCount = 5;

    //InputActionを自動で読み込むコンポーネントを取得
    [SerializeField] private PlayerInput playerInput;

    //ゲームパッドの取得
    private Gamepad gamepad;

    //複数人でやる際のプレイヤーの番号割り当てに使う変数
    public int playerNumber = 0;
    //public string groupName = "";
    //右トリガーを押して通常攻撃発動する値
    [SerializeField] private float triggerValue = 1.0f;

    // プレイヤーの準備完了の判定
    public bool isReady = false;

    // 生成したい距離
    private float spawnDistance = 2.0f;

    //割り当てられたコントローラーの最低数量
    private int minDevicesCount = 1;

    //回転の上限値と最低値
    private float maxRotate = 60.0f;
    private float minRotate = -20.0f;

    //カメラのz座標数値
    private float cameraZCoordinate = 0.0f;

    private float currentPlayerRotationY = 0.0f;

    /// <summary>
    /// それぞれのプレイヤーをチームごとに割り当てる
    /// </summary>
    private void Awake()
    {
        // チーム分けをタグによって行う
        if (this.gameObject.tag == "RedPlayer")
        {
            // RedPlayerタグであれば、赤色に変更
            //見分けやすくするためチームの色に変更
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
        else if (this.gameObject.tag == "BluePlayer")
        {
            // BluePlayerタグであれば、青色に変更
            //見分けやすくするためチームの色に変更
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        }

        // 割り当てられたデバイス（コントローラ）を取得
        if (playerInput.devices.Count >= minDevicesCount)
        {
            gamepad = playerInput.devices[0] as Gamepad;
            Debug.Log("接続されました");
        }
    }

    /// <summary>
    /// InputActionの登録
    /// </summary>
    private void OnEnable()
    {
        //InputActionの取得し、Action内で入力の設定して追加した名前を取得
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        sceneMoveAction = playerInput.actions["TutorialSceneMove"];
        attackAction = playerInput.actions["NomalAttack"];
        var lbAction = playerInput.actions["WeakSkill"];
        var rbAction = playerInput.actions["StrongSkill"];

        //それぞれのInputActionをメソッドに登録
        jumpAction.performed += OnJump;
        sceneMoveAction.performed += OnSceneMove;
        attackAction.performed += OnNomalAttack;
        attackAction.canceled += OnNomalAttack; // 押し離し両方見る
        lbAction.performed += OnLB;

        //LBボタンを離したときに呼び出されるメソッド
        lbAction.canceled += OnLB;

        rbAction.performed += OnRB;

        //RBボタンを離したときに呼び出されるメソッド
        rbAction.canceled += OnRB;
    }

    /// <summary>
    /// ジャンプ入力を押したときの処理
    /// </summary>
    private void OnJump(InputAction.CallbackContext ctx)
    {
        //ジャンプ入力を押したとき
        if (ctx.performed)
        {
            //地面の接地判定
            if (isGrounded())
            {
                Debug.Log("処理");

                //プレイヤーの上方向に力を加える
                playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                //animator.SetTrigger("isJump");
                Jumping();
            }
        }
    }

    /// <summary>
    /// 準備完了を表示するUIを表示非表示切り替える処理
    /// </summary>
    private void OnSceneMove(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            //boolのtrue/falseを切り替える
            isReady = !isReady;
            Debug.Log(isReady);

            // Canvasオブジェクトの表示・非表示を切り替え
            if (tutorial.isInTutorial && isReady)
            {
                Debug.Log("準備完了");
                whiteScreenObject.SetActive(true);
            }
            else if (!isReady)
            {
                whiteScreenObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// LBボタンを押したとき強スキルを発動する処理
    /// </summary>
    private void OnLB(InputAction.CallbackContext callbackContext)
    {
        // true/false 自動で更新
        isInputLB = callbackContext.performed;
        Debug.Log(isInputLB);

        //強スキルコマンドを押したとき
        if (callbackContext.performed)
        {
            isInputLB = true;
            StrongSkill();
            Debug.Log("StrongSkill");
        }
        //強スキルコマンドを離したとき
        else if (callbackContext.canceled)
        {
            isInputLB = false;
        }
    }

    /// <summary>
    /// RBボタンを押したとき弱スキルを発動する処理
    /// </summary>
    private void OnRB(InputAction.CallbackContext callbackContext)
    {
        // true/false 自動で更新
        isInputRB = callbackContext.performed;
        Debug.Log(isInputRB);

        //弱スキルコマンドを押したとき
        if (callbackContext.performed)
        {
            isInputRB = true;
            WeakSkill();
            Debug.Log("WeakSkill");
        }
        //弱スキルコマンドを離したとき
        else if (callbackContext.canceled)
        {
            isInputRB = false;
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void OnNomalAttack(InputAction.CallbackContext callbackContext)
    {
        //入力を検知した場合
        if (callbackContext.performed)
        {
            isRightTrigger = true;
            var lbPressed = playerInput.actions["WeakSkill"].IsPressed();
            Debug.Log("LB押下状態: " + lbPressed);
            //Debug.Log("hit");
            if (lbPressed)
            {

            }
            else
            {
                // 単発なら通常攻撃
                normalAttack();
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
        //準備完了を表示するUIを非表示
        whiteScreenObject.SetActive(false);

        isDush = false;

        //ナイフの初期生成
        GenerateKnife();

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

    /// <summary>
    /// 入力の取得・GamePadのトリガー入力時の計算、メソッドの呼び出し・
    /// ナイフ回収の呼び出しをしている処理
    /// </summary>
    void Update()
    {
        // デバイス別に処理
        if (gamepad == null) return;

        //左スティック入力の更新
        //左スティック入力の値が入る
        moveInput = moveAction.ReadValue<Vector2>();

        //右トリガーを押したときの数値
        //右トリガーを押すほど数値が大きくなる(最大１)
        float rightTriggerValue = attackAction.ReadValue<float>();

        //右トリガーを押した値が一定数以上ならば通常攻撃する
        if (rightTriggerValue > triggerValue && !isRightTrigger)
        {
            normalAttack();
            Debug.Log("通常攻撃");
            isRightTrigger = true;
        }

        //ナイフを回収するメソッド
        CheckKnifePickup();

        
            
        
    }

    /// <summary>
    /// プレイヤーの移動・カメラの正面を基準にプレイヤーの正面を更新する処理
    /// </summary>
    private void FixedUpdate()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("playerRigidbodyが設定されていません！");
        }

        //カメラの正面方向を取得
        Vector3 forward = cameraTransform.forward;

        //カメラのX方向を取得
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


        //進行方向に回転する情報
        Quaternion targetRot = Quaternion.LookRotation(moveDir);

        //進行方向にプレイヤーの体を向ける
        playerRigidbody.MoveRotation(Quaternion.Slerp(playerRigidbody.rotation,
            targetRot, speed * Time.fixedDeltaTime));

        //位置の更新・移動
        playerRigidbody.MovePosition(playerRigidbody.position + moveDir *
            speed * Time.fixedDeltaTime);




        if (gamepad == null)
        {
            Debug.Log("gamepadがnullです");
            return;
        }
    }

    /// <summary>
    /// カメラの回転・カメラをプレイヤーの後ろに配置して追従する処理
    /// </summary>
    private void LateUpdate()
    {
        // 右スティック入力を取得
        lookValue = lookAction.ReadValue<Vector2>();

        // 回転を更新
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -minRotate, maxRotate);

        // カメラの回転と位置
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, cameraZCoordinate);

        //プレイヤーの
        Vector3 playerCenter = playerTransform.position + Vector3.up * height;

        //カメラの位置
        Vector3 targetPosition = playerCenter - cameraRot * Vector3.forward * distance;

        //カメラをプレイヤーに即座に追従
        cameraTransform.position = targetPosition;
        cameraTransform.rotation = cameraRot;
    }

    /// <summary>
    /// 地面に足がついているかの判定
    /// </summary>
    private bool isGrounded()
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength, groundLayer);
    }

    /// <summary>
    /// RayをScene上だけ可視化
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // SceneビューでRayを可視化
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position +
            Vector3.down * rayLength);
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
        //プレイヤー前方の座標を取得
        Vector3 translatePos = playerTransform.position + playerTransform.forward * spawnDistance;
        if (knifeObjectList.Count > 0)
        {
            //ナイフを指定したpositionに生成して飛ばす
            GameObject knife = Instantiate(knifeObject, translatePos, translatePosition.rotation);

            //投げたプレイヤーによってTag名を変える
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

            //ナイフをプレイヤーの正面に力を加えて飛ばす
            Rigidbody rigidbody = knife.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
                Debug.Log(knife.tag);
            }
            KnifeControllertr knifeControllertr = knife.GetComponent<KnifeControllertr>();

            //ナイフを投げたらリストから削除
            knifeObjectList.RemoveAt(0);
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


    /// <summary>
    /// ナイフを回収・所持数を増やす
    /// </summary>
    void CheckKnifePickup()
    {
        //当たり判定はtagを使ってhitしたらrayを飛ばして当たり判定を使う
        // 半径2.0の範囲を調べる
        Collider[] hits = Physics.OverlapSphere(transform.position, knifeCollectRange);
        foreach (var hit in hits)
        {
            //ナイフの所持者が誰のものでもなく所持数が上限未満の場合ナイフを追加する
            if (hit.CompareTag("NotPossessionKnife") && knifeObjectList.Count <
                maxKnifePossessionsCount)
            {
                Debug.Log("ナイフを回収");
                knifeObjectList.Add(knifeObject.gameObject);
                Destroy(hit.gameObject);
                // 所持数を増やす処理もここで行う
            }
        }
    }
}
