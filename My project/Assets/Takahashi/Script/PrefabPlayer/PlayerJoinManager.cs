using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// プレイヤーに紐づけるデバイス情報を取得するクラス
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    //プレイヤーがゲームにJoinするためのInputAction
    [SerializeField] private InputAction playerJoinInputAction = null;

    //
    [SerializeField] private PlayerInputManager playerInputManager = null;

    //PlayerInputがアタッチされているプレイヤーオブジェクト
    [SerializeField] private PlayerInput playerPrefab = null;

    //最大参加人数
    [SerializeField] private int maxPlayerCount = 0;

    //Join済みのデバイス情報
    //private InputDevice[] joinedDevices = default;
    private List<InputDevice> joinedDevices = new List<InputDevice>();

    //現在のプレイヤー数
    private int currentPlayerCount = 0;

    private void Awake()
    {
        //最大参加可能数で配列を初期化
        //joinedDevices=new InputDevice[maxPlayerCount];

        //InputActionを有効化
        playerJoinInputAction.Enable();

        //InputAction入力時のコールバックを設定
        playerJoinInputAction.performed += OnJoin;
    }

    private void OnEnable()
    {
        playerJoinInputAction.Enable();
        //playerJoinInputAction
    }

    private void OnDisable()
    {
        playerJoinInputAction.performed -= OnJoin;
        playerJoinInputAction.Disable();
    }

    private void OnJoin(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;

        //プレイヤー数が最大数に達していたら処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            return;
        }

        //Join要求元のデバイスが既に参加済みの時、処理を終了
        foreach(var device in joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        //PlayerInputを保持した仮想のプレイヤーをインスタンス化
        //Join要求元のデバイス情報を紐づけてインスタンスを生成する
        PlayerInput.Instantiate(prefab: playerPrefab.gameObject, playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device);

        //Joinしたデバイス情報を保存
        joinedDevices[currentPlayerCount] = context.control.device;

        currentPlayerCount++;

        //InputActionを入力したデバイス情報を取得
        //InputDevice inputDevice = context.control.device;
    }
}
