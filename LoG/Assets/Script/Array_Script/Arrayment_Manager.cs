using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Manager : MonoBehaviourPun
{
    #region
    private int lastPageNum;
    [SerializeField]
    private bool is_click_inventory = false;
    public bool Ready_Array = false; // 레디 버튼을 눌렀다.
    private bool my_turn = true; // 나의 턴이다.
    private bool Pick = true;// true라면 배치. false라면 정보를 읽을수만있다. 추가 배치를 막는다.
    [SerializeField]
    private bool On_Raycast = false;//배치를 다했다.
    [SerializeField]
    private bool b_isCancle = false;
    private bool b_isMyCancle = false;
    private int Phase = (int)ArrayPhase.STANDBY;
    [SerializeField]
    private int cancle_num;
    private int arrayedThisTurn;
    private int InvenNum;

    private List<int> invenNums;
    private List<int> gridNums;
    private List<GameObject> SwitchCharacterTag = new List<GameObject>();

    public GameObject Array_Time;
    public GameObject[] Grids;
    public GameObject[] Enemy_Grids;
    public GameObject[] GridsHighLight;
    public GameObject[] Inventory;
    public GameObject PopUp_UI;
    public GameObject Array_Cancle_Button;
    public GameObject PopUp_Manager;
    private GameObject Cancle_Character;
    public GameObject move_object;
    public GameObject arive_object;

    public Text timeText;
    public ArrRoomManager arrRoomManager;
    public ArrData_Sync arrData_Sync;
    public Character_arrayment_showing character_Arrayment_Showing;
    private Arrayed_Data saved;
    #endregion//변수들 

    void Start()
    {
        saved = Arrayed_Data.instance;
        ResetArrayedData();

        lastPageNum = Deck_Data_Send.instance.lastPageNum;
        Phase = arrRoomManager.GetArrayPhase();
        StartCoroutine("Get_Inventory_ID");

        arrayedThisTurn = 0;

        invenNums = new List<int>();
        gridNums = new List<int>();

        for (int i = 1; i < 8; i++)
        {
            invenNums.Add(i);
        }
        for (int i = 1; i < 10; i++)
        {
            gridNums.Add(i);
        }
        ShuffleList<int>(invenNums);
        ShuffleList<int>(gridNums);

        InventoryBlock();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Arrayment_Raycast();
        }
        Array_Order();
    }

    public void InventoryBlock()
    {
        for (int i = 0; i < 7; i++)
        {
            Inventory[i].GetComponent<Inventory_ID>().SetArrayed();
        }
    }

    public void InventoryUnblock()
    {
        for (int i = 0; i < 7; i++)
        {
            Inventory[i].GetComponent<Inventory_ID>().SetNotArrayed();
        }
    }

    // 지금은 사용되지 않는 함수
    // Arraed_Data에 있는 데이터에 따라 그리드에 캐릭터를 배치
    private void InitGridCharacters()
    {
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
    }

    public void ResetArrayedData()
    {
        for (int i = 0; i < 5; i++)
        {
            saved.team1[i].GetComponent<Character>().Character_Reset();
            saved.team2[i].GetComponent<Character>().Character_Reset();
        }
    }

    public void Arrayment_Raycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Arrayed_Data T = Arrayed_Data.instance;
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

        if (!hit)
            return;

        if (hit.transform.tag == "Character") //자신이나 상대의 캐릭터를 클릭한 경우.
        {
            //PopUp_Manager.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(hit.collider.gameObject.GetComponent<Character>().character_ID);
            //클릭한 캐릭터의 정보를 보여준다

            //캐릭터 클릭시 팝업 생성 및 삭제
            if (hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.activeSelf == false)
            {
                hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.SetActive(true);
                hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).transform.GetChild(0).GetComponent<ShowingCharacterStat_In_Arrayment>().ShowingStatInarray();
                Limit_Popup.Limit_Poup_instance.SetIsbutton(this.gameObject);
                Limit_Popup.Limit_Poup_instance.SetPopup(hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject);
                Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(true);
            }
            else
            {
                hit.transform.gameObject.GetComponent<GridCharacter_To_PopupPosition>().Popup_Position.transform.GetChild(0).gameObject.SetActive(false);
                Limit_Popup.Limit_Poup_instance.SetIsbutton(null);
                Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(false);
            }

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
        if (hit.transform.tag == "Null_Character" && is_click_inventory == true)// 인벤토리를 누르고, 빈 그리드를 클릭 -> 정상적인 배치를 한 경우.
        {
            GameObject Grid_Character = hit.transform.gameObject;
            int gridNum = 0;
            cancle_num = 0;
            SwitchCharacterTag.Add(Grid_Character);
            for (int i = 0; i < 9; i++)
            {
                if (Grid_Character == Grids[i])
                {
                    gridNum = i + 1;
                    break;
                }
            }

            if (gridNum <= 0 || gridNum > 9)
            {
                Debug.LogError("Grid_Character 에 해당하는 Grids[i]의 개체 찾을 수 없음");
                return;
            }

            if(InvenNum<=0||InvenNum>7)
            {
                Debug.LogError("범위를 벗어남");
                return;
            }

            ArrayOnGrid(InvenNum, gridNum);
            is_click_inventory = false;
        }
    }

    private void ArrayOnGrid(int inventoryNum, int gridNum)
    {
        Grids[gridNum - 1].tag = "Character";
        Character gridCharacter = Grids[gridNum - 1].GetComponentInChildren<Character>();
        gridCharacter.Copy_Character_Stat(Deck_Data_Send.instance.Save_Data[lastPageNum, inventoryNum - 1]);
        gridCharacter.character_Num_Of_Grid = gridNum;
        gridCharacter.InitializeCharacterSprite();

        Arrayed_Data arrayedData = Arrayed_Data.instance;
        for (int i = 0; i < 5; i++)
        {
            if (arrayedData.team1[i].GetComponent<Character>().character_ID == 0)
            {
                arrayedData.team1[i].GetComponent<Character>().Copy_Character_Stat(Grids[gridNum - 1].transform.Find("Character_Prefab").gameObject);
                break;
            }
        }
        Inventory[inventoryNum - 1].GetComponent<Inventory_ID>().SetArrayed();
        Sync_Character();
        arrayedThisTurn++;
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
                    TimeOut(1 - arrayedThisTurn);
                    if (saved.team1[0].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[0].GetComponent<Character>().character_Attack_Order = 1;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[0].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[0]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[0].GetComponent<Character>().character_Attack_Order = 1;
                        }
                        if (saved.team2[1].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[1]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[1].GetComponent<Character>().character_Attack_Order = 2;
                        }

                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    my_turn = true;
                    Pick = true;
                    TimeOut(2 - arrayedThisTurn);
                    if (saved.team1[1].GetComponent<Character>().character_ID != 0 && saved.team1[2].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[1].GetComponent<Character>().character_Attack_Order = 2;
                            saved.team1[2].GetComponent<Character>().character_Attack_Order = 3;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[2].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[2]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[2].GetComponent<Character>().character_Attack_Order = 3;
                        }
                        if (saved.team2[3].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[3]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[3].GetComponent<Character>().character_Attack_Order = 4;
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    my_turn = true;
                    Pick = true;
                    TimeOut(2 - arrayedThisTurn);
                    if (saved.team1[3].GetComponent<Character>().character_ID != 0 && saved.team1[4].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[3].GetComponent<Character>().character_Attack_Order = 4;
                            saved.team1[4].GetComponent<Character>().character_Attack_Order = 5;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[4].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[4]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[4].GetComponent<Character>().character_Attack_Order = 5;
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
            }
            b_isCancle = false;
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
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[0].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[0].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[0]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[0].GetComponent<Character>().character_Attack_Order = 1;
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    my_turn = true;
                    Pick = true;
                    TimeOut(2 - arrayedThisTurn);
                    if (saved.team1[0].GetComponent<Character>().character_ID != 0 && saved.team1[1].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[0].GetComponent<Character>().character_Attack_Order = 1;
                            saved.team1[1].GetComponent<Character>().character_Attack_Order = 2;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[1].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[1].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[1]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[1].GetComponent<Character>().character_Attack_Order = 2;
                        }
                        if (saved.team2[2].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[2].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[2]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[2].GetComponent<Character>().character_Attack_Order = 3;
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    my_turn = true;
                    Pick = true;
                    TimeOut(2 - arrayedThisTurn);
                    if (saved.team1[2].GetComponent<Character>().character_ID != 0 && saved.team1[3].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[2].GetComponent<Character>().character_Attack_Order = 3;
                            saved.team1[3].GetComponent<Character>().character_Attack_Order = 4;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    if (PhotonNetwork.OfflineMode)
                        arrData_Sync.SetArrayPhaseInOffline();
                    my_turn = false;
                    if (arrData_Sync.is_datasync == true)
                    {
                        if (b_isCancle)
                        {
                            ResetEnemyGrid();
                        }
                        if (saved.team2[3].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[3].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[3]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[3].GetComponent<Character>().character_Attack_Order = 4;
                        }
                        if (saved.team2[4].GetComponent<Character>().character_ID != 0)
                        {
                            Enemy_Grid_Num = Reverse_Array(saved.team2[4].GetComponent<Character>().character_Num_Of_Grid);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().Copy_Character_Stat(saved.team2[4]);
                            Enemy_Grids[Enemy_Grid_Num - 1].GetComponentInChildren<Character>().InitializeCharacterSprite();
                            Enemy_Grids[Enemy_Grid_Num - 1].tag = "Character";
                            saved.team2[4].GetComponent<Character>().character_Attack_Order = 5;
                        }
                        arrData_Sync.is_datasync = false;
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    my_turn = true;
                    Pick = true;
                    TimeOut(1 - arrayedThisTurn);
                    if (saved.team1[4].GetComponent<Character>().character_ID != 0)
                    {
                        On_Raycast = true;
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            saved.team1[4].GetComponent<Character>().character_Attack_Order = 5;
                            Debug.Log((ArrayPhase)Phase + "배치 완료");
                            arrRoomManager.StartArrayPhase();
                            Ready_Array = false;
                            On_Raycast = false;
                            arrData_Sync.is_datasync = false;
                            arrayedThisTurn = 0;
                        }
                    }
                    break;
            }
        }
    }
    public void Cancle_Array()
    {
        b_isMyCancle = true;

        for (int i = 0; i < SwitchCharacterTag.Count; i++)
        {
            if (SwitchCharacterTag[i] == Cancle_Character)
                SwitchCharacterTag.RemoveAt(i);
        }
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
        cs.InitializeCharacterSprite();
        arrayedThisTurn--;
        GridOffHighLight();

        b_isMyCancle = false;
    }

    [PunRPC]
    private void Cancle_Sync(int i)
    {
        b_isCancle = true;

        if (!b_isMyCancle)
        {
            cancle_num = Reverse_Array(Arrayed_Data.instance.team2[i].GetComponent<Character>().character_Num_Of_Grid);
        }
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
        if (Cancle_Character==null)
        {
            return;
        }
    }
    private void ResetEnemyGrid()
    {
        Enemy_Grids[cancle_num-1].GetComponentInChildren<Character>().Character_Reset();
        Enemy_Grids[cancle_num-1].GetComponentInChildren<Character>().InitializeCharacterSprite();
        Enemy_Grids[cancle_num-1].tag = "Null_Character";
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
    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
    private void TimeOut(int count)
    {
        if (!timeText.GetComponent<Time_FlowScript>().Time_Over)
            return;

        for (int i = 0; i < count; i++)
        {
            int ranInvenIdx = Random.Range(0, 7);
            int ranGridIdx = Random.Range(0, 9);

            ArrayOnGrid(ranInvenIdx, ranGridIdx);
        }

        Ready_Array = true;
        timeText.GetComponent<Time_FlowScript>().Time_Over = false;
    }

    public void Move_Grid()
    {
        Cancle_Character = move_object;
        int ID = move_object.GetComponentInChildren<Character>().character_ID;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if(ID==Inventory[i].GetComponent<Inventory_ID>().GetCharacterID())
            {
                ID = Inventory[i].GetComponent<Inventory_ID>().GetInventoryNum();
                break;
            }
        }
        int GN=0;
        for (int i = 0; i < Grids.Length; i++)
        {
            if (arive_object == Grids[i])
            {
                GN = i+1;
                break;
            }        
        }
        Cancle_Array();
        ArrayOnGrid(ID, GN);
    }
    private void Order_Rearrange(int num)
    {
        Arrayed_Data cs = Arrayed_Data.instance;
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory_ID inven = Inventory[i].GetComponent<Inventory_ID>();
            if (inven.GetCharacterID() == saved.team1[num].GetComponent<Character>().character_ID)
            {
                inven.SetNotArrayed();
            }
        }
        saved.team1[num].GetComponent<Character>().Character_Reset();
        for (int i = num; i < 4; i++)
        {
            if (saved.team1[i + 1].GetComponent<Character>().character_ID != 0)
            {
                saved.team1[i].GetComponent<Character>().Copy_Character_Stat(saved.team1[i + 1]);
            }
            else
            {
                saved.team1[i].GetComponent<Character>().Character_Reset();
            }
        }
        if (saved.team1[4].GetComponent<Character>().character_ID != 0)
        {
            saved.team1[4].GetComponent<Character>().Character_Reset();
        }
    }
    public void Click_Inventory(int num)
    {
        if (my_turn == true&&Pick==true)
        {
            is_click_inventory = true;
            InvenNum = Inventory[num-1].GetComponent<Inventory_ID>().GetInventoryNum();
        }

    }
    public void BanPick_Ready()
    {
        Ready_Array = true;
        if (On_Raycast == false)
        {
            Ready_Array = false;
        }
        for (int i = 0; i < SwitchCharacterTag.Count; i++)
        {
            SwitchCharacterTag[i].tag = "Player";
        }
        SwitchCharacterTag.Clear();
    }
    IEnumerator Get_Inventory_ID()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory_ID cs = Inventory[i].GetComponent<Inventory_ID>();
            Character sv = Deck_Data_Send.instance.Save_Data[lastPageNum, i].GetComponent<Character>();
            cs.SetCharacterID(sv.character_ID);
        }
    }

    public void GridsOnHighLight(int num)
    {
        GridsHighLight[num].SetActive(true);
    }
    public void GridsOffHighLight(int num)
    {
        GridsHighLight[num].SetActive(false);
    }
    private void GridOffHighLight()
    {
        for (int i = 0; i < 9; i++)
            GridsHighLight[i].SetActive(false);
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