using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー入室処理
/// </summary>
public class PlayerEnteringRoom : MonoBehaviour
{
    private PlayerInputManager playerInputManager = null;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        //プレイヤー入室時のコールバックを設定
        playerInputManager.onPlayerJoined += OnPlayerJoined;

        //プレイヤー退出時のコールバックを設定
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }

    /// <summary>
    /// プレイヤーが入室したときの処理
    /// </summary>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        
    }

    /// <summary>
    /// プレイヤーが退室したときの処理
    /// </summary>
    private void OnPlayerLeft(PlayerInput playerInput)
    {

    }
}
