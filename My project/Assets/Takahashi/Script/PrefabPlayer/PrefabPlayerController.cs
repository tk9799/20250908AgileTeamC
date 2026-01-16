using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerController : MonoBehaviour
{
    //[SerializeField] private PlayerInput playerInput;

    private PrefabPlayerMove prefabPlayerMove;

    private PrefabPlayerCameraController prefabPlayerCameraController;
    private Vector2 moveInput;

    private static int playerCount = 0;//生成されたプレイヤー数をカウント

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Awake()
    {
        prefabPlayerMove = GetComponent<PrefabPlayerMove>();
        prefabPlayerCameraController = GetComponentInChildren<PrefabPlayerCameraController>();
    }

    // Move入力を受け取る
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        prefabPlayerMove.SetMoveInput(moveInput);
    }

    public void OnLook(InputValue value)
    {
        Vector2 lookInput = value.Get<Vector2>();

    }

}
