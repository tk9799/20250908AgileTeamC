using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�i�C�t�̏����l")]
    [SerializeField] public int possessionNumber = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float defaultSpeed = 10f;
    [SerializeField] private float maxSpeed = 13f;//�_�b�V�����̃X�s�[�h
    [SerializeField] private float jump = 10f;
    [SerializeField] private float translateSpeed = 10f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform translatePosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject knifeObject;//�i�C�t�I�u�W�F�N�g
    //�v���C���[���猩�����ΓI�Ȉʒu�i�����E�p�x�j��\��
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -4);
    //[SerializeField] private Vector3 cameraPosition;
    public bool onGround = true;
    public Transform groundCheck;   // �����̔���ʒu
    private Gamepad gamepad;
    private bool isRightTrigger = false;//�U������
    private bool isNomalAttack = false;
    private bool isGround = false;
    private bool isDush = false;//�_�b�V������
    private float groundCheckOffsetY = 0.2f;
    private float groundCheckRadius = 0.2f;
    private float groundCheckDistance = 0.2f;
    public float groundDistance = 0.2f;
    public float rayLength = 0.2f;
    public float distance=5f; // �J�����ƃv���C���[�Ԃ̋���
    private float height=2f;//�J�����̍���
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw,pitch;
    [SerializeField] private InputActionReference _lookActionRef;
    [SerializeField] private InputActionReference _moveActionRef;
    [SerializeField] private List<GameObject> knifeObjectList = new List<GameObject>();
    private bool isInitialGenerate = false;//������������ۂ�bool


    public void Domove(InputAction.CallbackContext context)
    {
        //performed�Acanceled�R�[���o�b�N���󂯎��
        if (context.started) return;

        //Move�A�N�V�����̓��͎擾
        var inputMove = context.ReadValue<Vector2>();
        var look= context.ReadValue<Quaternion>();
    }

    private void OnEnable()
    {
        // InputAction��L����
        // ��������Ȃ��Ɠ��͂��󂯎��Ȃ����Ƃɒ���
        _lookActionRef.action?.Enable();
        _moveActionRef.action.Enable();
    }

    // ������
    private void OnDisable()
    {
        // ���g�������������^�C�~���O�Ȃǂ�
        // Action�𖳌�������K�v������
        _lookActionRef.action.Disable();
        _moveActionRef.action.Disable();
    }

    void Start()
    {
        isDush = false;
        GenerateKnife();
    }

    // Update is called once per frame
    void Update()
    {
        // ���ݐڑ�����Ă���Gamepad���擾
        gamepad = Gamepad.current;
        //gamepad = Gamepad.all[0];//�����ڑ��̎��Ɏg�p����
        if (gamepad == null) return;

        //���X�e�B�b�N�ňړ�
        Vector2 moveValue = _moveActionRef.action.ReadValue<Vector2>();

        if (moveValue.sqrMagnitude > 0.01f)
        {
            // �u�J�����̑O�����v����Ɉړ�
            Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);

            Vector3 targetPos = playerRigidbody.position + moveDir * speed * Time.deltaTime;
            playerRigidbody.MovePosition(targetPos);
        }

        if (Gamepad.current != null)
        {
            // �E�g���K�[�̒l�i0.0�`1.0�j
            float rt = Gamepad.current.rightTrigger.ReadValue();
            float lt = Gamepad.current.leftTrigger.ReadValue();

            //�E�g���K�[�̓��͂����ė�������//�����̏d����h������isNomalAttack��true�̎�
            if (Gamepad.current.rightTrigger.wasReleasedThisFrame&&isNomalAttack)
            {
                normalAttack();//�ʏ�U��
                Debug.Log("RT �����ꂽ: " + rt);
                isRightTrigger = true;
            }
            //RT��RB�̓�������
            else if (Input.GetKeyUp(KeyCode.JoystickButton5) && rt > 0.5f)
            {
                WeakSkill();//��X�L��
                isNomalAttack = false;
            }
            else if (Input.GetKeyUp(KeyCode.JoystickButton4) && rt > 0.5f)
            {
                StrongSkill();//���X�L��
                isNomalAttack = false;
            }
            if (rt == 0)
            {
                isRightTrigger = false;
                isNomalAttack = true;
            }

            if (lt > 0.5f&&!isDush)//�_�b�V��
            {
                speed = maxSpeed;
                //playerTransform.position = newPos;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                isDush = true;
                Debug.Log(maxSpeed);
            }
            else
            {
                speed = defaultSpeed;
                //playerTransform.position = newPos;
                isDush = false;
            }

            //�W�����v����
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                Debug.Log("jump");
                if (isGrounded())
                {
                    playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
                }
            }
        }
    }

    void LateUpdate()
    {
        // �E�X�e�B�b�N���͂��擾
        Vector2 lookValue = _lookActionRef.action.ReadValue<Vector2>();

        // Y����]�������f�i���E��]�j
        yaw += lookValue.x * rotationSpeed * Time.deltaTime;//���E��]���v�Z

        // �v���C���[����]������
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);
        pitch -= lookValue.y * rotationSpeed * Time.deltaTime;//�㉺��]���v�Z
        pitch = Mathf.Clamp(pitch, -20f, 60f); // �㉺�̐���
        // �J�����̉�]���v�Z
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 playerCenter = playerTransform.position + Vector3.up * height;//�v���C���[�̒��S�ʒu���v�Z
        Vector3 targetPosition = playerCenter - playerTransform.forward * distance;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, speed * Time.deltaTime);
        cameraTransform.rotation= cameraRot;

        //�v���C���[��ŃI�t�Z�b�g��������
        cameraTransform.position = playerTransform.position + playerTransform.rotation * cameraOffset;
        // �J�����̓v���C���[������
        cameraTransform.LookAt(playerTransform.position + Vector3.up);

    }

    private bool isGrounded()//�n�ʂɑ������Ă��邩�̔���Ɏg����
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength, groundLayer);
    }

    //Ray��Scene�ゾ������
    private void OnDrawGizmosSelected()
    {
        // Scene�r���[��Ray������
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position + Vector3.down * rayLength);
    }

    private void GenerateKnife()//�����ݒ肵���i�C�t�̐��������X�g�ɉ�����
    {
        if (!isInitialGenerate)
        {
            for (int i = 0; i <= possessionNumber-1; i++)
            {
                knifeObjectList.Add(knifeObject);
            }
            isInitialGenerate = true;
        }
    }

    public void normalAttack()
    {
        Debug.Log("�ʏ�U���I");
        if (knifeObjectList.Count > 0)
        {
            //�i�C�t���w�肵��position�ɐ������Ĕ�΂�
            GameObject knife = Instantiate(knifeObject, translatePosition.position, translatePosition.rotation);
            Rigidbody rigidbody = knife.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
            }
        }
    }

    public void WeakSkill()
    {
        Debug.Log("��X�L��");
    }

    public void StrongSkill()
    {
        Debug.Log("���X�L��");
    }
}
