using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Gamepad gamepad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        gamepad = playerInput.devices[0] as Gamepad;

        if (playerInput.playerIndex >= 0||playerInput.playerIndex <= 1)
        {
            Debug.Log("このプレイヤーは赤チームです");
            gameObject.tag = "RedPlayer";
        }
        else if (playerInput.playerIndex >= 2 || playerInput.playerIndex <= 3)
        {
            Debug.Log("このプレイヤーは青チームです");
            gameObject.tag = "BluePlayer";
        }

        Debug.Log($"Player {playerInput.playerIndex + 1} ready " +
            $"({gamepad.displayName})"
        );
    }

    // Update is called once per frame
    private void Update()
    {
        if (gamepad == null) return;

        Vector2 move = gamepad.leftStick.ReadValue();

        //if (move.x > 0.5f)
        //{
        //    Debug.Log($"P{playerInput.playerIndex + 1} Right");
        //}
        //else if (move.x < -0.5f)
        //{
        //    Debug.Log($"P{playerInput.playerIndex + 1} Left");
        //}

        //if (gamepad.buttonSouth.wasPressedThisFrame)
        //{
        //    Debug.Log($"P{playerInput.playerIndex + 1} A pressed");
        //}
    }
}
