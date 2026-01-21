using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PrefabPlayerController : MonoBehaviour
{
    //[SerializeField] private PlayerInput playerInput;

    private PrefabPlayerMove prefabPlayerMove;

    private PrefabPlayerCameraController prefabPlayerCameraController;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private static int playerCount = 0;//生成されたプレイヤー数をカウント

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Awake()
    {
        prefabPlayerMove = GetComponent<PrefabPlayerMove>();
        prefabPlayerCameraController = GetComponentInChildren<PrefabPlayerCameraController>();
    }

    // Move入力を受け取る
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            //return;
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }

        Debug.Log($"Move Input: {moveInput}");
        prefabPlayerMove.SetMoveInput(moveInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            return;
        }

        Vector2 lookInput = context.ReadValue<Vector2>();
        prefabPlayerCameraController.SetLookInput(lookInput);

    }

}
