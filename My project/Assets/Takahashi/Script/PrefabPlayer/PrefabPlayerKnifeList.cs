using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PrefabPlayerKnifeList : MonoBehaviour
{
    [SerializeField] private GameObject knifeObject;

    public List<GameObject>knifePossessionList= new List<GameObject>();

    //ナイフを初期生成する数値
    [Header("ナイフ初期生成数")]
    [SerializeField] private int initialKnifeNumber = 0;

    //ナイフを所持できる最大値
    [Header("ナイフ所持最大数")]
    [SerializeField] private int maxKnifePossessionNumber = 0;

    //ナイフを回収できる範囲
    [Header("ナイフ回収範囲")]
    [SerializeField] private float collectionRangeNumber=0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        KnifeInitialAddList();   
    }

    // Update is called once per frame
    private void Update()
    {
        KnifeCollection();
    }

    /// <summary>
    /// ナイフの初期生成
    /// </summary>
    private void KnifeInitialAddList()
    {
        for(int i =0;i<initialKnifeNumber; i++)
        {
            knifePossessionList.Add(knifeObject.gameObject);
        }
    }

    /// <summary>
    /// ナイフを回収してListに登録するメソッド
    /// </summary>
    private void KnifeCollection()
    {
        Collider[] collectionRange = Physics.OverlapSphere(transform.position, collectionRangeNumber);

        foreach(var knifeHit in collectionRange)
        {
            if(knifeHit.CompareTag("NotPossessionKnife")&&
                knifePossessionList.Count < maxKnifePossessionNumber)
            {
                knifePossessionList.Add(knifeObject.gameObject);
                Debug.Log("ナイフ回収");
                Destroy(knifeHit.gameObject);
            }
        }
    }
}
