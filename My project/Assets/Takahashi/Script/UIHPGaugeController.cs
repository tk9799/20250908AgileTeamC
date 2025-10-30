using UnityEngine;
using UnityEngine.UI;

public class UIHPGaugeController : MonoBehaviour
{
    [SerializeField] public PlayerLifeController playerLifeController;
    [SerializeField] Image image;
    private float HP = 0.0f;

    void Update()
    {
        HP = playerLifeController.playerLife;
        image.fillAmount = HP / 100f;
    }
}
