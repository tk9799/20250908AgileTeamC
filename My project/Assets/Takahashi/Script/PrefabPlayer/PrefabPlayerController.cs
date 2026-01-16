using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Camera playerCamera;
    private Gamepad gamepad;

    private static int playerCount = 0;//生成されたプレイヤー数をカウント

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //プレイヤーごとにカメラを生成
        GameObject cameraObject = new GameObject("PlayerCamera");
        playerCamera = cameraObject.AddComponent<Camera>();
        //生成したカメラを子オブジェクトにする
        cameraObject.transform.SetParent(transform);
    }

    // Move入力を受け取る
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Jump入力を受け取る
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        //移動処理
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
}
