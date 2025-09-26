using UnityEngine;
using TMPro;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int maxLife = 100;
    private int playerLife = 0;
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Knife"))
    //    {
    //        Debug.Log("hit");
    //        life -= 20;
    //    }
    //    if(life <= 0)
    //    {
    //        this.gameObject.SetActive(false);
    //    }
    //}

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
        this.gameObject.SetActive(false);
        // Destroy(gameObject);  // プレイヤーを消す場合
    }
}
