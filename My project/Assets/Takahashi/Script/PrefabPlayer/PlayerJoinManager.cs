using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーに紐づけるデバイス情報を取得するクラス
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    //自身のスクリプトを格納し、どこからでもアクセスできるようにする
    public static PlayerJoinManager playerJoinManagerInstance = null;

    //プレイヤーがゲームにJoinするためのInputAction
    [SerializeField] private InputAction playerJoinInputAction = null;

    [Header("PlayerInputManagerがコンポーネントにあるオブジェクト")]
    [SerializeField] private PlayerInputManager playerInputManager = null;

    //PlayerInputがアタッチされているプレイヤーオブジェクト
    [Header("PlayerInputがアタッチされているプレイヤープレハブ")]
    [SerializeField] private PlayerInput playerPrefab = null;

    //最大参加人数
    //[SerializeField] private int maxPlayerCount = 0;

    //Join済みのデバイス情報
    //private InputDevice[] joinedDevices = default;
    [SerializeField] private List<InputDevice> joinedDevices = new List<InputDevice>();

    //現在のプレイヤー数
    private int currentPlayerCount = 0;

    private void Awake()
    {
        //最大参加可能数で配列を初期化
        //joinedDevices=new InputDevice[maxPlayerCount];

        if (playerJoinManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }



        playerJoinManagerInstance = this;

        //Sceneが切り替わってもデバイス情報を保持するために破棄しない
        DontDestroyOnLoad(gameObject);

        //InputActionを有効化
        playerJoinInputAction.Enable();
        Debug.Log($"Player spawned: {gameObject.name}");

        //Scene移動後に RespawnPlayers()が呼ばれるようになる
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        playerJoinInputAction.Enable();
        playerJoinInputAction.performed += OnJoin;
    }

    private void OnDisable()
    {
        playerJoinInputAction.performed -= OnJoin;
        playerJoinInputAction.Disable();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Scene遷移後nullになっているPlayerInputManagerを再取得し、
    /// 登録されているデバイス情報をもとにプレイヤーを生成するメソッド
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerInputManager = FindAnyObjectByType<PlayerInputManager>();
        RespawnPlayers();
    }

    /// <summary>
    /// InputActionで設定した入力をすると追加で参加できるメソッド
    /// インスペクターのplayerJoinInputActionの項目に設定してある
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        Debug.Log("参加");
        InputDevice inputDevice = context.control.device;

        // Gamepad以外は参加させない
        if (!(inputDevice is Gamepad))
            return;

        //プレイヤー数が最大数に達していたら処理を終了
        if (playerInputManager.playerCount >= playerInputManager.maxPlayerCount)
        {
            return;
        }

        //Join要求元のデバイスが既に参加済みの時、処理を終了
        foreach (var device in joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        //List（デバイス情報）に登録
        joinedDevices.Add(inputDevice);

        //登録したプレイヤーを生成するために呼び出す
        RespawnPlayers();

    }

    /// <summary>
    /// 登録されているデバイス情報をもとにプレイヤーをまとめて生成するメソッド
    /// </summary>
    public void RespawnPlayers()
    {
        if (playerInputManager == null)
        {
            Debug.LogWarning("PlayerInputManager がシーン内に見つかりません");
            return;
        }

        foreach (var player in new List<PlayerInput>(PlayerInput.all))
            Destroy(player.gameObject);

        int index = 0;

        foreach (var device in joinedDevices)
        {
            var player = PlayerInput.Instantiate(
                prefab: playerPrefab.gameObject,
                playerIndex: index,
                controlScheme: "Gamepad",
                pairWithDevice: device
            );

            Debug.Log($"Spawn Player{index} : {device}");

            index++;
        }

    }
}
