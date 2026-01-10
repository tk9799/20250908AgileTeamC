using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //プレイヤーのTransform
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform spawnPoint;

    //ナイフを投げる時の速度
    [SerializeField] private float translateSpeed = 1f;

    [SerializeField] private float spawnDistance = 2f;

    //ナイフを生成するために参照するGameObject
    [SerializeField] public GameObject knifeObject;

    private KnifeInventory knifeInventory;

    private void Awake()
    {
        knifeInventory = GetComponent<KnifeInventory>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalAttack()
    {
        if (!knifeInventory.HasKnife()) return;

        //プレイヤー前方の座標を取得
        Vector3 translatePos = playerTransform.position + playerTransform.forward * spawnDistance;

        GameObject knife = Instantiate(knifeObject, translatePos, spawnPoint.rotation);

        Rigidbody rb = knife.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(playerTransform.forward * translateSpeed, ForceMode.Impulse);
        }

        knifeInventory.UseKnife();
    }

    protected virtual void WeakSkill()
    {
        Debug.Log("弱スキル");
    }

    protected virtual void StrongSkill()
    {
        Debug.Log("強スキル");
    }
}
