using UnityEngine;

/// <summary>
/// プレハブプレイヤーを一定数生成する処理
/// </summary>
public class PrefabPlayerInstance : MonoBehaviour
{
    //プレハブプレイヤー
    [SerializeField] GameObject prefabPlayer = null;

    //生成する数
    [SerializeField] private int InstanceNum = 0;

    //生成する座標
    private Transform InstanceTransform = null;

    //更に生成したときに離れる距離
    private float offsetPosition = 0.0f;

    
    private void Update()
    {
        //
        for(int i=0; i<InstanceNum; i++)
        {
            //プレイヤーの生成
            Instantiate(prefabPlayer,InstanceTransform);
            InstanceTransform.position = new Vector3(offsetPosition, 0.0f, 0.0f);

            //生成する座標を変更
            //生成する座標はプレイヤーごとに固定するため仮の状態で作っている
            offsetPosition += 1.0f;
        }
    }
}
