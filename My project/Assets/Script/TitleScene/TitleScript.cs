using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;


/// <summary>
/// タイトルシーンから別のシーンへ遷移させるためのクラス
/// </summary>
public class TitleScript : MonoBehaviour
{
    [Header("ゲームパッドの接続")]
    private Gamepad pad = null;

    /// <summary>
    /// いずれかのボタンを押してシーン遷移するメソッド
    /// </summary>
    void Update()
    {
        // コントローラーが接続されいるかつABXYのいずれかのボタンが押されたらシーン遷移
        if (pad != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Singleton.instance.TransitionTutorialScene();
        }
        else
        {
            //// キーボードのいずれかのキーが押されたらシーン遷移
            //if (Input.anyKey)
            //{
            //    Singleton.instance.TransitionTutorialScene();
            //}
        }
    }
}
