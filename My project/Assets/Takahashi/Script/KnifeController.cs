using UnityEngine;


public class KnifeControllertr : MonoBehaviour
{
    //定数のため書き換えることができない
    //tagの宣言
    public static readonly string BLUE_PLAYER = "BluePlayer";
    public static readonly string BLUE_KNIFE = "Blueknife";
    public static readonly string RED_PLAYER = "RedPlayer";
    public static readonly string RED_KNIFE = "RedKnife";
    public static readonly string NOT_POSSESSION_KNIFE = "NotPossessionKnife";
    public static readonly string WALL = "Wall";

    //プレイヤーが受けるダメージの値
    public int damage = 0;

    //通常攻撃のダメージの値
    private int normalAttackDamage = 10;

    //Rigidbodyを取得
    //物理挙動を変更するために使う
    private Rigidbody rigidbody = null;

    private PlayerLifeController playerLifeController = null;

    private enum KnifeState
    {
        // 飛んでいるときのナイフの状態
        FLYINGKNIFE,

        // 所持している時(待機状態のナイフ)の状態
        STAYKNIFE
    }

    // ナイフの状態を初期化
    private static KnifeState knifeState = KnifeState.STAYKNIFE;


    /// <summary>
    /// 初期化・宣言
    /// </summary>
    private void Start() 
    {
        rigidbody = GetComponent<Rigidbody>();
        playerLifeController = GetComponent<PlayerLifeController>();
    }

    private void Update()
    {
        if(transform.position.x > 0 || transform.position.z > 0)
        {
            knifeState = KnifeState.FLYINGKNIFE;
        }
        else
        {
            knifeState = KnifeState.STAYKNIFE;
        }
    }



    /// <summary>
    /// ナイフを生成した後の敵プレイヤーのダメージを与える、壁に当たった場合くっつくようにさせる＋その状態だと
    /// 回収して自分のものにできる処理（予定）
    /// </summary>
    //private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision other)
    {
        //PlayerLifeControllerを取得して敵にダメージを与える
        PlayerLifeController health = other.gameObject.GetComponent<PlayerLifeController>();

        //PlayerLifeControllerがnullでない場合
        if (health != null)
        {
            //このオブジェクトのtagがRedKnifeで当たったオブジェクトのtagがBluePlayer(敵)の場合
            if (gameObject.tag == RED_KNIFE && other.gameObject.CompareTag(BLUE_PLAYER))
            {
                //敵に与えるダメージの値（通常攻撃）
                damage = normalAttackDamage;

                //TakeDamageメソッドを呼び出して当たったプレイヤーの体力を減らす
                health.TakeDamage(damage);

                //プレイヤーに当たった状態ナイフは使わないため非表示
                gameObject.SetActive(false); 
            }

            //このオブジェクトのtagがBlueknifeで当たったオブジェクトのtagがRedPlayer(敵)の場合
            if (this.gameObject.tag == BLUE_KNIFE && other.gameObject.CompareTag(RED_PLAYER))
            {
                //敵に与えるダメージの値（通常攻撃）
                damage = normalAttackDamage;

                //TakeDamageメソッドを呼び出して当たったプレイヤーの体力を減らす
                health.TakeDamage(damage);

                //プレイヤーに当たった状態ナイフは使わないため非表示
                Destroy(gameObject);
            }
        }

        //当たったオブジェクトのtagが"Wall"だった場合
        if (other.gameObject.CompareTag(WALL))
        {
            //Rigidbodyを取得している場合
            if (rigidbody != null)
            {
                // 物理挙動を止める
                //ナイフを落下させないようにする
                rigidbody.isKinematic = true; 
            }

            //プレイヤーのナイフではない状態にするためtagの変更
            this.gameObject.tag = NOT_POSSESSION_KNIFE;

            //くっついた壁の子オブジェクトにする
            transform.parent = other.transform;//壁にくっつける
        }

        //誰のものでもないナイフ（gameObject.tag== "NotPossessionKnife"）だった場合、ナイフオブジェクトに
        //触れたプレイヤーの物になる（予定）
        if (other.gameObject.CompareTag(RED_PLAYER) && other.gameObject.CompareTag(BLUE_PLAYER) &&
            gameObject.tag== NOT_POSSESSION_KNIFE)
        {
            Debug.Log("ナイフを回収");
        }
    }
}
