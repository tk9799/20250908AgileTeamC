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
            Debug.Log("hit");
            //このオブジェクトのtagがRedKnifeで当たったオブジェクトのtagがBluePlayer(敵)の場合
            if (this.gameObject.tag == "RedKnife" && other.gameObject.CompareTag("BluePlayer"))
            {
                health.TakeDamage(damage);
                damage = 10;//10ダメージを与える
                Destroy(gameObject); // 弾を消す
                Debug.Log("敵に命中");
            }
            //このオブジェクトのtagがBlueknifeで当たったオブジェクトのtagがRedPlayer(敵)の場合
            if (this.gameObject.tag == "Blueknife" && other.gameObject.CompareTag("RedPlayer"))
            {
                health.TakeDamage(damage);
                damage = 10;
                Destroy(gameObject); // 弾を消す
                Debug.Log("敵に命中");
            }
            Destroy(gameObject); // 弾を消す
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            
            if (rb != null)
            {
                rb.isKinematic = true; // 物理挙動を止める
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
