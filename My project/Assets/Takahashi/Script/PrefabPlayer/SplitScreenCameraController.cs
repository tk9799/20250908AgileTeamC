using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenCameraController : MonoBehaviour
{
    private Camera camera;
    private PlayerInput playerInput;

    private void Start()
    {
        camera = GetComponent<Camera>();

        playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is not assigned!");
            return;
        }

        ApplyViewport(playerInput.playerIndex);

        //Debug.Log($"Camera Found = {camera != null}");
        //Debug.Log($"PlayerInput Found = {playerInput != null}");
        Debug.Log($"PlayerIndex = {playerInput?.playerIndex}");
    }

    private void ApplyViewport(int index)
    {
        switch (index)
        {
            case 0:
                camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
                break;
            case 1:
                camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;
            case 2:
                camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
                break;
            case 3:
                camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
                break;
            default:
                camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                break;
        }
        //Debug.Log($"Player {playerInput.playerIndex} camera rect = {camera.rect}");
        //Debug.Log($"Camera {gameObject.name} Å® PlayerIndex = {index}");
    }
}
