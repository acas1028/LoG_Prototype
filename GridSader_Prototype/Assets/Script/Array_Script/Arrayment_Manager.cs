using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Manager : MonoBehaviour
{

    private bool is_click_inventory = false;
    public bool Ready_Array = false; // 레디 버튼을 눌렀다.
    private bool my_turn = false; // 나의 턴이다.
    private bool Pick = false;// true라면 배치. false라면 정보를 읽을수만있다.
    private bool On_Raycast = false;//배치를 다했다.
    private int Phase = (int)ArrayPhase.STANDBY;
    private int Time_Out_Inventory;
    private int cancle_num;
    private int click_id;

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
    public PhotonView PV;

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
        if(Input.GetMouseButtonDown(0))
        {
            Arrayment_Raycast();
        }
    }

    public void Arrayment_Raycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

        if (!hit)
            return;

        if(hit.transform.tag=="Character") //자신이나 상대의 캐릭터를 클릭한 경우.
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
                        Array_Cancle_Button.SetActive(true);
                    }
                    else//자신의 턴이 아니면 취소가 불가능 하다.
                    {
                        Array_Cancle_Button.SetActive(false);
                    }
                }
            }
        }
        if(hit.transform.tag=="Null_Character"&&is_click_inventory==true)// 인벤토리를 누르고, 빈 그리드를 클릭 -> 정상적인 배치를 한 경우.
        {
            GameObject Grid_Character = hit.transform.gameObject;
            Character cs = Grid_Character.GetComponent<Character>();
            cs.Character_Setting(click_id);
            Grid_Character.tag = "Character";
            for(int i=0;i<9;i++)
            {
                if (Grid_Character == Grids[i])
                {
                    cs.character_Num_Of_Grid = i + 1;
                }
            }
            cs.Debuging_Character();
            int j = 0;
            while(j<5)
            {
                if (Order[j].GetComponent<Character>().character_ID == 0)
                {
                    Order[j].GetComponent<Character>().Copy_Character_Stat(Grid_Character);
                    Order[j].GetComponent<Character>().Debuging_Character();
                    Order[j].tag = "Character";
                    break;
                }
                j++;
            }
            for(int i=0;i<Inventory.Length;i++)
            {
                Inventory_ID inven = Inventory[i].GetComponent<Inventory_ID>();
                if(inven.m_Character_ID==cs.character_ID)
                {
                    inven.is_Arrayed = true;
                }
            }
            Sync_Character();
            is_click_inventory = false;
        }
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
                    if(Order[0].GetComponent<Character>().character_ID!=0)
                    {

                    }
                    break;
            }
        }
    }
    public void Cancle_Array()
    {
        Character cs = Cancle_Character.GetComponent<Character>();
        for(int i=0;i<5;i++)
        {
            if(cs.character_ID==Order[i].GetComponent<Character>().character_ID)
            {
                Order_Rearrange(i);
                PV.RPC("Cancle_Sync",RpcTarget.All,i);
            }
        }
        cs.tag = "Null_Character";
        cs.Character_Reset();
        cs.Debuging_Character();
    }
    IEnumerator Cancle_Sync(int i)
    {
        Arrayed_Data cs = Arrayed_Data.instance;
        cs.team1[i].GetComponent<Character>().Character_Reset();
        cs.team1[i].GetComponent<Character>().Debuging_Character();
        arrData_Sync.DataSync(i);
        yield return null;
    }
    public void Sync_Character()
    {
        int Return_num = 0;
        while(Return_num<4)
        {
            if (Order[Return_num + 1].GetComponent<Character>().character_ID == 0)
            {
                break;
            }
            Return_num++;
        }
        arrData_Sync.DataSync(Return_num);
    }
    private void Order_Rearrange(int num)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory_ID inven = Inventory[i].GetComponent<Inventory_ID>();
            if (inven.m_Character_ID == Order[num].GetComponent<Character>().character_ID)
            {
                inven.is_Arrayed = false;
            }
        }
        Order[num].GetComponent<Character>().Character_Reset();
        Order[num].GetComponent<Character>().Debuging_Character();
        for (int i = num; i < 4; i++)
        {
            if (Order[i + 1].GetComponent<Character>().character_ID != 0)
            {
                Order[i].GetComponent<Character>().Copy_Character_Stat(Order[i + 1]);
                Order[i].GetComponent<Character>().Debuging_Character();
            }
            else
            {
                Order[i + 1].GetComponent<Character>().Character_Reset();
                Order[i + 1].GetComponent<Character>().Debuging_Character();
            }
        }
        if (Order[4].GetComponent<Character>().character_ID != 0)
        {
            Order[4].GetComponent<Character>().Character_Reset();
            Order[4].GetComponent<Character>().Debuging_Character();
        }
    }
    public void Click_Inventory(int num)
    {
        if (my_turn == true)
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