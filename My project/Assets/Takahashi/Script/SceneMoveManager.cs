using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    [Header("それぞれのプレイヤーのPlayerController")]
    [SerializeField] private PlayerController playerController1;
    [SerializeField] private PlayerController playerController2;
    [SerializeField] private PlayerController playerController3;
    [SerializeField] private PlayerController playerController4;

    void Update()
    {
        //それぞれのPlayerControllerスクリプトのisReadyがtrueになったらシーン移動
        if (playerController1!=null&&playerController1.isReady
            && playerController2!=null&&playerController2.isReady
            && playerController3!=null&&playerController3.isReady
            && playerController4 != null && playerController4.isReady)
        {
            SceneManager.LoadScene("MainGameScene");
        }
    }
}
