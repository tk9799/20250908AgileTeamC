using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動・攻撃・準備完了・カメラの管理をするクラス
/// </summary>
public class PlayerManagement : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private KnifeInventory knifeInventory;

    [SerializeField] private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["NomalAttack"];

        //ジャンプ入力がされるとJumpメソッドを呼び出す
        jumpAction.performed += _ => playerMovement.Jump();

        //攻撃入力がされるとNormalAttackメソッドを呼び出す
        attackAction.performed += _ => playerAttack.NormalAttack();
    }

    private void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector2 look = lookAction.ReadValue<Vector2>();

        playerMovement.SetMoveInput(move);
        playerCameraController.SetLookInput(look);

        knifeInventory.CheckPickup();
    }
}
