using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuPlayerController[] menuPlayerController;
    [SerializeField] private GameObject playerObject = null;
    [SerializeField] public int decisionCount = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < menuPlayerController.Length; i++)
        {
            if (Gamepad.all.Count > i)
            {
                
                menuPlayerController[i].pad = Gamepad.all[i];
            }
            else
            {
                menuPlayerController[i].pad = null;
            }


            // キャラクター生成
            //var player = Instantiate(playerObject, new Vector3(0, 0, 0), Quaternion.identity);



            //// null参照をはじく
            //if (controller != null && controller.charactors != null && i < controller.charactors.Length)
            //{
            //    playerObject = controller.charactors[i];
            //}

            
            menuPlayerController[i].playerNum = i;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (decisionCount == 4)
        {
            // 4人決定したらゲームシーンへ遷移
            SceneManager.LoadScene("");

            Debug.Log("4人決定！");
        }
        else if (decisionCount > 4)
        {
            decisionCount = 4;
        }
    }
}
