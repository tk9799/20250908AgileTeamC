using UnityEngine;
using UnityEngine.InputSystem;


public class MenuPlayerController : MonoBehaviour
{


    public int playerNum = 0;
    public Gamepad pad = null;
    private Vector3 input = Vector3.zero;
    [SerializeField] float speed = 0.0f;
    [SerializeField] public GameObject[] charactors;
    [SerializeField] private MenuManager menuManager;

    private int currentIndex = 0;
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

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
        input = new Vector2(Gamepad.all[0].leftStick.ReadValue().x, Gamepad.all[0].leftStick.ReadValue().y);

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (input.x > 0)
            {
                currentIndex = (currentIndex + 1) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.deltaTime;
            }
            else if (input.x < 0)
            {
                currentIndex = (currentIndex - 1 + charactors.Length) % charactors.Length;
                UpdateCharactorDisplay();
                lastInputTime = Time.deltaTime;
            }
        }
    }

    private void UpdateCharactorDisplay()
    {
        for (int i = 0; i < charactors.Length; i++)
        {
            charactors[i].SetActive(i == currentIndex);
        }
    }
}

    


