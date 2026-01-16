using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    public string winnerTeamName = "";

    // シーン遷移用の変数
    // ここではタイトルシーンを入力する
    public string titleScene = "";

    // シーン遷移用の変数
    // キャラクター選択シーンを入力する
    public string choseCharactorScene = "";

    // シーン遷移用の変数
    // チュートリアルシーンを入力する
    public string tutorialScene = "";

    // シーン遷移用の変数
    // メインゲームシーンを入力する
    public string mainGameScene = "";


    // シーン遷移用の変数
    // リザルトシーンを入力する
    public string resultScene = "";

    //プレイヤー番号
    //public int playerNum = 0;
    //public MenuPlayerController menuPlayerController = null;

    [Header("MenuPlayerControllerを取得して配列にする")]
    [SerializeField] private MenuPlayerController[] menuPlayerController;

    public void Connection()
    {
        //// プレイヤーの数だけループして、どのコントローラーがどのプレイヤーとして割り当てられているかを確定させる
        //for (int i = 0; i < menuPlayerController.Length; i++)
        //{

        //    if (i < Gamepad.all.Count)
        //    {
        //        // i番目のコントローラーより大きいときに処理
        //        Debug.Log("jvidfskoa");
        //        // 接続順にpadへ情報を入れる
        //        menuPlayerController[i].pad = Gamepad.all[i];
        //    }
        //    else
        //    {
        //        // i番目未満の時はnullを入れる
        //        menuPlayerController[i].pad = null;
        //    }

        //    // プレイヤー番号をセット
        //    menuPlayerController[i].playerNum = i;
        //}
    }

    /// <summary>
    /// タイトルシーンへ遷移するメソッド
    /// </summary>
    public void TransitionTitleScene()
    {
        SceneManager.LoadScene(titleScene);
    }

    /// <summary>
    /// キャラクター選択シーンへ遷移するメソッド
    /// </summary>
    public void TransitionChoseCharactorScene()
    {
        SceneManager.LoadScene(choseCharactorScene);
    }

    /// <summary>
    /// チュートリアルシーンへ遷移するメソッド
    /// </summary>
    public void TransitionTutorialScene()
    {
        SceneManager.LoadScene(tutorialScene);
    }

    /// <summary>
    /// メインゲームシーンへ遷移するメソッド
    /// </summary>
    public void TransitionMainGameScene()
    {
        SceneManager.LoadScene(mainGameScene);
    }

    /// <summary>
    /// リザルトシーンへ遷移するメソッド
    /// </summary>
    public void TransitionResultScene()
    {
        SceneManager.LoadScene(resultScene);
    }

    public void StringWinnerName(string winnerName)
    {
        winnerTeamName = winnerName;
    }

    //public int GeyPlayerNum()
    //{
    //    menuPlayerController = GetComponent<MenuPlayerController>();
    //    return menuPlayerController.playerNum;
    //}

    //public void PlayerNum(int playerNum)
    //{
    //    playerNum = menuPlayerController.playerNum;
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
