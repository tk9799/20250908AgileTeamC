using UnityEngine;

public class TutorialPlayerLifeController : MonoBehaviour
{
    //プレイヤーの最大体力のデフォルト値
    //プレイヤーの体力が減る処理があるため０にしていない
    [SerializeField] private int maxLife = 100;

    //プレイヤーのプレイ中のの体力
    public int playerLife = 0;

    //プレイヤーがやられた時の判定
    public bool isDed = false;

    //プレイヤーがやられた後脱落させる判定
    private bool isDropout = false;

    //PlayerLifeManagerスクリプトを取得
    [SerializeField] private PlayerLifeManager playerLifeManager = null;

    /// <summary>
    /// 処理開始時プレイヤーの体力に最大値の体力を代入
    /// </summary>
    private void Start()
    {
        //プレイヤーの体力が最大になる
        playerLife = maxLife;
    }

    /// <summary>
    /// プレイヤーの体力を減らす処理
    /// </summary>
    public void TakeDamage(int damage)
    {
        //TakeDamagemeメソッドが呼ばれた時代入した引数分プレイヤーの体力を減らす
        playerLife -= damage;

        Debug.Log(gameObject.name + " が " + damage + " ダメージを受けた！ 残りHP: " + playerLife);

        //プレイヤーの体力が０（初期値）以下の時
        if (playerLife <= 0)
        {
            //Die();
        }
    }

    /// <summary>
    /// プレイヤーが倒された時の処理
    /// </summary>
    private void Die()
    {
        Debug.Log(gameObject.name + " は倒された！");

        //プレイヤーがやられた判定にするためtrueにする
        isDed = true;

        //GameJudgement()を呼び出すことでプレイヤーがやられた時Listから削除できる
        playerLifeManager.GameJudgement();

        //playerLifeManagerのteamAMemberList、teamBMemberListから削除された場合
        if (playerLifeManager != null && playerLifeManager.isDeleteConfirmation)
        {
            //やられたプレイヤーのtagがRedPlayerの場合
            if (gameObject.tag == "RedPlayer")
            {
                //teamAMemberListから削除
                playerLifeManager.teamAMemberList.Remove(gameObject);
                Debug.Log("teamAListから削除");
                //脱落判定にする
                isDropout = true;
            }
            //やられたプレイヤーのtagがBluePlayerの場合
            else if (gameObject.tag == "BluePlayer")
            {
                //teamBMemberListから削除
                playerLifeManager.teamBMemberList.Remove(gameObject);
                Debug.Log("teamBListから削除");
                //脱落判定にする
                isDropout = true;
            }
            //gameObject.SetActive(false);
        }

        //プレイヤーが脱落判定になった時非表示にする
        if (isDropout)
        {
            gameObject.SetActive(false);
        }
    }
}
