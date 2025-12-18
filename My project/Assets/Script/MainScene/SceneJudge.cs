using UnityEngine;

/// <summary>
/// チュートリアルマネージャーの状態を判定するクラス
/// </summary>
public class SceneJudge : MonoBehaviour
{
    [Header("チュートリアルマネージャーの取得gs")]
    [SerializeField] private TutorialManager tutorialManager = null;

    private void Update()
    {
        // チュートリアルが終了したらisInTutorialをfalseに設定
        if (tutorialManager.isInTutorial)
        {
            tutorialManager.isInTutorial = false;
        }
    }
}
