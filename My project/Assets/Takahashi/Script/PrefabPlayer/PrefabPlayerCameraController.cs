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
    void LateUpdate()
    {
        if(playerLookInput.sqrMagnitude < 0.01f)
        {
            return;
        }

        yaw += playerLookInput.x * cameraRotationSpeed * Time.deltaTime;

        pitch -= playerLookInput.y * cameraRotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minRotate, maxRotate);

        Quaternion rotation=Quaternion.Euler(pitch, yaw, cameraZCoordinate);

        Vector3 targetPosition = targetTransform.position + Vector3.up * height - rotation * Vector3.forward * distance;

        transform.position = targetPosition;
        transform.rotation = rotation;

    }
}
