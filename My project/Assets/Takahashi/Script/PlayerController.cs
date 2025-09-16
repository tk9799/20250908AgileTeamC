using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float defaultSpeed = 10f;
    [SerializeField] private float maxSpeed = 13f;//�_�b�V�����̃X�s�[�h
    [SerializeField] private float jump = 10f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private LayerMask groundLayer;
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
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private float rotationSpeed = 100f;
    private float yaw,pitch;
    [SerializeField] private InputActionReference _lookActionRef;
    [SerializeField] private InputActionReference _moveActionRef;


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
        _moveActionRef.action?.Enable();
    }

    // ������
    private void OnDisable()
    {
        // ���g�������������^�C�~���O�Ȃǂ�
        // Action�𖳌�������K�v������
        _lookActionRef.action.Disable();
        _moveActionRef.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDush = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ݐڑ�����Ă���Gamepad���擾
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        //���͎擾�iWASD����L�[�j
        //float moveX = Input.GetAxisRaw("Horizontal");
        //float moveY = Input.GetAxisRaw("Vertical");
        //Vector3 move = new Vector3(moveX, 0, moveY).normalized * speed * Time.deltaTime;
        //Vector3 newPos = playerTransform.position + move;
        //playerTransform.position = newPos;

        //if (Input.GetKeyDown(KeyCode.JoystickButton5))
        //{
        //    normalAttack();
        //}

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

            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                Debug.Log("jump");
                if (isGrounded())
                {
                    playerRigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
                }
               
            }
            // �ڒn����
            //isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }
        //Debug.Log(speed);
    }

    void LateUpdate()
    {
        //�E�X�e�B�b�N���͂��擾
        Vector2 lookValue=_lookActionRef.action.ReadValue<Vector2>();

        yaw += lookValue.x * rotationSpeed * Time.deltaTime;
        //pitch -= lookValue.y * rotationSpeed * Time.deltaTime;

        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // �v���C���[�̈ʒu�ɃJ������Ǐ]
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);

        cameraTransform.position = playerTransform.position + rotation * cameraOffset;
        // �J�����̓v���C���[�̓������������
        cameraTransform.LookAt(playerTransform.position + Vector3.up * 1.5f);

        //Vector3 forward = cameraTransform.forward;
        // ���X�e�B�b�N���͂ňړ�
        Vector2 moveValue = _moveActionRef.action.ReadValue<Vector2>();
        if (moveValue.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = (playerTransform.forward * moveValue.y + playerTransform.right * moveValue.x);
            playerTransform.position += moveDir * speed * Time.deltaTime;
            //playerTransform.rotation = Quaternion.LookRotation(forward);
        }

        Vector3 playerAngle = playerTransform.eulerAngles;
        playerAngle.x = 0f;
    }

    private bool isGrounded()
    {
        //RaycastHit hit = Physics.CheckSphere(playerTransform.position, groundDistance, groundLayer);
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength, groundLayer);
        //return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Scene�r���[��Ray������
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position + Vector3.down * rayLength);
    }


    public void normalAttack()
    {
        Debug.Log("�ʏ�U���I");
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
