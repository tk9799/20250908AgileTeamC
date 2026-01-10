using System.Collections.Generic;
using UnityEngine;

public class KnifeInventory : MonoBehaviour
{
    //プレイヤーが所持しているナイフを格納するリスト
    [SerializeField] public List<GameObject> knifeObjectList = new List<GameObject>();
    //開始時ナイフを設定した数生成する際に使うbool
    private bool isInitialGenerate = false;

    //ナイフの最大所持数
    [SerializeField] private int maxCount = 5;

    //ナイフを生成するために参照するGameObject
    [SerializeField] public GameObject knifeObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateInitialKnives();
    }

    private void GenerateInitialKnives()
    {
        for (int i = 0; i < maxCount; i++)
        {
            knifeObjectList.Add(knifeObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasKnife()
    {
        return knifeObjectList.Count > 0;
    }

    public void UseKnife()
    {
        if (knifeObjectList.Count > 0)
        {
            knifeObjectList.RemoveAt(0);
        }
    }

    public void AddKnife()
    {
        if (knifeObjectList.Count < maxCount)
        {
            knifeObjectList.Add(knifeObject);
        }
    }

    /// <summary>
    /// ナイフを回収・所持数を増やす
    /// </summary>
    public void CheckPickup()
    {
        //当たり判定はtagを使ってhitしたらrayを飛ばして当たり判定を使う
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f); // 半径2.0の範囲を調べる
        foreach (var hit in hits)
        {
            //ナイフの所持者が誰のものでもなく所持数が上限未満の場合ナイフを追加する
            if (hit.CompareTag("NotPossessionKnife") && knifeObjectList.Count < maxCount)
            {
                Debug.Log("ナイフを回収");
                //knifeObjectList.Add(knifeObject.gameObject);
                AddKnife();
                Destroy(hit.gameObject);
                // 所持数を増やす処理もここで行う
            }
        }
    }
}
