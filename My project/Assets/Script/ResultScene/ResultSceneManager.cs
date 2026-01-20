using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// リザルトシーンでの処理を管理するクラス
/// </summary>
public class ResultSceneManager : MonoBehaviour
{
    private void Update()
    {

        // Aボタンを押したらタイトルシーンに遷移
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Singleton.instance.TransitionTitleScene();
        }
    }

}
