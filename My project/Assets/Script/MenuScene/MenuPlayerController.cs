using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// メニュー画面においてのプレイヤーの操作を管理するスクリプト
/// </summary>
public class MenuPlayerController : MonoBehaviour
{
    [Header("プレイヤーの番号を指定する変数")]
    [SerializeField] public int playerNum = 0;

    [Header("接続されているコントローラー")]
    public Gamepad pad = null;

    [Header("スティックでの入力情報を受け取る")]
    private Vector3 input = Vector3.zero;

    [Header("キャラクターの配列")]
    [SerializeField] public GameObject[] charactors = null;

    [Header("キャラクターの詳細のUI")]
    [SerializeField] private TextMeshProUGUI[] charactorsState = null;

    [Header("キャラクター詳細のページ表記")]
    [SerializeField] private TextMeshProUGUI charactorPage = null;

    [Header("メニューマネージャーを取得するための変数")]
    [SerializeField] private MenuManager menuManager = null;

    [Header("キャラクター選択が決定したかどうかの判定")]
    [SerializeField] private bool isDecided = false;

    [Header("キャラクター詳細表示の判定")]
    [SerializeField] private bool isStateDisplay = false;

    [Header("キャラクター選択のインデックス")]
    [SerializeField] private int currentIndex = 0;

    [Header("Yを押した回数")]
    [SerializeField] private int yButtonPressCount = 0;

    [Header("入力のクールタイム変数")]
    [SerializeField] private float inputCooldown = 0.0f;

    [Header("最後に入力を受け付けた時間")]
    [SerializeField] private float lastInputTime = 0f;

    [Header("TitleSceneへシーン遷移")]
    [SerializeField] private string titleScene = "";

    [SerializeField] private TextMeshProUGUI readyText = null;

    /// <summary>
    /// キャラ表示とキャラ詳細説明文の表示管理メソッド
    /// </summary>
    void Start()
    {
        // 最初のキャラクターだけ表示
        UpdateCharactorDisplay();

        // キャラ詳細の二つ目の文を非表示にする
        charactorsState[1].gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラ切り替えと説明文切り替えのメソッド
    /// </summary>
    void Update()
    {
        // コントローラーがつながってないときは通さない
        if (pad == null || charactors.Length == 0) return;

        // 左スティック受け取り
        input = new Vector2(Gamepad.all[playerNum].leftStick.ReadValue().x, Gamepad.all[playerNum].leftStick.ReadValue().y);

        // 上下操作の変数
        float vertical = Gamepad.all[playerNum].leftStick.ReadValue().y;

        // 左右操作の変数
        float horizontal = Gamepad.all[playerNum].leftStick.ReadValue().x;

        // 左右の入力でキャラクター切り替え
        // 最後に入力された時間がクールタイムを上回ったら処理開始
        if (inputCooldown < Time.time - lastInputTime)
        {
            if (0 < input.x)
            {
                //右入力でインデックスを増やして、配列の範囲を超えたら0に戻す
                currentIndex = (currentIndex + 1) % charactors.Length;

                // 配列を切り替えてキャラ変更
                UpdateCharactorDisplay();

                // 入力した時間を変数に入れる
                lastInputTime = Time.time;
            }
            else if (input.x < 0)
            {
                // 左入力でインデックスを減らして、配列の範囲を超えたら最後のインデックスに戻す
                currentIndex = (currentIndex - 1 + charactors.Length) % charactors.Length;

                // 配列を切り替えてキャラ変更
                UpdateCharactorDisplay();

                // 入力した時間を変数に入れる
                lastInputTime = Time.time;
            }
        }

        if (this.pad.buttonSouth.wasPressedThisFrame && !isDecided)
        {
            // isDecidedがfalseのとき、Aボタンを押したら決定処理

            Debug.Log("Player " + (playerNum + 1) + " selected character " + charactors[currentIndex].name);

            // キャラクター決定処理
            menuManager.decisionCount++;

            

            // 決定した判定
            isDecided = true;
        }
        else if (this.pad.buttonEast.wasPressedThisFrame && isDecided)
        {
            // isDecidedがtrueのとき、Bボタンを押したらキャンセル処理

            Debug.Log("Player " + (playerNum + 1) + " canceled character selection.");

            // キャラクター選択キャンセル処理
            menuManager.decisionCount--;

            // 非表示
            readyText.gameObject.SetActive(false);

            

            // 誰も決定ボタンを押していないとき、タイトルシーンへ戻る
            if (this.pad.buttonEast.wasPressedThisFrame && menuManager.decisionCount <= -1)
            {
                // タイトルシーンへ遷移
                Singleton.instance.TransitionTitleScene();
            }
        }

        if (this.pad.buttonNorth.wasPressedThisFrame)
        {
            // ゲームパッドのYボタンを押すと処理開始

            // Yボタンを押した回数を1増やす
            yButtonPressCount++;
        }


        //if(isDecided)
        //{
        //    // 準備できたら表示
        //    readyText.gameObject.SetActive(true);
        //}
        //else
        //{
        //    // 決定をキャンセルした判定
        //    isDecided = false;
        //}


        switch (yButtonPressCount)
        {
            // Yボタンを押した回数に応じて処理が開始される

            case 0:
                break;

            case 1:
                // 1回押されたとき、キャラクター詳細表示

                // 最初に表示されていたテキストを非表示にする
                charactorsState[yButtonPressCount].gameObject.SetActive(false);

                // 次に表示されるものを表示させる
                charactorsState[yButtonPressCount - 1].gameObject.SetActive(true);
                break;

            case 2:
                // 2回目に押されたとき、キャラクター詳細表示

                // 表示されていたテキストを非表示にする
                charactorsState[yButtonPressCount - 2].gameObject.SetActive(false);

                // 最初に表示されていたテキストを再表示する
                charactorsState[yButtonPressCount - 1].gameObject.SetActive(true);
                break;

            case 3:
                // 3回目に押されたとき、処理開始

                // Yボタンの押された回数を1にする
                yButtonPressCount = 1;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// キャラ表示の更新
    /// </summary>
    private void UpdateCharactorDisplay()
    {
        // キャラ表示の切り替え
        for (int i = 0; i < charactors.Length; i++)
        {
            charactors[i].SetActive(i == currentIndex);
        }
    }

    /// <summary>
    /// キャラ詳細テキストの更新
    /// </summary>
    private void UpdateCharactorStateDisplay()
    {
        // キャラ詳細テキストの切り替え
        for (int i = 0; i < charactorsState.Length; i++)
        {
            charactorsState[i].gameObject.SetActive(i == currentIndex);
        }
    }
}




