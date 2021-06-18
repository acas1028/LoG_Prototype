using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Arrayed_Character_Data : MonoBehaviour
{
    private GameObject[] Array_Team;
    public GameObject[] Pass_Data;
    public ArrData_Sync arrdata_sync;

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Arrayment_Manager.Array_instance.Ready_Array == true)
        {
            Array_Team = GameObject.FindGameObjectsWithTag("Character");
            Arrayment_Manager.Array_instance.Ready_Array = false;
            initial_Ready();
        }
    }

    public void initial_Ready()//Ready버튼을 눌렀을시.
    {
        for (int i = 0; i < Pass_Data.Length; i++)//공격 순서 초기화
        {
            Pass_Data[i].GetComponent<Character_Script>().Copy_Character_Stat(Array_Team[i]);
            if(Pass_Data[i].GetComponent<Character_Script>().character_Attack_Order>5)
            {
                Pass_Data[i].GetComponent<Character_Script>().character_Attack_Order = Pass_Data[i].GetComponent<Character_Script>().character_Attack_Order % 5;
                if (Pass_Data[i].GetComponent<Character_Script>().character_Attack_Order % 5 == 0)
                {
                    Pass_Data[i].GetComponent<Character_Script>().character_Attack_Order = 5;
                }
                Pass_Data[i].GetComponent<Character_Script>().Debuging_Character();

            }
            Pass_Data[i].GetComponent<Character_Script>().Debuging_Character();
        }
        arrdata_sync.DataSync(Pass_Data);
    }
}
