using UnityEngine;

public class TutorialKnifeUI : MonoBehaviour
{
    //ナイフUIプレハブオブジェクト
    [SerializeField] private GameObject knifeIconPrefab;

    //ナイフの外枠UIプレハブオブジェクト
    [SerializeField] private GameObject knifeOuterFrameIconPrefab;
    //ナイフUIを表示させる座標
    [SerializeField] private Transform iconParent;
    [SerializeField] private Transform outerFrameIconParent;
    [SerializeField] private PlayerControllerTutorial playerControllerTutorial;

    //ナイフUIの最小値と最大値
    private int lowestKnifeCount = 0;
    private int MaxestKnifeCount = 5;

    //ナイフの外枠UIの最小値と最大値
    private int lowestOuterFrameKnifeCount = 0;
    private int MaxestOuterFrameKnifeCount = 5;

    //ナイフUI表示非表示の切り替え判定
    bool isActive = false;

    private int currentCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCount = playerControllerTutorial.knifeObjectList.Count;
        for (int i = lowestKnifeCount; i < MaxestKnifeCount; i++)
        {
            Instantiate(knifeIconPrefab, iconParent);
        }

        for (int j = lowestOuterFrameKnifeCount; j < MaxestOuterFrameKnifeCount; j++)
        {
            Instantiate(knifeOuterFrameIconPrefab, outerFrameIconParent);
        }

        UpdateKnifeUI(currentCount);
    }

    // Update is called once per frame
    void Update()
    {
        int newCount = playerControllerTutorial.knifeObjectList.Count;

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
            iconParent.GetChild(i).gameObject.SetActive(isActive);
            //Debug.Log("ナイフUI増加");
        }
    }
}
