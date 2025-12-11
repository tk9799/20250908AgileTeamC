using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] public bool isInTutorial = false;

    private void Awake()
    {
        isInTutorial = true;
    }
}
