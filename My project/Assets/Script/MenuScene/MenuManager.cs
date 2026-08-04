using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// コントローラーの接続管理をするクラス
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("MenuPlayerControllerを取得して配列にする")]
    [SerializeField] private MenuPlayerController[] menuPlayerController = null;

    [Header("決定、キャンセルで増減する変数")]
    [SerializeField] public int decisionCount = 0;

    /// <summary>
    /// 全員決定後にシーン遷移をするメソッド
    /// </summary>
    void Update()
    {
        if (decisionCount == menuPlayerController.Length)
        {
            // 4人決定したらゲームシーンへ遷移
            Singleton.instance.TransitionTutorialScene();

            Debug.Log("4人決定！");
        }
        else if (menuPlayerController.Length < decisionCount)
        {
            // 4人を超えたら4人に戻す
            decisionCount = menuPlayerController.Length;
        }
    }
}
