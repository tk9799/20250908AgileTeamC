using UnityEngine;

/// <summary>
/// チュートリアルシーン全体を管理するクラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
    // チュートリアル中かどうかの判定
    [SerializeField] public bool isInTutorial = false;

    /// <summary>
    /// 初期設定メソッド
    /// </summary>
    private void Start()
    {
        // チュートリアルシーンでは最初からチュートリアル中に設定
        isInTutorial = true;
    }
}
