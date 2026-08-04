using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;


/// <summary>
/// タイトルシーンから別のシーンへ遷移させるためのクラス
/// </summary>
public class TitleScript : MonoBehaviour
{
    //InputActionを自動で読み込むコンポーネントを取得
    [SerializeField] private PlayerInput playerInput = null;

    private InputAction ChoseCharactorScenemove= null;

    //複数人でやる際のプレイヤーの番号割り当てに使う変数
    public int playerNumber = 0;

    private void OnEnable()
    {
        ChoseCharactorScenemove = playerInput.actions["ChoseCharactorSceneMove"];

        ChoseCharactorScenemove.performed += OnChoseCharactorSceneMove;
    }

    private void OnChoseCharactorSceneMove(InputAction.CallbackContext callbackContext)
    {
        Singleton.instance.TransitionChoseCharactorScene();
    }

    /// <summary>
    /// いずれかのボタンを押してシーン遷移するメソッド
    /// </summary>
    void Update()
    {
        // コントローラーが接続されいるかつABXYのいずれかのボタンが押されたらシーン遷移
        if (Gamepad.current.leftShoulder.IsPressed() && Gamepad.current.rightShoulder.IsPressed())
        {
            Singleton.instance.TransitionChoseCharactorScene();

        }

    }
}