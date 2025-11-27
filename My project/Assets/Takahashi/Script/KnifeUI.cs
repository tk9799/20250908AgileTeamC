using UnityEngine;
using UnityEngine.UI;

public class KnifeUI : MonoBehaviour
{
    [SerializeField] private GameObject knifeIconPrefab;
    [SerializeField] private Transform iconParent;
    [SerializeField] private PlayerController playerController;
    private int lowestKnifeCount = 0;
    private int MaxestKnifeCount = 5;

    //ナイフUI表示非表示の切り替え判定
    bool isActive = false;

    private int currentCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCount = playerController.knifeObjectList.Count;
        for(int i=lowestKnifeCount; i<MaxestKnifeCount; i++)
        {
            Instantiate(knifeIconPrefab, iconParent);
        }

        UpdateKnifeUI(currentCount);
    }

    // Update is called once per frame
    void Update()
    {
        int newCount = playerController.knifeObjectList.Count;

        //プレイヤーのknifeObjectListと現在のcurrentCountを比較して変化があればUIを更新
        if (newCount != currentCount)
        {
            UpdateKnifeUI(newCount);
            currentCount = newCount;
        }
    }

    private void UpdateKnifeUI(int count)
    {
        for (int i = lowestKnifeCount; i < iconParent.childCount; i++)
        {
            //iがcount未満なら表示、同じまたは以上なら非表示
            if (i < count)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
            //bool isActive = (i < count);
            iconParent.GetChild(i).gameObject.SetActive(isActive);
            //Debug.Log("ナイフUI増加");
        }
    }
}
