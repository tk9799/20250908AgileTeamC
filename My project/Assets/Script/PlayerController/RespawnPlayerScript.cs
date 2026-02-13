using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RespawnPlayerScript : MonoBehaviour
{
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RespawnPlayers();
    }


    /// <summary>
    /// 登録されているデバイス情報をもとにプレイヤーをまとめて生成するメソッド
    /// </summary>
    public void RespawnPlayers()
    {
        if (PlayerJoinManager.playerJoinManagerInstance.playerInputManager == null)
        {
            Debug.LogWarning("PlayerInputManager がシーン内に見つかりません");
            return;
        }

        foreach (var player in new List<PlayerInput>(PlayerInput.all))
            Destroy(player.gameObject);

        int index = 0;

        foreach (var device in PlayerJoinManager.playerJoinManagerInstance.joinedDevices)
        {
            var player = PlayerInput.Instantiate(
                prefab: PlayerJoinManager.playerJoinManagerInstance.playerPrefab.gameObject,
                playerIndex: index,
                controlScheme: "Gamepad",
                pairWithDevice: device
            );

            Debug.Log($"Spawn Player{index} : {device}");

            index++;
        }

    }
}
