using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// メニュー画面においてのプレイヤーの操作を管理するスクリプト
/// ・キャラクター切替
/// ・ステータスUIのページ切替
/// ・決定 / キャンセル処理
/// </summary>
public class MenuPlayerController : MonoBehaviour
{
    [Header("プレイヤー番号")]
    [SerializeField] public int playerNum = 0;

    //[Header("接続されているコントローラー")]
    public Gamepad pad = null;

    [Header("キャラクター決定テキスト")]
    [SerializeField] private GameObject characterDecidedText = null;

    [Header("キャラクター配列（表示用モデルなど）")]
    [SerializeField] public GameObject[] charactors = null;

    [Header("キャラクター詳細UI (0=ステータス / 1=スキル)")]
    [SerializeField] private TextMeshProUGUI[] charactorsState = null;

    [Header("ページ表記 例：1/2")]
    [SerializeField] private TextMeshProUGUI charactorPage = null;

    [Header("メニューマネージャー")]
    [SerializeField] private MenuManager menuManager = null;

    [Header("決定状態（キャラを確定したか）")]
    [SerializeField] private bool isDecided = false;

    [Header("ステータスUIを表示中か")]
    [SerializeField] private bool isStateDisplay = true;

    [Header("現在選択中のキャラIndex")]
    [SerializeField] private int currentIndex = 0;

    [Header("現在ページ (0=ステータス / 1=スキル)")]
    [SerializeField] private int currentPage = 0;

    [Header("入力クールタイム（連続入力防止）")]
    [SerializeField] private float inputCooldown = 0.25f;

    [Header("最後に入力した時間")]
    [SerializeField] private float lastInputTime = 0f;

    [Header("右入力のしきい値")]
    [SerializeField] private float leftStickInputThreshold = 0.5f;

    [Header("左入力のしきい値")]
    [SerializeField] private float leftStickInputThresholdNegative = -0.5f;

    [SerializeField] private PlayerInput playerInput;

    // InputSystemアクション
    private InputAction decisionAction;
    private InputAction canselAction;
    private InputAction displaySwitchingAction;

    /// <summary>
    /// InputActionの登録
    /// </summary>
    private void OnEnable()
    {
        decisionAction = playerInput.actions["DecisionButton"];
        canselAction = playerInput.actions["CancelButton"];
        displaySwitchingAction = playerInput.actions["DisplaySwitching"];

        // --- Action を確実に有効化 ---
        decisionAction.Enable();
        canselAction.Enable();
        displaySwitchingAction.Enable();

        decisionAction.performed += OnDecision;
        canselAction.performed += OnCansel;
        displaySwitchingAction.performed += OnDisplaySwitching;
    }

    private void OnDisable()
    {
        decisionAction.performed -= OnDecision;
        canselAction.performed -= OnCansel;
        displaySwitchingAction.performed -= OnDisplaySwitching;

        decisionAction.Disable();
        canselAction.Disable();
        displaySwitchingAction.Disable();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    //private void Start()
    //{
    //    // コントローラー取得
    //    pad = playerInput.devices[0] as Gamepad;
    //    playerNum = playerInput.playerIndex;

    //    // 「決定済み」テキストを非表示
    //    if (characterDecidedText != null)
    //        characterDecidedText.SetActive(false);

    //    // 初期表示更新
    //    UpdateCharactorDisplay();
    //    UpdateCharactorStateDisplay();
    //}

    private void Start()
    {
        playerNum = playerInput.playerIndex;

        Debug.Log($"==== Player {playerNum} ====");

        // --- PlayerJoinManager から割り当て ---
        if (PlayerJoinManager.playerJoinManagerInstance != null &&
            PlayerJoinManager.playerJoinManagerInstance.joinedDevices.Count > playerNum)
        {
            pad = PlayerJoinManager.playerJoinManagerInstance.joinedDevices[playerNum] as Gamepad;
        }

        if (pad == null)
        {
            Debug.LogError($"Player{playerNum} にGamepadが割り当てられていません");
            return;
        }

        Debug.Log($"Player{playerNum} pad = {pad}");

        if (characterDecidedText != null)
            characterDecidedText.SetActive(false);

        UpdateCharactorDisplay();
        UpdateCharactorStateDisplay();
    }






    /// <summary>
    /// 毎フレーム更新
    /// ・左右入力でキャラ切替
    /// </summary>
    private void Update()
    {
        // --- Gamepad未接続 ---
        if (pad == null) return;

        // --- キャラ未設定 ---
        if (charactors == null || charactors.Length == 0) return;

        // --- 決定済み ---
        if (isDecided) return;

        Vector2 stick = pad.leftStick.ReadValue();

        // 入力クールタイム中は処理しない
        if (Time.time - lastInputTime < inputCooldown) return;

        // 右入力
        if (stick.x >= leftStickInputThreshold)
        {
            currentIndex = (currentIndex + 1) % charactors.Length;
            currentPage = 0;

            UpdateCharactorDisplay();
            UpdateCharactorStateDisplay();

            lastInputTime = Time.time;
        }
        // 左入力
        else if (stick.x <= leftStickInputThresholdNegative)
        {
            currentIndex = (currentIndex - 1 + charactors.Length) % charactors.Length;
            currentPage = 0;

            UpdateCharactorDisplay();
            UpdateCharactorStateDisplay();

            lastInputTime = Time.time;
        }
    }


    /// <summary>
    /// 決定ボタン（A）
    /// キャラクター確定
    /// </summary>
    private void OnDecision(InputAction.CallbackContext ctx)
    {
        if (isDecided) return;

        menuManager.decisionCount++;

        if (characterDecidedText != null)
            characterDecidedText.SetActive(true);

        isDecided = true;

        Debug.Log($"Player {playerNum + 1} がキャラクターを決定しました。");
    }

    /// <summary>
    /// キャンセルボタン（B）
    /// キャラ確定を解除
    /// </summary>
    private void OnCansel(InputAction.CallbackContext ctx)
    {
        menuManager.decisionCount--;

        if (characterDecidedText != null)
            characterDecidedText.SetActive(false);

        isDecided = false;

        // 全員キャンセルでタイトルへ戻る
        if (menuManager.decisionCount <= -1)
        {
            Singleton.instance.TransitionTitleScene();
        }
    }

    /// <summary>
    /// Yボタン
    /// ステータスページ切替
    /// </summary>
    private void OnDisplaySwitching(InputAction.CallbackContext ctx)
    {
        if (!isStateDisplay) return;

        currentPage = (currentPage + 1) % charactorsState.Length;
        UpdateCharactorStateDisplay();
    }

    /// <summary>
    /// キャラクター表示切替
    /// 選択中のキャラだけ表示
    /// </summary>
    private void UpdateCharactorDisplay()
    {
        for (int i = 0; i < charactors.Length; i++)
        {
            charactors[i].SetActive(i == currentIndex);
        }
    }

    /// <summary>
    /// ステータスUIページ切替
    /// currentPage のみ表示
    /// </summary>
    private void UpdateCharactorStateDisplay()
    {
        for (int i = 0; i < charactorsState.Length; i++)
        {
            charactorsState[i].gameObject.SetActive(i == currentPage);
        }

        // ページ表記更新
        if (charactorPage != null)
        {
            charactorPage.text = $"{currentPage + 1}/{charactorsState.Length}";
        }
    }
}
