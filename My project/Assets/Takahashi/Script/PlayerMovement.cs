using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;

    //カメラのTransform
    [SerializeField] private Transform cameraTransform;

    //プレイヤーの移動速度とRotation速度
    [SerializeField] private float speed = 10f;

    //プレイヤーが走る前のデフォルトスピード
    [SerializeField] private float defaultSpeed = 10f;

    //ダッシュ時のスピード
    [SerializeField] private float maxSpeed = 13f;

    //ジャンプするときの力
    [SerializeField] private float jumpPower = 10f;

    //地面についているかを判定するLayer
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float rayLength = 0.2f;

    //ゲームパッドの取得
    private Gamepad gamepad;

    //プレイヤーの移動入力
    private Vector2 moveInput;

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
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
        playerRigidbody.MoveRotation(Quaternion.Slerp(playerRigidbody.rotation, targetRot, speed * Time.fixedDeltaTime));

        //位置の更新・移動
        playerRigidbody.MovePosition(playerRigidbody.position + moveDir * speed * Time.fixedDeltaTime);
        if (gamepad == null)
        {
            Debug.Log("gamepadがnullです");
            return;
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            Debug.Log("処理");

            //プレイヤーの上方向に力を加える
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //animator.SetTrigger("isJump");
            //Jumping();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
    }
}
