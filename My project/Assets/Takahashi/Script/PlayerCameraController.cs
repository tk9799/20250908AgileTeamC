using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    // カメラとプレイヤー間の距離
    public float distance = 5f;

    //カメラの高さ
    private float height = 2f;

    [SerializeField] private float rotationSpeed = 100f;

    //カメラの縦横回転の数値
    private float yaw, pitch;

    //視点回転入力
    private Vector2 lookValue;

    public void SetLookInput(Vector2 input)
    {
        lookValue = input;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotateCsmera();
    }

    private void RotateCsmera()
    {
        // 回転を更新
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        // カメラの回転と位置
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);

        //プレイヤーの
        Vector3 playerCenter = playerTransform.position + Vector3.up * height;

        //カメラの位置
        Vector3 targetPosition = playerCenter - cameraRot * Vector3.forward * distance;

        //カメラをプレイヤーに即座に追従
        transform.position = targetPosition;
        transform.rotation = cameraRot;
    }
}
