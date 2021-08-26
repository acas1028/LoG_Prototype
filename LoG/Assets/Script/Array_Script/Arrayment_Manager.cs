using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrayment_Manager : MonoBehaviourPun
{

    private bool is_click_inventory = false;
    public bool Ready_Array = false; // 레디 버튼을 눌렀다.
    private bool my_turn = true; // 나의 턴이다.
    private bool Pick = true;// true라면 배치. false라면 정보를 읽을수만있다. 추가 배치를 막는다.
    private bool On_Raycast = false;//배치를 다했다.
    private int Phase = (int)ArrayPhase.STANDBY;
    private int Time_Out_Inventory;
    private int cancle_num;
    private int click_id;

    public GameObject Array_Time;
    public GameObject[] Grids;
    public GameObject[] Enemy_Grids;
    public GameObject[] Inventory;
    public GameObject PopUp_UI;
    public GameObject Array_Cancle_Button;
    public GameObject PopUp_Manager;
    private GameObject Cancle_Character;
    public ArrRoomManager arrRoomManager;
    public ArrData_Sync arrData_Sync;
    public Character_arrayment_showing character_Arrayment_Showing;

    void Start()
    {
        Phase = arrRoomManager.GetArrayPhase();
        StartCoroutine("Get_Inventory_ID");

        for (int i = 0; i < 5; i++)
        {
            GameObject ac = Arrayed_Data.instance.team1[i];
            if (ac.GetComponent<Character>().character_ID != 0)
            {
                Character c = Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].GetComponentInChildren<Character>();
                Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].tag = "Character";
                c.Copy_Character_Stat(ac);
                c.InitializeCharacterSprite();
            }

            ac = Arrayed_Data.instance.team2[i];
            if (ac.GetComponent<Character>().character_ID != 0)
            {
                Character c = Enemy_Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].GetComponentInChildren<Character>();
                Enemy_Grids[ac.GetComponent<Character>().character_Num_Of_Grid - 1].tag = "Character";
                c.Copy_Character_Stat(ac);
                c.InitializeCharacterSprite();
            }
        }

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable() { { "RoundWinCount", 0 } };
        PhotonNetwork.SetPlayerCustomProperties(table);

        if (PhotonNetwork.IsMasterClient)
        {
            table = new ExitGames.Client.Photon.Hashtable() { { "RoundCount", 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(table);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Arrayment_Raycast();
        }
        Array_Order();
    }

    public void Arrayment_Raycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Arrayed_Data T = Arrayed_Data.instance;
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

        if (!hit)
            return;

        if(hit.transform.tag == "Character") //자신이나 상대의 캐릭터를 클릭한 경우.
        {
            //PopUp_Manager.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(hit.collider.gameObject.GetComponent<Character>().character_ID);
            //클릭한 캐릭터의 정보를 보여준다

            for (int i = 0; i < 9; i++)
            {
                if (hit.transform.gameObject == Grids[i])//자신의 그리드에 있는 캐릭터를 클릭한 경우.
                {
                    if (my_turn == true)//자신의 턴이면 취소가 가능하다
                    {
                        Cancle_Character = hit.transform.gameObject;
                        Array_Cancle_Button.SetActive(!Array_Cancle_Button.activeSelf);
                    }
                    else//자신의 턴이 아니면 취소가 불가능 하다.
                    {
                        Array_Cancle_Button.SetActive(false);
                    }
                }
            }
        }
        if(hit.transform.tag == "Null_Character" && is_click_inventory == true)// 인벤토리를 누르고, 빈 그리드를 클릭 -> 정상적인 배치를 한 경우.
        {
            GameObject Grid_Character = hit.transform.gameObject;
            Character cs = Grid_Character.GetComponentInChildren<Character>();
            Deck_Data_Send sd = Deck_Data_Send.instance;
            cs.Copy_Character_Stat(sd.Save_Data[0, click_id - 1]);
            Grid_Character.tag = "Character";
            for(int i=0;i<9;i++)
            {
                if (Grid_Character == Grids[i])
                {
                    cs.character_Num_Of_Grid = i + 1;
                }
            }
            cs.Debuging_Character();
            cs.InitializeCharacterSprite();
            int j = 0;
            while(j<5)
            {
                if (T.team1[j].GetComponent<Character>().character_ID == 0)
                {
                    T.team1[j].GetComponent<Character>().Copy_Character_Stat(Grid_Character.transform.Find("Character_Prefab").gameObject);
                    T.team1[j].GetComponent<Character>().Debuging_Character();
                    break;
                }
                j++;
            }
            Inventory[click_id - 1].GetComponent<Inventory_ID>().Block_Inventory.SetActive(true);
            Sync_Character();
            is_click_inventory = false;
        }
    }
    public void Array_Order()
    {
        Phase = arrRoomManager.GetArrayPhase();
        int Enemy_Grid_Num;
        Arrayed_Data cs = Arrayed_Data.instance;
        if (arrRoomManager.IsPlayerPreemptive())
        {
            switch (Phase)
            {
                case (int)ArrayPhase.FIRST1:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[0].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[0].GetComponent<Character>().character_Attack_Order = 1;
                            cs.team1[0].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[0].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[0]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[0].GetComponent<Character>().character_Attack_Order = 1;
                        }
                        else
                        {
                            Debug.Log(cs.team2[0].GetComponent<Character>().character_ID);
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        
                        if (cs.team2[1].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[1]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[1].GetComponent<Character>().character_Attack_Order = 2;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        
                       arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[1].GetComponent<Character>().character_ID != 0 && cs.team1[2].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[1].GetComponent<Character>().character_Attack_Order = 2;
                            cs.team1[1].GetComponent<Character>().Debuging_Character();
                            cs.team1[2].GetComponent<Character>().character_Attack_Order = 3;
                            cs.team1[2].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[2].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[2]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[2].GetComponent<Character>().character_Attack_Order = 3;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        if (cs.team2[3].GetComponent<Character> ().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[3]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[3].GetComponent<Character>().character_Attack_Order = 4;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[3].GetComponent<Character>().character_ID != 0 && cs.team1[4].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[3].GetComponent<Character>().character_Attack_Order = 4;
                            cs.team1[3].GetComponent<Character>().Debuging_Character();
                            cs.team1[4].GetComponent<Character>().character_Attack_Order = 5;
                            cs.team1[4].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[4].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[4]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[4].GetComponent<Character>().character_Attack_Order = 5;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
            }
        }
        else
        {
            switch (Phase)
            {

                case (int)ArrayPhase.FIRST1:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[0].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[0]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[0].GetComponent<Character>().character_Attack_Order = 1;
                        }
                        else
                        {
                            Debug.Log(cs.team2[0].GetComponent<Character>().character_ID);
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[0].GetComponent<Character>().character_ID != 0 && cs.team1[1].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[0].GetComponent<Character>().character_Attack_Order = 1;
                            cs.team1[0].GetComponent<Character>().Debuging_Character();
                            cs.team1[1].GetComponent<Character>().character_Attack_Order = 2;
                            cs.team1[1].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[1].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[1]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[1].GetComponent<Character>().character_Attack_Order = 2;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        if (cs.team2[2].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[2]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[2].GetComponent<Character>().character_Attack_Order = 3;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[2].GetComponent<Character>().character_ID != 0 && cs.team1[3].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[2].GetComponent<Character>().character_Attack_Order = 3;
                            cs.team1[2].GetComponent<Character>().Debuging_Character();
                            cs.team1[3].GetComponent<Character>().character_Attack_Order = 4;
                            cs.team1[3].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (cs.team2[3].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[3]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[3].GetComponent<Character>().character_Attack_Order = 4;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        }
                        if (cs.team2[4].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(cs.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(cs.team2[4]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Debuging_Character();
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            cs.team2[4].GetComponent<Character>().character_Attack_Order = 5;
                        }
                        else
                        {
                            Enemy_Grids[cancle_num].GetComponent<Character>().Character_Reset();
                            Enemy_Grids[cancle_num].GetComponent<Character>().Debuging_Character();
                            Enemy_Grids[cancle_num].GetComponent<Character>().InitializeCharacterSprite();
                            Enemy_Grids[cancle_num].tag = "Null_Character";
                        } 
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    my_turn = true;
                    Pick = true;
                    if (cs.team1[4].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            cs.team1[4].GetComponent<Character>().character_Attack_Order = 5;
                            cs.team1[4].GetComponent<Character>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                        }
                    }
                    break;
            }
        }
    }

    public void Cancle_Array()
    {
        Character cs = Cancle_Character.GetComponentInChildren<Character>();
        Arrayed_Data T = Arrayed_Data.instance;
        for (int i = 0; i < 5; i++)
        {
            if (cs.character_ID == T.team1[i].GetComponent<Character>().character_ID)
            {
                Array_Cancle_Button.SetActive(false);
                photonView.RPC("Cancle_Sync", RpcTarget.All, i);
                Order_Rearrange(i);
                arrData_Sync.DataSync(i);
            }
        }
        character_Arrayment_Showing.Set_AttackRange_Ui(false);
        Cancle_Character.tag = "Null_Character";
        cs.Character_Reset();
        cs.Debuging_Character();
        cs.InitializeCharacterSprite();
    }

    [PunRPC]
    private void Cancle_Sync(int i)
    {
        arrData_Sync.DataSync(i);
        if(character_Arrayment_Showing.is_Mine==true)
        {
            character_Arrayment_Showing.cancel_Character = Arrayed_Data.instance.team1[i];
            character_Arrayment_Showing.temp = Arrayed_Data.instance.team1[i].GetComponent<Character>().character_Attack_Range;
        }
        else
        {
            character_Arrayment_Showing.cancel_Character = Arrayed_Data.instance.team2[i];
            character_Arrayment_Showing.temp = Arrayed_Data.instance.team2[i].GetComponent<Character>().character_Attack_Range;
        }
        cancle_num = Reverse_Array(Arrayed_Data.instance.team2[i].GetComponent<Character>().character_Num_Of_Grid)-1;
    }
    public void Sync_Character()
    {
        Arrayed_Data T = Arrayed_Data.instance;
        int Return_num = 0;
        while(Return_num<4)
        {
            if (T.team1[Return_num + 1].GetComponent<Character>().character_ID == 0)
            {
                break;
            }
            Return_num++;
        }
        arrData_Sync.DataSync(Return_num);
    }
   IEnumerator Time_OUT()
   {
        int inventroy_Leng = 0;
        Arrayed_Data cs = Arrayed_Data.instance;
        while (inventroy_Leng<Inventory.Length)
        {
            if (Inventory[inventroy_Leng].GetComponent<Inventory_ID>().is_Arrayed == false)
            {
                Time_Out_Inventory = inventroy_Leng;
            }
        }

        inventroy_Leng++;
        
        Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().is_Arrayed = true;
        int random = Random.Range(0, 9);
        while(Grids[random].tag == "Character")
        {
            random = Random.Range(0, 9);
        }

        //Grids[random].GetComponent<Character>().Character_Setting();
        Grids[random].GetComponentInChildren<Character>().character_Num_Of_Grid = random + 1;
        Grids[random].GetComponentInChildren<Character>().Debuging_Character();
        Grids[random].GetComponentInChildren<Character>().InitializeCharacterSprite();

        int j = 0;
        while(j<5)
        {
            if(cs.team1[j].GetComponent<Character>().character_ID==0)
            {
                cs.team1[j].GetComponent<Character>().Copy_Character_Stat(Grids[random]);
                cs.team1[j].GetComponent<Character>().Debuging_Character();
                arrData_Sync.DataSync(j);
                break;
            }
        }
        yield return null;
    }


    private void Order_Rearrange(int num)
    {
        Arrayed_Data cs = Arrayed_Data.instance;
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory_ID inven = Inventory[i].GetComponent<Inventory_ID>();
            if (inven.inventory_ID == cs.team1[num].GetComponent<Character>().character_ID)
            {
                inven.is_Arrayed = false;
                inven.Block_Inventory.SetActive(false);
            }
        }
        cs.team1[num].GetComponent<Character>().Character_Reset();
        cs.team1[num].GetComponent<Character>().Debuging_Character();
        for (int i = num; i < 4; i++)
        {
            if (cs.team1[i + 1].GetComponent<Character>().character_ID != 0)
            {
                cs.team1[i].GetComponent<Character>().Copy_Character_Stat(cs.team1[i + 1]);
                cs.team1[i].GetComponent<Character>().Debuging_Character();
            }
            else
            {
                cs.team1[i].GetComponent<Character>().Character_Reset();
                cs.team1[i].GetComponent<Character>().Debuging_Character();
            }
        }
        if (cs.team1[4].GetComponent<Character>().character_ID != 0)
        {
            cs.team1[4].GetComponent<Character>().Character_Reset();
            cs.team1[4].GetComponent<Character>().Debuging_Character();
        }
    }
    public void Click_Inventory(int num)
    {
        if (my_turn == true&&Pick==true)
            is_click_inventory = true;
        click_id = num;
    }
    public void BanPick_Ready()
    {
        Ready_Array = true;
        if (On_Raycast == false)
        {
            Ready_Array = false;
        }
    }
    IEnumerator Get_Inventory_ID()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory_ID cs = Inventory[i].GetComponent<Inventory_ID>();
            Character sv = Deck_Data_Send.instance.Save_Data[0, i].GetComponent<Character>();
            cs.inventory_ID = sv.character_ID;
        }
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