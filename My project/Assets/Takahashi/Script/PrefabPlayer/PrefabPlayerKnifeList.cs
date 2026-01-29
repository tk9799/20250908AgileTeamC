using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PrefabPlayerKnifeList : MonoBehaviour
{
    //ナイフオブジェクト
   [SerializeField] private GameObject knifeObject;

    public List<GameObject> knifePossessionList= new List<GameObject>();

    //ナイフを初期生成する数
    [SerializeField] private int initialKnifeNumber = 0;

    //ナイフの最大所持数
    [SerializeField] private int maxKnifePossessionNumber = 0;

    //ナイフを回収する範囲
    [SerializeField] private float collectionRangeNumber = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //ナイフを初期生成する数だけListに登録するメソッド
        KnifeInitialAddList();
    }

    // Update is called once per frame
    private void Update()
    {
        KnifeCollection();
    }

    private void KnifeInitialAddList()
    {
        for(int i = 0; i < initialKnifeNumber; i++)
        {
            knifePossessionList.Add(knifeObject.gameObject);
        }
    }

    /// <summary>
    /// 一定以内の範囲にある誰のものでもないナイフを回収するメソッド
    /// </summary>
    private void KnifeCollection()
    {
        Collider[] collectionRange = Physics.OverlapSphere(transform.position, collectionRangeNumber);

        foreach(var knifeHit in collectionRange)
        {
            if (knifeHit.CompareTag("NotPossessionKnife") && 
                knifePossessionList.Count < maxKnifePossessionNumber)
            {
                knifePossessionList.Add(knifeObject.gameObject);
                Debug.Log("ナイフ回収");
                Destroy(knifeHit.gameObject);
            }
        }
    }
}
