using UnityEngine;
using UnityEngine.UI;

public class TutorialUIHPGaugeController : MonoBehaviour
{
    [SerializeField] public TutorialPlayerLifeController tutorialPlayerLifeController;
    [SerializeField] Image image;
    private float HP = 0.0f;

    void Update()
    {
        HP = tutorialPlayerLifeController.playerLife;
        image.fillAmount = HP / 100f;
    }
}
