using UnityEngine;

public class KnifeControllertr : MonoBehaviour
{
    public int damage = 10;
    public GameObject owner; // 誰が撃ったかを記録

    private void Start()
    {
        //Destroy(gameObject, lifeTime); // 一定時間後に弾を消す
    }

    private void OnTriggerEnter(Collider other)
    {
        // 自分を撃ったプレイヤーに当たったら無視する場合はここでチェック
        if (other.gameObject == owner)
        {
            return;
        }

        PlayerLifeController health = other.GetComponent<PlayerLifeController>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject); // 弾を消す
        }
    }
}
