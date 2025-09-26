using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MenuPlayerController : MonoBehaviour
{


    [SerializeField] public int playerNum = 0;
    public Gamepad pad = null;
    private Vector3 input = Vector3.zero;
    [SerializeField] float speed = 0.0f;
    [SerializeField] public GameObject[] charactors;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private bool isDecided = false;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private float inputCooldown = 0.2f;
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
                currentIndex = (currentIndex + 1) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.time;
            }
            else if (input.x < 0)
            {
                currentIndex = (currentIndex - 1 + charactors.Length) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.time;
            }
        }

        if (this.pad.buttonSouth.wasPressedThisFrame && !isDecided)
        {
            Debug.Log("Player " + (playerNum + 1) + " selected character " + charactors[currentIndex].name);
            // キャラクター決定処理
            menuManager.decisionCount++;
            isDecided = true;
        }
        else if (this.pad.buttonEast.wasPressedThisFrame && isDecided)
        {
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
}




