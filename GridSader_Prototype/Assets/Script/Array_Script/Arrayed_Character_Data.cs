using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Arrayed_Character_Data : MonoBehaviour
{
    public GameObject[] Pass_Data;
    public ArrData_Sync arrdata_sync;
    public static Arrayed_Character_Data instance;

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initial_Ready()//Ready��ư�� ��������.
    {
        arrdata_sync.DataSync(Pass_Data);
    }

}
