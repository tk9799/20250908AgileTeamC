using UnityEngine;
using TMPro;

public class ResultText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resultTect.text = "èüé“"+ Singleton.instance.winnerTeamName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
