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

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerMoveInput == Vector2.zero) return;

        //カメラの向きを基準にした移動方向を計算
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //左スティックの入力に基づいて移動方向を計算
        Vector3 moveDir = forward * playerMoveInput.y + right * playerMoveInput.x;
        moveDir.Normalize();

        //移動方向に向けた回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        //rigidbodyを通してカメラの向きを基準にプレイヤーを移動させる
        rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        //rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        //rigidbodyを通してプレイヤーの向きを左スティックが入力した方向にゆっくりと回転する
        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation,targetRotation,rotationSpeed*Time.fixedDeltaTime));

        //Debug.Log($"MoveDir: {moveDir}");
        //Debug.Log($"Pos Before: {rigidbody.position}");
    }
}
