using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    [Header("それぞれのプレイヤーのPlayerControllerTutorial")]
    [SerializeField] private PlayerControllerTutorial playerController1;
    [SerializeField] private PlayerControllerTutorial playerController2;
    [SerializeField] private PlayerControllerTutorial playerController3;
    [SerializeField] private PlayerControllerTutorial playerController4;

    [SerializeField] private TutorialManager tutorialManager = null;
    void Update()
    {
        // チュートリアル中であるかを判定
        if (tutorialManager.isInTutorial)
        {
            //それぞれのPlayerControllerスクリプトのisReadyがtrueになったらシーン移動
            if (playerController1 != null && playerController1.isReady
                && playerController2 != null && playerController2.isReady
                && playerController3 != null && playerController3.isReady
                && playerController4 != null && playerController4.isReady)
            {
                Debug.Log("シーン移動");
                Singleton.instance.TransitionMainGameScene();
            }
        }
    }
}
