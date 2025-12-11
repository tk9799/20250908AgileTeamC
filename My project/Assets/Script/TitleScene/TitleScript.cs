using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;


/// <summary>
/// タイトルシーンから別のシーンへ遷移させるためのクラス
/// </summary>
public class TitleScript : MonoBehaviour
{
    /// <summary>
    /// いずれかのボタンを押してシーン遷移するメソッド
    /// </summary>
    void Update()
    {
        // コントローラーが接続されいるかつABXYのいずれかのボタンが押されたらシーン遷移
        if (Gamepad.current.leftShoulder.wasPressedThisFrame && Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            Singleton.instance.TransitionChoseCharactorScene();
        }
    }
}
