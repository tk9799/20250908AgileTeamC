using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    public string winnerTeamName = "";

    public string GetWinnerName()
    {
        return winnerTeamName;
    }

    public void StringWinnerName(string winnerName)
    {
        winnerTeamName = winnerName;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
