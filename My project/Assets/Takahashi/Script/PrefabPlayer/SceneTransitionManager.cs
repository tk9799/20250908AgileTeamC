using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    //private void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == "ExperimentScene")
    //    {
    //        SceneMove();
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    public void SceneMove()
    {
        if (SceneManager.GetActiveScene().name == "ExperimentScene")
        {
            Debug.Log("ƒV[ƒ“ˆÚ“®");
            SceneManager.LoadScene("ExperimentScene2");
        }
        else
        {
            return;
        }
        
    }
}
