using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private PlayerInput playerInput;

    private InputAction moveAction;

    //ゲームパッドの取得
    private Gamepad gamepad;

    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 leftStickInput = moveAction.ReadValue<Vector2>();
        //Vector3 playerMove = new Vector3(leftStickInput.x, leftStickInput.y, 0);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * leftStickInput.y + right * leftStickInput.x;
    }
}
