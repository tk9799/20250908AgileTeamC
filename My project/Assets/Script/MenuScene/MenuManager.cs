using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// コントローラーの接続管理をするクラス
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("MenuPlayerControllerを取得して配列にする")]
    [SerializeField] private MenuPlayerController[] menuPlayerController;

    [Header("決定、キャンセルで増減する変数")]
    [SerializeField] public int decisionCount = 0;

    /// <summary>
    /// コントローラーの接続をして、それぞれに番号を与えるメソッド
    /// </summary>
    void Start()
    {
        // プレイヤーの数だけループして、どのコントローラーがどのプレイヤーとして割り当てられているかを確定させる
        for (int i = 0; i < menuPlayerController.Length; i++)
        {
            if (i < Gamepad.all.Count)
            {
                // i番目のコントローラーより大きいときに処理

                // 接続順にpadへ情報を入れる
                menuPlayerController[i].pad = Gamepad.all[i];
            }
            else
            {
                // i番目未満の時はnullを入れる
                menuPlayerController[i].pad = null;
            }

            // プレイヤー番号をセット
            menuPlayerController[i].playerNum = i;
        }
    }

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
