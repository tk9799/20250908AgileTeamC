using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;


/// <summary>
/// タイトルシーンから別のシーンへ遷移させるためのクラス
/// </summary>
public class TitleScript : MonoBehaviour
{
    //[Header("MenuPlayerControllerを取得して配列にする")]
    //[SerializeField] public MenuPlayerController[] menuPlayerController;


    //InputActionを自動で読み込むコンポーネントを取得
    [SerializeField] private PlayerInput playerInput;

    //ゲームパッドの取得
    public Gamepad gamepad;

    private InputAction ChoseCharactorScenemove;

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

    private void Start()
    {
        //if (playerInput.devices.Count > 0)
        //{
        //    gamepad = playerInput.devices[0] as Gamepad;
        //    playerNumber = playerInput.playerIndex;
        //}
            
        // プレイヤーの数だけループして、どのコントローラーがどのプレイヤーとして割り当てられているかを確定させる
        //for (int i = 0; i < menuPlayerController.Length; i++)
        //{
            

        //    if (i < Gamepad.all.Count)
        //    {
        //        // i番目のコントローラーより大きいときに処理
        //        // 接続順にpadへ情報を入れる
        //        //menuPlayerController[i].pad = Gamepad.all[i];
        //        //gamepad = playerInput.devices[0] as Gamepad;
        //        //playerNumber = playerInput.playerIndex;
        //    }
        //    else
        //    {
        //        // i番目未満の時はnullを入れる
        //        menuPlayerController[i].pad = null;
        //    }

        //    // プレイヤー番号をセット
        //    //menuPlayerController[i].playerNum = i;
        //    //playerNumber = menuPlayerController[i].playerNum;
        //    //gamepad = menuPlayerController[i].pad;
        //}

        //if (playerInput != null && menuPlayerController.Length > 0)
        //{
        //    gamepad = playerInput.devices[0] as Gamepad;
        //    if (gamepad != null)
        //    {
        //        Debug.Log($"Player {playerNumber + 1} が {gamepad.displayName} を使用中");
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"Player {playerNumber + 1}: デバイスはあるがGamepadではありません。");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning($"Player {playerNumber + 1}: PlayerInputにデバイスが割り当てられていません。");
        //}
    }



    /// <summary>
    /// いずれかのボタンを押してシーン遷移するメソッド
    /// </summary>
    void Update()
    {
        // コントローラーが接続されいるかつABXYのいずれかのボタンが押されたらシーン遷移
        //if (Gamepad.current.leftShoulder.wasPressedThisFrame && Gamepad.current.rightShoulder.wasPressedThisFrame)
        //{
        //    Singleton.instance.TransitionChoseCharactorScene();

        //}

    }
}