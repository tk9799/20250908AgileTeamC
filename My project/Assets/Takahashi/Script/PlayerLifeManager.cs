using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    [SerializeField] private PlayerLifeController player1Life;
    [SerializeField] private PlayerLifeController player2Life;
    [SerializeField] private PlayerLifeController player3Life;
    [SerializeField] private PlayerLifeController player4Life;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;
    
    [SerializeField] public List<GameObject> teamAMemberList=new List<GameObject>();
    [SerializeField] public List<GameObject> teamBMemberList=new List<GameObject>();
    public bool isDeleteConfirmation = false;//ListÇ©ÇÁçÌèúÇµÇΩÇ©ÇämîFÇ∑ÇÈbool
    private string winnerName = "";

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (teamAMemberList.Count == 0 || teamBMemberList.Count == 0)
        {
            if (teamAMemberList.Count > 0)
            {
                winnerName = "RedPlayer";
            }
            else if (teamBMemberList.Count > 0)
            {
                winnerName = "BulePlayer";
            }

            ////DontDestroyOnLoad(gameObject);
            //SceneManager.LoadScene("ResultScene");
            ResultJudgement();
        }
    }

    public void GameJudgement()
    {
        if(player1Life != null && player1Life.isDed)
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

        //if (teamAMemberList.Count == 0 || teamBMemberList.Count == 0)
        //{
        //    if (teamAMemberList.Count > 0)
        //    {
        //        winnerName = "RedPlayer";
        //        Debug.Log(winnerName);
        //    }
        //    else if (teamBMemberList.Count > 0)
        //    {
        //        winnerName = "BulePlayer";
        //        Debug.Log(winnerName);
        //    }

        //    //DontDestroyOnLoad(gameObject);
        //    //SceneManager.LoadScene("ResultScene");
        //}
        //isDeleteConfirmation = false;
    }

    public void ResultJudgement()
    {
        //èüé“ÇÃèÓïÒÇï€ë∂Ç∑ÇÈ
        Singleton.instance.winnerTeamName = winnerName;
        Debug.Log(winnerName);

        SceneManager.LoadScene("ResultScene");
    }
}
