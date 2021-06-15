using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArrData_Sync : MonoBehaviourPunCallbacks
{
    public GameObject[] team1_characterPrefab;
    public GameObject[] team2_characterPrefab;
    private ArrData_Sync instance;

    // Start is called before the first frame update
    public void DataSync(GameObject[] array_team)
    {
        team1_characterPrefab = new GameObject[5];
        team1_characterPrefab = array_team;

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Photon Instantiate " + i);
        }
    }
}
