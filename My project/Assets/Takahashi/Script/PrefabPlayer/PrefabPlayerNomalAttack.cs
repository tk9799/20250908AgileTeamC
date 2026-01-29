using UnityEngine;

public class PrefabPlayerNomalAttack : MonoBehaviour
{
    //ナイフを生成するときのプレイヤーの距離
    [SerializeField] private float knifeSpawnDistance = 0.0f;

    [SerializeField] private float tlanslateSpoeed = 0.0f;

    [SerializeField] private PrefabPlayerKnifeList prefabPlayerKnifeList = null;

    [SerializeField] private GameObject knifeObject = null;
 
    /// <summary>
    /// 通常攻撃処理
    /// ナイフを所持しているListにナイフがある場合のみナイフを飛ばせる
    /// </summary>
    public void NormalAttack()
    {
        //プレイヤー正面＋生成する位置の座標を取得
        Vector3 knifeTranslatePosition = transform.position + transform.forward * knifeSpawnDistance;

        //ナイフを所持しているリストにナイフがある場合ナイフを飛ばす
        if (prefabPlayerKnifeList != null && prefabPlayerKnifeList.knifePossessionList.Count > 0)
        {
            GameObject generateKnife = Instantiate(knifeObject, knifeTranslatePosition, transform.rotation);

            Rigidbody rigidbody = generateKnife.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(transform.forward * tlanslateSpoeed, ForceMode.Impulse);
            }

            //ナイフを投げたらListから削除
            prefabPlayerKnifeList.knifePossessionList.RemoveAt(0);
        }
        else
        {
            Debug.LogError("ナイフを所持していません");
        }
    }
}
