using UnityEngine;
using UnityEngine.InputSystem;

public class PrefabPlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody rigidbody;

    private PrefabPlayerMove prefabPlayerMove;

    private PrefabPlayerCameraController prefabPlayerCameraController;

    [SerializeField] private PrefabPlayerJumpScript prefabPlayerJumpScript;

    [SerializeField] private PrefabPlayerNomalAttack prefabPlayernomalAttack;

    [SerializeField] private SceneTransitionManager sceneTransitionManager;

    //[SerializeField] 
    private Vector2 moveInput;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public bool isLetTriggerInput = false;

    void Awake()
    {
        prefabPlayerMove = GetComponent<PrefabPlayerMove>();
        prefabPlayerCameraController = GetComponentInChildren<PrefabPlayerCameraController>();
        prefabPlayerJumpScript = GetComponent<PrefabPlayerJumpScript>();
    }

    // Move入力を受け取る
    /// <summary>
    /// プレイヤーの移動メソッド
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }

        //Debug.Log($"Move Input: {moveInput}");
        prefabPlayerMove.SetMoveInput(moveInput);
    }

    /// <summary>
    /// InputActionのLookの入力を受け取った時に処理するメソッド
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        prefabPlayerCameraController.SetLookInput(lookInput);

    }

    /// <summary>
    /// InputActionのJumpの入力を受け取った時に処理するメソッド
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ジャンプ入力");
            prefabPlayerJumpScript.PlayerJump();
        }
        
    }

    /// <summary>
    /// 通常攻撃メソッド
    /// </summary>
    public void OnNomalAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //エイム中ではない場合
            if (!isLetTriggerInput)
            {
                prefabPlayernomalAttack.NormalAttack();
            }
            //エイム中の場合
            else if (isLetTriggerInput)
            {
                Debug.Log("エイム中のナイフ投げ");
                prefabPlayernomalAttack.AimNormalAttack();
            }
        }
    }

    public void OnSceneChange(InputAction.CallbackContext context)
    {
        Debug.Log("SceneChangeInput");
        if (context.performed)
        {
            Debug.Log("SceneChangeInput");
            sceneTransitionManager.SceneMove();
        }
    }

    /// <summary>
    /// 左トリガー入力でエイム状態を切り替えるメソッド
    /// </summary>
    public void OnAimMode(InputAction.CallbackContext context)
    {
        //左トリガー入力を検知した場合
        if (context.performed)
        {
            float leftTriggerValue = context.ReadValue<float>();

            //トリガー入力が0.3より多いの場合
            if (leftTriggerValue > 0.3f)
            {
                isLetTriggerInput = true;
                Debug.Log("左トリガー入力");
            }
            //トリガー入力が0.3未満の場合
            else
            {
                isLetTriggerInput = false;
            }
        }
        //左トリガーを離した場合
        else if (context.canceled)
        {
            isLetTriggerInput = false;
            Debug.Log("左トリガーを離した");
        }
    }
}
