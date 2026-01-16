using Unity.VisualScripting;
using UnityEngine;

public class PrefabPlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float cameraRotationSpeed = 5f;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDistance = transform.position - targetTransform.position;
    }

    public void SetLookInput(Vector2 input)
    {
        playerLookInput = input;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransform.position + playerDistance;

        // 入力されている場合、プレイヤーとカメラの距離を回転させる
        if (playerLookInput != Vector2.zero)
        {
            Quaternion cameraRotation = Quaternion.Euler(0, playerLookInput.x * cameraRotationSpeed, 0);
            playerDistance = cameraRotation * playerDistance;
        }

        //// 回転を更新
        //yaw += playerLookInput.x * cameraRotationSpeed * Time.deltaTime;
        //pitch -= playerLookInput.y * cameraRotationSpeed * Time.deltaTime;
        //pitch = Mathf.Clamp(pitch, -minRotate, maxRotate);

        //// カメラの回転と位置
        //Quaternion cameraRot = Quaternion.Euler(pitch, yaw, cameraZCoordinate);

        ////プレイヤーの高さを調整
        //Vector3 playerCenter = targetTransform.position + Vector3.up * height;

        ////カメラの位置
        //Vector3 targetPosition = playerCenter - cameraRot * Vector3.forward * distance;

        ////カメラをプレイヤーに即座に追従
        //transform.position = targetPosition;
        //transform.rotation = cameraRot;
    }
}
