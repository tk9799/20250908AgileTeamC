using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float defaultSpeed = 10f;
    [SerializeField] private float maxSpeed = 13f;//ダッシュ時のスピード
    [SerializeField] private float jump = 10f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private LayerMask groundLayer;
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
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw,pitch;
    [SerializeField] private InputActionReference _lookActionRef;
    [SerializeField] private InputActionReference _moveActionRef;


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
        _moveActionRef.action?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        // Actionを無効化する必要がある
        _lookActionRef.action.Disable();
        _moveActionRef.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDush = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        //入力取得（WASDや矢印キー）
        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveY = Input.GetAxisRaw("Vertical");
        //Vector3 move = new Vector3(moveX, 0, moveY).normalized * speed * Time.deltaTime;
        //Vector3 newPos = playerTransform.position + move;
        //playerTransform.position = newPos;

        //if (Input.GetKeyDown(KeyCode.JoystickButton5))
        //{
        //    normalAttack();
        //}

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

            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                Debug.Log("jump");
                if (isGrounded())
                {
                    playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
                }
               
            }
            // 接地判定
            //isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }
        //Debug.Log(speed);
    }

    void LateUpdate()
    {
        //右スティック入力を取得
        Vector2 lookValue=_lookActionRef.action.ReadValue<Vector2>();

        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        //pitch -= lookValue.y * rotationSpeed * Time.deltaTime;

        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // プレイヤーの位置にカメラを追従
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);

        cameraTransform.position = playerTransform.position + rotation * cameraOffset;
        // カメラはプレイヤーの頭あたりを見る
        cameraTransform.LookAt(playerTransform.position + Vector3.up * 1.5f);

        //Vector3 forward = cameraTransform.forward;
        // 左スティック入力で移動
        Vector2 moveValue = _moveActionRef.action.ReadValue<Vector2>();
        if (moveValue.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);
            playerTransform.position += moveDir * speed * Time.deltaTime;
            //playerTransform.rotation = Quaternion.LookRotation(forward);
        }

        Vector3 playerAngle = playerTransform.eulerAngles;
        playerAngle.x = 0f;
    }

    private bool isGrounded()
    {
        //RaycastHit hit = Physics.CheckSphere(playerTransform.position, groundDistance, groundLayer);
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength, groundLayer);
        //return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        // SceneビューでRayを可視化
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position + Vector3.down * rayLength);
    }


    public void normalAttack()
    {
        Debug.Log("通常攻撃！");
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
