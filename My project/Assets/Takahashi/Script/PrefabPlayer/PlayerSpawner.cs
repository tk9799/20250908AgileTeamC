using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerJoinManager.playerJoinManagerInstance != null)
        {
            PlayerJoinManager.playerJoinManagerInstance.RespawnPlayers();
        }
        //PlayerJoinManager.Instantiate.RespawnPlayers();
    }

}
