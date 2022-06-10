using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVESceneLoad : MonoBehaviour
{
    private void Start()
    {
        Arrayed_Data arr = Arrayed_Data.instance;

        if(arr)
        {
            for(int i=0;i<5;i++)
            {
                arr.team1[i].GetComponent<Character>().Character_Reset();
            }
        }
    }
}
