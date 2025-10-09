using UnityEngine;

public class KnifeControllertr : MonoBehaviour
{
    public int damage = 10;
    public GameObject owner; // 誰が撃ったかを記録
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
            if(this.gameObject.tag== "Knife")
            {
                //Debug.Log(this.gameObject.tag);
                damage = 10;
            }
            Destroy(gameObject); // 弾を消す
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            
            if (rb != null)
            {
                rb.isKinematic = true; // 物理挙動を止める
                //transform.position = Vector3.zero;
                //rb.linearVelocity = Vector3.zero;
                //rb.angularVelocity = Vector3.zero;
                Debug.Log(rb.isKinematic);
            }
            this.gameObject.tag = "NotPossessionKnife";//誰のものでもないナイフにする
            transform.parent = other.transform;//壁にくっつける

            
        }

        if (other.gameObject.CompareTag("Player")&&this.gameObject.tag== "NotPossessionKnife")
        {
            Debug.Log("ナイフを回収");
        }
    }
}
