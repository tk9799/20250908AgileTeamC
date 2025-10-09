using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// メニュー画面においてのプレイヤーの操作を管理するスクリプト
public class MenuPlayerController : MonoBehaviour
{

    // プレイヤーの番号を指定する変数
    [SerializeField] public int playerNum = 0;

    // 接続されているコントローラー
    public Gamepad pad = null;

    // スティックでの入力情報を受け取る
    private Vector3 input = Vector3.zero;

    // キャラクターの配列    
    [SerializeField] public GameObject[] charactors;

    // キャラクター詳細のUI
    [SerializeField] private TextMeshProUGUI[] charactorsState = null;

    // メニューマネージャーを取得するための変数
    [SerializeField] private MenuManager menuManager;

    // キャラクター選択が決定したかどうかの判定
    [SerializeField] private bool isDecided = false;

    // キャラクター詳細表示の判定
    [SerializeField] private bool isStateDisplay = false;

    // キャラクター選択のインデックス
    [SerializeField] private int currentIndex = 0;

    // Yを押した回数
    [SerializeField] private int yButtonPressCount = 0;

    // 入力のクールタイム変数
    [SerializeField] private float inputCooldown = 0.0f;

    // 最後に入力を受け付けた時間
    [SerializeField] private float lastInputTime = 0f;

    void Start()
    {
        // 最初のキャラクターだけ表示
        UpdateCharactorDisplay();
    }

    void Update()
    {
        // コントローラーがつながってないときは通さない
        if (pad == null || charactors.Length == 0) return;

        // 左スティック受け取り
        input = new Vector2(Gamepad.all[playerNum].leftStick.ReadValue().x, Gamepad.all[playerNum].leftStick.ReadValue().y);

        // 左右の入力でキャラクター切り替え
        if (Time.time - lastInputTime > inputCooldown)
        {
            if (input.x > 0)
            {
                // インデックスを増やして、配列の範囲を超えたら0に戻す
                currentIndex = (currentIndex + 1) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.time;
            }
            else if (input.x < 0)
            {
                // インデックスを減らして、配列の範囲を超えたら最後のインデックスに戻す
                currentIndex = (currentIndex - 1 + charactors.Length) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.time;
            }
        }

        if (this.pad.buttonSouth.wasPressedThisFrame && !isDecided)
        {
            // isDecidedがfalseのとき、Aボタンを押したら決定処理
            Debug.Log("Player " + (playerNum + 1) + " selected character " + charactors[currentIndex].name);
            // キャラクター決定処理
            menuManager.decisionCount++;
            isDecided = true;
        }
        else if (this.pad.buttonEast.wasPressedThisFrame && isDecided)
        {
            // isDecidedがtrueのとき、Bボタンを押したらキャンセル処理
            Debug.Log("Player " + (playerNum + 1) + " canceled character selection.");
            // キャラクター選択キャンセル処理
            menuManager.decisionCount--;
            isDecided = false;

            // 誰も決定ボタンを押していないとき、タイトルシーンへ戻る
            if (this.pad.buttonEast.wasPressedThisFrame && menuManager.decisionCount <= -1)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        if (this.pad.buttonNorth.wasPressedThisFrame)
        {
            yButtonPressCount++;
        }

        switch (yButtonPressCount)
        {
            case 1:
                // 1回押されたとき、キャラクター詳細表示
                charactorsState[yButtonPressCount - 1].gameObject.SetActive(true);
                charactorsState[yButtonPressCount].gameObject.SetActive(false);
                break;

            case 2:
                charactorsState[yButtonPressCount - 1].gameObject.SetActive(true);
                charactorsState[yButtonPressCount - 2].gameObject.SetActive(false);
                break;

            case 3:
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
        for (int i = 0; i < charactors.Length; i++)
        {
            charactors[i].SetActive(i == currentIndex);
        }
    }


    private void UpdateCharactorStateDisplay()
    {
        for (int i = 0; i < charactorsState.Length; i++)
        {
            charactorsState[i].gameObject.SetActive(i == currentIndex);
        }
    }
}




