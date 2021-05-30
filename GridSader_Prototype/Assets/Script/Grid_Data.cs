using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Data : MonoBehaviour
{
    private GameObject[] Grids;

    void Start()
    {
        Grids = GameObject.FindGameObjectsWithTag("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init_Grids_Num()
    {
        for (int i = 0; i < Grids.Length; i++)
        {

        }

    }
}
