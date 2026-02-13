//using UnityEngine;
//using UnityEngine.InputSystem;

//public class ConnectionManager : MonoBehaviour
//{
//    [SerializeField] private MenuPlayerController[] menuPlayers;

//    private void Start()
//    {
//        AssignGamepads();
//    }

//    private void Update()
//    {
//        // ”²‚«·‚µ‘Î‰‚µ‚½‚¢ê‡
//        if (Gamepad.all.Count != GetAssignedCount())
//        {
//            AssignGamepads();
//        }
//    }

//    private void AssignGamepads()
//    {
//        var pads = Gamepad.all;

//        for (int i = 0; i < menuPlayers.Length; i++)
//        {
//            if (i < pads.Count)
//            {
//                menuPlayers[i].pad = pads[i];
//                menuPlayers[i].playerNum = i;

//                Debug.Log($"Player {i + 1} assigned to {pads[i].displayName}");
//            }
//            else
//            {
//                menuPlayers[i].pad = null;
//                menuPlayers[i].playerNum = -1;
//            }
//        }
//    }

//    private int GetAssignedCount()
//    {
//        int count = 0;
//        foreach (var p in menuPlayers)
//        {
//            if (p.pad != null) count++;
//        }
//        return count;
//    }
//}
