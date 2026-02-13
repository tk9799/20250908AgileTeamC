using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    //カメラのTransform
    [SerializeField] private Transform cameraTransform;

    //プレイヤーの回転速度
    [SerializeField] private float rotationSpeed = 0.0f;

    //プレイヤーの移動速度
    [SerializeField] private float moveSpeed = 0.0f;

    [SerializeField] private PrefabPlayerController prefabPlayerController;

    private Vector2 playerMoveInput;


    public void SetMoveInput(Vector2 input)
    {
        playerMoveInput = input;
    }


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
        //moveDir.Normalize();

        //移動方向に向けた回転を計算
        //Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            moveDir.Normalize();

            rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }

        //プレイヤーの状態で移動方法を切り替えるメソッドを呼び出す
        RotatePlayer(moveDir);
    }

    /// <summary>
    /// プレイヤーが移動状態を切り替える処理
    /// </summary>
    private void RotatePlayer(Vector3 moveDir)
    {
        Debug.Log("処理");
        Quaternion targetRotation;

        if (prefabPlayerController != null && prefabPlayerController.isLetTriggerInput)
        {
            //カメラの正面方向を取得
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0.0f;

            if (cameraForward.sqrMagnitude < 0.01f)
            {
                return;
            }

            //プレイヤーが常にカメラの正面を向くように回転する
            targetRotation = Quaternion.LookRotation(cameraForward);
        }
        else
        {
            if (moveDir.sqrMagnitude < 0.01f)
            {
                return;
            }

            //プレイヤーが左スティック入力の方向に回転
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, 
            targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}
