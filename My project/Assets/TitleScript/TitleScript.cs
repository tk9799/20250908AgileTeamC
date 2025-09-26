using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    private Gamepad pad = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame || Gamepad.current.buttonNorth.wasPressedThisFrame ||
            Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }
}
