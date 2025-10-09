using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    [SerializeField] private PlayerLifeController player1Life;
    [SerializeField] private PlayerLifeController player2Life;
    [SerializeField] private PlayerLifeController player3Life;
    [SerializeField] private PlayerLifeController player4Life;
    
    [SerializeField] private List<GameObject> teamAMemberList=new List<GameObject>();
    [SerializeField] private List<GameObject> teamBMemberList=new List<GameObject>();
    public bool isDeleteConfirmation = false;//ListÇ©ÇÁçÌèúÇµÇΩÇ©ÇämîFÇ∑ÇÈbool
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teamAMemberList.Add(player1Life.gameObject);
        teamAMemberList.Add(player2Life.gameObject);
        teamBMemberList.Add(player3Life.gameObject);
        teamBMemberList.Add(player4Life.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(teamAMemberList.Count==0 || teamBMemberList.Count==0)
        {
            SceneManager.LoadScene("ResultScene");
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
        //isDeleteConfirmation = false;
    }
}
