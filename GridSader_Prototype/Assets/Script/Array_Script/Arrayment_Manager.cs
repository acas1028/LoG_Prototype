using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Manager: MonoBehaviour
{
   
    private bool Character_instance = false;
    public bool Ready_Array = false;
    private bool my_turn = true;
    private bool Pick = true;
    private bool On_Raycast = false;
    private int Phase = (int)ArrayPhase.STANDBY;
    private int Time_Out_Inventory;
    private int cancle_num;

    GameObject Character_Instantiate;
    [Tooltip("프리펩된 캐릭터")]
    public GameObject Prefeb_Character;
    public List<GameObject> Order = new List<GameObject>();

    public GameObject Array_Time;
    public GameObject[] Grids;
    public GameObject[] Enemy_Grids;
    private GameObject[] Inventory;
    public GameObject[] Block_Inventory;
    public GameObject PopUp_UI;
    public GameObject Array_Cancle_Button;
    public GameObject PopUp_Manager;
    private GameObject Cancle_Character;
    public ArrRoomManager arrRoomManager;
    public ArrData_Sync arrData_Sync;

    private static Arrayment_Manager ArrayManager;
    public static Arrayment_Manager Array_instance
    {
        get
        {
            if (!ArrayManager)
            {
                ArrayManager = FindObjectOfType(typeof(Arrayment_Manager)) as Arrayment_Manager;

                if (ArrayManager == null)
                    Debug.Log("no Singleton obj");
            }
            return ArrayManager;
        }
    }
    private void Awake()
    {
        if (ArrayManager == null)
        {
            ArrayManager = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (!ArrayManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Inventory = GameObject.FindGameObjectsWithTag("Character_inventory_Button");
        Phase = arrRoomManager.GetArrayPhase();
    }
    void Update()
    {
        Arrayment_Raycast();
        Array_Order();
    }
    public void Arrayment_Raycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
            if (!hit)
                return;

            if (hit.transform.CompareTag("Character")&&Character_instance==false)
            {
                //PopUp_Manager.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(hit.collider.gameObject.GetComponent<Character>().character_ID);
                Cancle_Character = hit.transform.gameObject;
                if(my_turn==true)
                {
                    Array_Cancle_Button.SetActive(true);
                }
                //PopUp_UI.SetActive(true);                   
            }
            else
            {
                Array_Cancle_Button.SetActive(false);
            }
            if (hit.transform.CompareTag("Null_Character")&&Character_instance == true)//Null_Character로 태그 되어 있는 물체에게 raycast가 닿으면.
            {
                Character_Instantiate = hit.collider.gameObject;
                Character_Instantiate.tag = "Character";//Character로 태그를 변경한다. 예외처리.
                Character_Instantiate.GetComponent<Character>().character_ID = Prefeb_Character.GetComponent<Character>().character_ID;
                Character_Instantiate.GetComponent<Character>().Character_Setting(Character_Instantiate.GetComponent<Character>().character_ID);
                for (int i = 0; i < Grids.Length; i++)//캐릭터에 Grid_Number 삽입
                {
                    if (Character_Instantiate == Grids[i])
                    {
                        Character_Instantiate.GetComponent<Character>().character_Num_Of_Grid = i + 1;
                    }
                }
                for (int i = 0; i < Inventory.Length; i++)//인벤토리 개별화
                {
                if (Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Character_Instantiate.GetComponent<Character>().character_ID) //ID가 변동 되므로 수정 해야함.
                {

                    Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = true;
                }

                }
                Character_Instantiate.GetComponent<Character>().Debuging_Character();
                Order.Add(Character_Instantiate);
                StartCoroutine(Sync_Character());
                Debug.Log(arrData_Sync.test);
                //PopUp_UI.SetActive(false);
                Character_instance = false;
            }
        }
    }
    public void Cancle_Array()
    {
        
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Cancle_Character.GetComponent<Character>().character_ID)
            {
                Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = false;
            }
        }
        for (int i = 0; i < Order.Count; i++)
        {
            if (Cancle_Character == Order[i])
            {
                Order.Remove(Order[i]);
                Cancle_Sync(i);
                Debug.Log(arrData_Sync.test);
            }
        }
        Cancle_Character.tag = "Null_Character";
        Cancle_Character.GetComponent<Character>().Character_Reset();
        Cancle_Character.GetComponent<SpriteRenderer>().sprite = null;
        Cancle_Character.GetComponent<Character>().Debuging_Character();
    }
    public void Array_Order()
    {
        Phase = arrRoomManager.GetArrayPhase();
        int Enemy_Grid_Num;

        if (arrRoomManager.IsPlayerPreemptive())
        {
            switch (Phase)
            {
                case (int)ArrayPhase.FIRST1:
                    my_turn = true;
                    Pick = true;
                    if(Array_Time.GetComponent<Time_FlowScript>().Time_Over==true)
                    {
                        StartCoroutine(Time_Out());
                        Ready_Array = true;
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                    }
                    if (Order[0].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;

                        if (Ready_Array == true)
                        {
                            Order[0].GetComponent<Character>().character_Attack_Order = 1;
                            Arrayed_Data.instance.team1[0].GetComponent<Character>().Copy_Character_Stat(Order[0]);
                            Arrayed_Data.instance.team1[0].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;

                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (Arrayed_Data.instance.team2[0].GetComponent<Character>().character_Num_Of_Grid != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[0].GetComponent<Character>().character_ID);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            arrData_Sync.is_datasync = false;
                        }
                        else
                        {
                            Debug.Log(arrData_Sync.test);
                        }
                        if (Arrayed_Data.instance.team2[1].GetComponent<Character>().character_Num_Of_Grid != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[1].GetComponent<Character>().character_ID);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            arrData_Sync.is_datasync = false;
                        }
                        else
                        {
                            Debug.Log(arrData_Sync.test);
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    my_turn = true;
                    Pick = true;
                    if (Array_Time.GetComponent<Time_FlowScript>().Time_Over == true)
                    {
                        StartCoroutine(Time_Out());
                        StartCoroutine(Time_Out());
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                        Ready_Array = true;
                    }

                    if (Order[1].tag == "Character" && Order[2].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[1].GetComponent<Character>().character_Attack_Order = 2;
                            Order[2].GetComponent<Character>().character_Attack_Order = 3;
                            Arrayed_Data.instance.team1[1].GetComponent<Character>().Copy_Character_Stat(Order[1]);
                            Arrayed_Data.instance.team1[1].GetComponent<Character>().Debuging_Character();
                            Arrayed_Data.instance.team1[2].GetComponent<Character>().Copy_Character_Stat(Order[2]);
                            Arrayed_Data.instance.team1[2].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                        }
                    }
                    
                    break;
                case (int)ArrayPhase.SECOND34:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (Arrayed_Data.instance.team2[2].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[2].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    if (Arrayed_Data.instance.team2[3].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[3].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    my_turn = true;
                    Pick = true;
                    if (Array_Time.GetComponent<Time_FlowScript>().Time_Over == true)
                    {
                        StartCoroutine(Time_Out());
                        StartCoroutine(Time_Out());
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                        Ready_Array = true;
                    }
                    if (Order[3].tag == "Character" && Order[4].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[3].GetComponent<Character>().character_Attack_Order = 4;
                            Order[4].GetComponent<Character>().character_Attack_Order = 5;
                            Arrayed_Data.instance.team1[3].GetComponent<Character>().Copy_Character_Stat(Order[3]);
                            Arrayed_Data.instance.team1[3].GetComponent<Character>().Debuging_Character();
                            Arrayed_Data.instance.team1[4].GetComponent<Character>().Copy_Character_Stat(Order[4]);
                            Arrayed_Data.instance.team1[4].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            }
                        }
                    
                    break;
                case (int)ArrayPhase.SECOND5:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (Arrayed_Data.instance.team2[4].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[4].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    break;
            }
        }
        else
        {
            switch (Phase)
            {
                case (int)ArrayPhase.FIRST1:
                    my_turn = false;
                    if (Arrayed_Data.instance.team2[0].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[0].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    my_turn = true;
                    Pick = true;
                    if (Array_Time.GetComponent<Time_FlowScript>().Time_Over == true)
                    {
                        StartCoroutine(Time_Out());
                        StartCoroutine(Time_Out());
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                        Ready_Array = true;
                    }
                    if (Order[0].tag == "Character" && Order[1].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[0].GetComponent<Character>().character_Attack_Order = 1;
                            Order[1].GetComponent<Character>().character_Attack_Order = 2;
                            Arrayed_Data.instance.team1[0].GetComponent<Character>().Copy_Character_Stat(Order[0]);
                            Arrayed_Data.instance.team1[0].GetComponent<Character>().Debuging_Character();
                            Arrayed_Data.instance.team1[1].GetComponent<Character>().Copy_Character_Stat(Order[1]);
                            Arrayed_Data.instance.team1[1].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    my_turn = false;
                    if (Arrayed_Data.instance.team2[1].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[1].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    if (Arrayed_Data.instance.team2[2].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[2].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    my_turn = true;
                    Pick = true;
                    if (Array_Time.GetComponent<Time_FlowScript>().Time_Over == true)
                    {
                        StartCoroutine(Time_Out());
                        StartCoroutine(Time_Out());
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                        Ready_Array = true;
                    }
                    if (Order[2].tag == "Character" && Order[3].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[2].GetComponent<Character>().character_Attack_Order = 3;
                            Order[3].GetComponent<Character>().character_Attack_Order = 4;
                            Arrayed_Data.instance.team1[2].GetComponent<Character>().Copy_Character_Stat(Order[2]);
                            Arrayed_Data.instance.team1[2].GetComponent<Character>().Debuging_Character();
                            Arrayed_Data.instance.team1[3].GetComponent<Character>().Copy_Character_Stat(Order[3]);
                            Arrayed_Data.instance.team1[3].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    my_turn = false;
                    if (Arrayed_Data.instance.team2[3].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[3].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    if (Arrayed_Data.instance.team2[4].GetComponent<Character>().character_Num_Of_Grid != 0)
                    {
                        Enemy_Grid_Num = Reverse_Array(Arrayed_Data.instance.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Character_Setting(Arrayed_Data.instance.team2[4].GetComponent<Character>().character_ID);
                        Enemy_Grids[Enemy_Grid_Num - 1].GetComponent<Character>().Debuging_Character();
                        Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    my_turn = true;
                    Pick = true;
                    if (Array_Time.GetComponent<Time_FlowScript>().Time_Over == true)
                    {
                        StartCoroutine(Time_Out());
                        Array_Time.GetComponent<Time_FlowScript>().Time_Over = false;
                        Ready_Array = true;
                    }
                    if (Order[4].tag == "Character")
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[4].GetComponent<Character>().character_Attack_Order = 5;
                            Arrayed_Data.instance.team1[4].GetComponent<Character>().Copy_Character_Stat(Order[4]);
                            Arrayed_Data.instance.team1[4].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                        }
                    }
                    break;
            }
        }
    }
    private void Cancle_Sync(int num)
    {
        int Count = Arrayed_Data.instance.team1.Count - 1;
        //cancle_num = Arrayed_Data.instance.team1[num].GetComponent<Character>().character_Num_Of_Grid; -> 서버를 통해 상대에게 전달해 주어야 함.
        Arrayed_Data.instance.team1[num].GetComponent<Character>().Character_Reset();//id = 0;
        for (int i=num;i< Count; i++)
        {
            Arrayed_Data.instance.team1[i].GetComponent<Character>().Copy_Character_Stat(Arrayed_Data.instance.team1[i + 1]);
            Arrayed_Data.instance.team1[i].GetComponent<Character>().Debuging_Character();
        }
        if (num != 4)
        {
            Arrayed_Data.instance.team1[Count].GetComponent<Character>().Character_Reset();
            Arrayed_Data.instance.team1[Count].GetComponent<Character>().Debuging_Character();
        }
        arrData_Sync.DataSync();
        Debug.Log(arrData_Sync.test);
    }
    IEnumerator Sync_Character()
    {
        int Count = Order.Count - 1;
        Arrayed_Data.instance.team1[Count].GetComponent<Character>().Copy_Character_Stat(Order[Count]);
        Arrayed_Data.instance.team1[Count].GetComponent<Character>().Debuging_Character();
<<<<<<< Updated upstream
        arrData_Sync.DataSync();
        Debug.Log(arrData_Sync.test);
=======
        arrData_Sync.DataSync(Count);

>>>>>>> Stashed changes
        yield return null;
    }
    IEnumerator Time_Out()
    {
        Debug.Log("Arrayment_Manger: Time_Out() 호출");
        bool Inventory_ID_Time_Out = true;
        int Count = Order.Count - 1;
        while (Inventory_ID_Time_Out)
        {
            for(int i=0;i<Inventory.Length;i++)
            {
                if(Inventory[i].GetComponent<Inventory_ID>().is_Arrayed==false)
                {
                    Time_Out_Inventory = i;
                    Inventory_ID_Time_Out = false;
                }
            }
        }
        Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().is_Arrayed = true;
        int random = Random.Range(0, 9);
        while(Grids[random].tag == "Character")
        {
            random = Random.Range(0, 9);
        }

        Grids[random].GetComponent<Character>().character_ID = Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().m_Character_ID;
        Grids[random].GetComponent<Character>().Character_Setting(Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().m_Character_ID);
        Grids[random].GetComponent<Character>().character_Num_Of_Grid = random + 1;
        Grids[random].GetComponent<Character>().Debuging_Character();
        Grids[random].tag = "Character";
        Order.Add(Grids[random]);
        Arrayed_Data.instance.team1[Count].GetComponent<Character>().Copy_Character_Stat(Order[Count]);
        Arrayed_Data.instance.team1[Count].GetComponent<Character>().Debuging_Character();
        arrData_Sync.DataSync(Count);
        yield return null;
    }
    public void BanPick_Ready()
    {
        Ready_Array = true;
        if(On_Raycast==false)
        {
            Ready_Array = false;
        }
    }
    public void Get_Button(int num)// 인벤토리 클릭시 발생
    {
        if (my_turn == true && Pick == true)
            Character_instance = true;
        Prefeb_Character.GetComponent<Character>().Character_Setting(num);
    }
    private int Reverse_Array(int num)
    {
        switch (num)
        {
            case 1:
                num = 3;
                break;
            case 2:
                num = 2;
                break;
            case 3:
                num = 1;
                break;
            case 4:
                num = 6;
                break;
            case 5:
                num = 5;
                break;
            case 6:
                num = 4;
                break;
            case 7:
                num = 9;
                break;
            case 8:
                num = 8;
                break;
            case 9:
                num = 7;
                break;
        }
        return num;
    }
}