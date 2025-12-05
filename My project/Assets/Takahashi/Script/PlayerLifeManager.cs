using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    //4人想定でそれぞれのPlayerLifeControllerとplayerオブジェクトをアタッチする
    [SerializeField] private PlayerLifeController player1Life;
    [SerializeField] private PlayerLifeController player2Life;
    [SerializeField] private PlayerLifeController player3Life;
    [SerializeField] private PlayerLifeController player4Life;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;
    
    //チーム分けする際Listを使ってチーム分けするため2つのListを作成
    [SerializeField] public List<GameObject> teamAMemberList=new List<GameObject>();
    [SerializeField] public List<GameObject> teamBMemberList=new List<GameObject>();

    //Listから削除したかを確認するbool
    public bool isDeleteConfirmation = false;

    //勝敗が決まった時に変更される変数
    private string winnerName = "";

    /// <summary>
    /// tagの名前でプレイヤーのチーム分けしてそれぞれのListに加える
    /// </summary>
    void Start()
    {
        //プレイヤーのtag名でチームを分ける
        if (player1.tag == "RedPlayer")
        {
            teamAMemberList.Add(player1.gameObject);
        }
        else if (player1.tag == "BluePlayer")
        {
            teamBMemberList.Add(player1.gameObject);
        }

        if (player2.tag == "RedPlayer")
        {
            teamAMemberList.Add(player2.gameObject);
        }
        else if (player2.tag == "BluePlayer")
        {
            teamBMemberList.Add(player2.gameObject);
        }

        if (player3.tag == "RedPlayer")
        {
            teamAMemberList.Add(player3.gameObject);
        }
        else if (player3.tag == "BluePlayer")
        {
            teamBMemberList.Add(player3.gameObject);
        }

        if (player4.tag == "RedPlayer")
        {
            teamAMemberList.Add(player4.gameObject);
        }
        else if (player4.tag == "BluePlayer")
        {
            teamBMemberList.Add(player4.gameObject);
        }
    }

    /// <summary>
    /// Listが0になったのを検知して勝ったチーム名を更新する
    /// </summary>
    void Update()
    {
        //どちらかのListの中身が0になった場合
        if (teamAMemberList.Count == 0 || teamBMemberList.Count == 0)
        {
            //Listの中身が0の場合もう片方のチーム名をwinnerNameに代入
            if (teamAMemberList.Count > 0)
            {
                winnerName = "RedPlayer";
            }
            else if (teamBMemberList.Count > 0)
            {
                winnerName = "BulePlayer";
            }

            //勝者の名前を保存するメソッド
            ResultJudgement();
        }
    }

    /// <summary>
    /// プレイヤーがやられた時Listから削除するメソッド
    /// </summary>
    public void GameJudgement()
    {
        //プレイヤーがやられた時Listから削除する
        if (player1Life != null && player1Life.isDed)
        {
            teamAMemberList.Remove(player1Life.gameObject);
            isDeleteConfirmation = true;
        }
        else if(player2Life != null && player2Life.isDed)
        {
            teamAMemberList.Remove(player2Life.gameObject);
            isDeleteConfirmation = true;
        }
        else if(player3Life != null && player3Life.isDed)
        {
            teamBMemberList.Remove(player3Life.gameObject);
            isDeleteConfirmation = true;
        }
        else if(player4Life != null && player4Life.isDed)
        {
            teamBMemberList.Remove(player4Life.gameObject);
            isDeleteConfirmation = true;
        }
    }

    /// <summary>
    /// 勝者の名前を保存するメソッド
    /// </summary>
    public void ResultJudgement()
    {
        //勝者の情報を保存する
        Singleton.instance.winnerTeamName = winnerName;
        Debug.Log(winnerName);

        SceneManager.LoadScene("ResultScene");
    }
}
