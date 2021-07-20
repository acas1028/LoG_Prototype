using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Arrayed_Character_Data : MonoBehaviour
{
    public GameObject[] Pass_Data;
    public ArrData_Sync arrdata_sync;
    public ArrRoomManager roomManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initial_Ready()//Ready버튼을 눌렀을시.
    {
        arrdata_sync.DataSync(Pass_Data);
        roomManager.StartArrayPhase();
    }
}
