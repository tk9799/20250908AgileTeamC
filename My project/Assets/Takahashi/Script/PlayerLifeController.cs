using UnityEngine;
using TMPro;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int maxLife = 100;
    private int playerLife = 0;
    public bool isDed = false;
    [SerializeField] private PlayerLifeManager playerLifeManager;
    //[SerializeField] private TextMeshProUGUI lifeCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerLife -= damage;
        Debug.Log(gameObject.name + " が " + damage + " ダメージを受けた！ 残りHP: " + playerLife);

        if (playerLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " は倒された！");
        isDed = true;
        playerLifeManager.GameJudgement();
        if (playerLifeManager != null && playerLifeManager.isDeleteConfirmation)
        {
            this.gameObject.SetActive(false);
        }
        
        // Destroy(gameObject);  // プレイヤーを消す場合
    }
}
