using UnityEngine;
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
