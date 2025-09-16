using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform myCamera;
    [SerializeField] public Vector3 cameraOffset;
    [SerializeField] private Transform playerTransform;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw, pitch;

    private PlayerInput playerInput;
    //private InputAction lookAction;
    // Actionをインスペクターから編集できるようにする
    [SerializeField] private InputActionReference _lookActionRef;

    // 有効化
    //private void OnEnable()
    //{
    //    // InputActionを有効化
    //    // これをしないと入力を受け取れないことに注意
    //    _lookActionRef.action?.Enable();
    //}

    //// 無効化
    //private void OnDisable()
    //{
    //    // 自身が無効化されるタイミングなどで
    //    // Actionを無効化する必要がある
    //    _lookActionRef.action.Disable();
    //}

    void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Vector3 desiredPosition = playerTransform.position + cameraOffset;
        //myCamera.position = Vector3.Lerp(myCamera.position, desiredPosition, Time.deltaTime * rotationSpeed);

        //// 2. カメラの向きにプレイヤーを同期
        //playerTransform.rotation = Quaternion.Euler(0f, myCamera.rotation.eulerAngles.y, 0f);

        //myCamera.LookAt(playerTransform.position + Vector3.up * 1.5f);
        //Vector2 look = _lookActionRef.action.ReadValue<Vector2>();//右スティックの入力を取得
        //Debug.Log($"Right Stick: {look}");
        //yaw += look.x * mouseSensitivity;
        //pitch -= look.y * mouseSensitivity;
        //pitch = Mathf.Clamp(pitch, -80f, 80f);

        //Quaternion cameraRotation = Quaternion.Euler(0, pitch, 0);//回転を反映
        //myCamera.rotation = cameraRotation;
        Vector3 desiredPosition = playerTransform.position + cameraOffset;// カメラの位置をプレイヤーに追従させる
        //プレイヤーに追従
        myCamera.position = desiredPosition;
        Vector3 lookDir = myCamera.forward;// プレイヤーをカメラの向きに合わせて回転させる

        //lookDir.y = 0;
        //if (lookDir.sqrMagnitude > 0.001f)
        //{
        //    playerTransform.rotation = Quaternion.LookRotation(lookDir);
        //}
        //playerTransform.forward = myCamera.forward;
    }

}
