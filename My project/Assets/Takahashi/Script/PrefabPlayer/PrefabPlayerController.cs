using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PrefabPlayerController : MonoBehaviour
{
    //[SerializeField] private PlayerInput playerInput;
    [SerializeField] public Rigidbody rigidbody;

    private PrefabPlayerMove prefabPlayerMove;

    private PrefabPlayerCameraController prefabPlayerCameraController;

    [SerializeField] private PrefabPlayerJumpScript prefabPlayerJumpScript;

    [SerializeField] private PrefabPlayerNomalAttack prefabPlayernomalAttack;

    //[SerializeField] 
    private Vector2 moveInput;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    void Awake()
    {
        prefabPlayerMove = GetComponent<PrefabPlayerMove>();
        prefabPlayerCameraController = GetComponentInChildren<PrefabPlayerCameraController>();
        prefabPlayerJumpScript = GetComponent<PrefabPlayerJumpScript>();
    }

    // Move入力を受け取る
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }

        //Debug.Log($"Move Input: {moveInput}");
        prefabPlayerMove.SetMoveInput(moveInput);
    }

    /// <summary>
    /// InputActionのLookの入力を受け取った時に処理するメソッド
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        prefabPlayerCameraController.SetLookInput(lookInput);

    }

    /// <summary>
    /// InputActionのJumpの入力を受け取った時に処理するメソッド
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ジャンプ入力");
            prefabPlayerJumpScript.PlayerJump();
        }
        
    }

    public void OnNomalAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            prefabPlayernomalAttack.NormalAttack();
        }
    }

}
