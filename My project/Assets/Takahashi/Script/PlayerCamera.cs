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
    // Action���C���X�y�N�^�[����ҏW�ł���悤�ɂ���
    [SerializeField] private InputActionReference _lookActionRef;

    // �L����
    //private void OnEnable()
    //{
    //    // InputAction��L����
    //    // ��������Ȃ��Ɠ��͂��󂯎��Ȃ����Ƃɒ���
    //    _lookActionRef.action?.Enable();
    //}

    //// ������
    //private void OnDisable()
    //{
    //    // ���g�������������^�C�~���O�Ȃǂ�
    //    // Action�𖳌�������K�v������
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

        //// 2. �J�����̌����Ƀv���C���[�𓯊�
        //playerTransform.rotation = Quaternion.Euler(0f, myCamera.rotation.eulerAngles.y, 0f);

        //myCamera.LookAt(playerTransform.position + Vector3.up * 1.5f);
        //Vector2 look = _lookActionRef.action.ReadValue<Vector2>();//�E�X�e�B�b�N�̓��͂��擾
        //Debug.Log($"Right Stick: {look}");
        //yaw += look.x * mouseSensitivity;
        //pitch -= look.y * mouseSensitivity;
        //pitch = Mathf.Clamp(pitch, -80f, 80f);

        //Quaternion cameraRotation = Quaternion.Euler(0, pitch, 0);//��]�𔽉f
        //myCamera.rotation = cameraRotation;
        Vector3 desiredPosition = playerTransform.position + cameraOffset;// �J�����̈ʒu���v���C���[�ɒǏ]������
        //�v���C���[�ɒǏ]
        myCamera.position = desiredPosition;
        Vector3 lookDir = myCamera.forward;// �v���C���[���J�����̌����ɍ��킹�ĉ�]������

        //lookDir.y = 0;
        //if (lookDir.sqrMagnitude > 0.001f)
        //{
        //    playerTransform.rotation = Quaternion.LookRotation(lookDir);
        //}
        //playerTransform.forward = myCamera.forward;
    }

}
