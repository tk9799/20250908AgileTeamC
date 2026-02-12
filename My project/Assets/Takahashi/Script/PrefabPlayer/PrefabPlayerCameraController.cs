using Unity.VisualScripting;
using UnityEngine;

public class PrefabPlayerCameraController : MonoBehaviour
{
    [Header("追従するオブジェクト")]
    [SerializeField] private Transform targetTransform;

    [Header("カメラの回転速度")]
    [SerializeField] private float cameraRotationSpeed = 5f;

    //エイム状態のカメラを横に傾ける数値
    [Header("エイム状態のカメラを横に傾ける数値")]
    [SerializeField] private float offset = 0.0f;

    private Vector2 playerLookInput;

    //プレイヤーとカメラの距離
    private Vector3 playerDistance;

    //カメラの縦横回転の数値
    private float yaw, pitch;

    //回転の上限値と最低値
    private float maxRotate = 60.0f;
    private float minRotate = -20.0f;

    //カメラの高さ
    private float height = 2f;

    // カメラとプレイヤー間の距離
    public float distance = 5f;

    //カメラのz座標数値
    private float cameraZCoordinate = 0.0f;
    

    private void Start()
    {
        playerDistance = transform.position - targetTransform.position;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    public void SetLookInput(Vector2 input)
    {
        playerLookInput = input;
    }


    // Update is called once per frame
    private void LateUpdate()
    {
        //右スティック入力がある場合のみ回転を更新する
        if(playerLookInput.sqrMagnitude >= 0.01f)
        {
            //カメラの横回転を計算
            yaw += playerLookInput.x * cameraRotationSpeed * Time.deltaTime;

            //カメラの縦回転を計算
            pitch -= playerLookInput.y * cameraRotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minRotate, maxRotate);
        }

        //カメラの位置と回転を毎フレーム更新する
        //カメラの計算した縦横回転数値分カメラを回転する
        Quaternion rotation=Quaternion.Euler(pitch, yaw, cameraZCoordinate);

        Vector3 targetPosition = targetTransform.position + Vector3.up * height - rotation *
            Vector3.forward * distance+rotation*Vector3.right*offset;

        transform.position = targetPosition;
        transform.rotation = rotation;

    }
}
