using UnityEngine;
using TMPro;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int life = 100;
    [SerializeField] private TextMeshProUGUI lifeCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Knife"))
        {
            life -= 20;
        }
    }
}
