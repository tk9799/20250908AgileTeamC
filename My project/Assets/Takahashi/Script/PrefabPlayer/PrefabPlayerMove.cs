using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody;

    //カメラのTransform
    [SerializeField] private Transform cameraTransform;

    //プレイヤーの回転速度
    [SerializeField] private float rotationSpeed = 0.0f;

    //プレイヤーの移動速度
    [SerializeField] private float moveSpeed = 0.0f;

    private Vector2 playerMoveInput;


    public void SetMoveInput(Vector2 input)
    {
        playerMoveInput = input;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerMoveInput == Vector2.zero) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * playerMoveInput.y + right * playerMoveInput.x;
        moveDir.Normalize();

        rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        Quaternion targetRotation=Quaternion.LookRotation(moveDir);

        //rigidbodyを通してプレイヤーの向きを左スティックが入力した方向にゆっくりと回転する
        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation,targetRotation,rotationSpeed*Time.fixedDeltaTime));

        Debug.Log($"MoveDir: {moveDir}");
        Debug.Log($"Pos Before: {rigidbody.position}");
    }
}
