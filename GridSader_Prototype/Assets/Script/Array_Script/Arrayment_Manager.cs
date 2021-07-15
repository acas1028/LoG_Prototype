using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Manager: MonoBehaviour
{
   
    private bool Character_instance = true;
    public bool Ready_Array = false;
    private bool my_turn = true;
    private bool Pick = true;
    private int Phase = (int)ArrayPhase.STANDBY;
    private int Time_Out_Inventory;

    GameObject Character_Instantiate;
    [Tooltip("������� ĳ����")]
    public GameObject Prefeb_Character;
    public List<GameObject> Order = new List<GameObject>();

    public GameObject Array_Time;
    public GameObject[] Grids;
    private GameObject[] Inventory;
    public GameObject[] Block_Inventory;
    public GameObject PopUp_UI;
    public GameObject Array_Cancle_Button;
    public GameObject PopUp_Manager;
    public GameObject PassData_;
    private GameObject Cancle_Character;
    public ArrRoomManager arrRoomManager;

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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
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
                PopUp_Manager.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(hit.collider.gameObject.GetComponent<Character_Script>().character_ID);
                Cancle_Character = hit.transform.gameObject;
                if(my_turn==true)
                {
                    Array_Cancle_Button.SetActive(true);
                }
                PopUp_UI.SetActive(true);                   
            }
            else
            {
                //Array_Cancle_Button.SetActive(false);
            }
            if (hit.transform.CompareTag("Null_Character")&&Character_instance == true)//Null_Character�� �±� �Ǿ� �ִ� ��ü���� raycast�� ������.
            {
                Character_Instantiate = hit.collider.gameObject;
                Character_Instantiate.tag = "Character";//Character�� �±׸� �����Ѵ�. ����ó��.
                Character_Instantiate.GetComponent<Character_Script>().character_ID = Prefeb_Character.GetComponent<Character_Script>().character_ID;
                Character_Instantiate.GetComponent<Character_Script>().Character_Setting(Character_Instantiate.GetComponent<Character_Script>().character_ID);
                for (int i = 0; i < Grids.Length; i++)//ĳ���Ϳ� Grid_Number ����
                {
                    if (Character_Instantiate == Grids[i])
                    {
                        Character_Instantiate.GetComponent<Character_Script>().character_Num_Of_Grid = i + 1;
                    }
                }
                for (int i = 0; i < Inventory.Length; i++)//�κ��丮 ����ȭ
                {
                if (Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Character_Instantiate.GetComponent<Character_Script>().character_ID) //ID�� ���� �ǹǷ� ���� �ؾ���.
                {

                    Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = true;
                }

                }
                Character_Instantiate.GetComponent<Character_Script>().Debuging_Character();
                Order.Add(Character_Instantiate);
                PopUp_UI.SetActive(false);
                Character_instance = false;
            }
        }
    }
    public void Cancle_Array()
    {

            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Cancle_Character.GetComponent<Character_Script>().character_ID)
                {
                    Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = false;
                }
            }
            for (int i = 0; i < Order.Count; i++)
            {
                if (Cancle_Character == Order[i])
                {
                    Order.Remove(Order[i]);
                }
            }
            Cancle_Character.tag = "Null_Character";
            Cancle_Character.GetComponent<Character_Script>().Character_Reset();
            Cancle_Character.GetComponent<SpriteRenderer>().sprite = null;
            Cancle_Character.GetComponent<Character_Script>().Debuging_Character();
    }
    public void Array_Order()
    {
        Phase = arrRoomManager.GetArrayPhase();

        if (PhotonNetwork.IsMasterClient)
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
                    if (Order[0].tag=="Character")
                    {
                        Pick = false;
                        if(Ready_Array == true)
                        {
                            Order[0].GetComponent<Character_Script>().character_Attack_Order = 1;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[0].GetComponent<Character_Script>().Copy_Character_Stat(Order[0]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[0].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND12:
                    my_turn = false;
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
                    if (Order[1].tag=="Character"&&Order[2].tag=="Character")
                    {
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[1].GetComponent<Character_Script>().character_Attack_Order = 2;
                            Order[2].GetComponent<Character_Script>().character_Attack_Order = 3;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[1].GetComponent<Character_Script>().Copy_Character_Stat(Order[1]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[1].GetComponent<Character_Script>().Debuging_Character();
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[2].GetComponent<Character_Script>().Copy_Character_Stat(Order[2]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[2].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND34:
                    my_turn = false;
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
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[3].GetComponent<Character_Script>().character_Attack_Order = 4;
                            Order[4].GetComponent<Character_Script>().character_Attack_Order = 5;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[3].GetComponent<Character_Script>().Copy_Character_Stat(Order[3]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[3].GetComponent<Character_Script>().Debuging_Character();
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[4].GetComponent<Character_Script>().Copy_Character_Stat(Order[4]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[4].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.SECOND5:
                    my_turn = false;
                    break;
            }
        }
        else
        {
            switch (Phase)
            {
                case (int)ArrayPhase.FIRST1:
                    my_turn = false;
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
                        Pick = false;
                        //�غ� �Ϸ� ��ư Ȱ��ȭ
                        if (Ready_Array == true)
                        {
                            Order[0].GetComponent<Character_Script>().character_Attack_Order = 1;
                            Order[1].GetComponent<Character_Script>().character_Attack_Order = 2;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[0].GetComponent<Character_Script>().Copy_Character_Stat(Order[0]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[0].GetComponent<Character_Script>().Debuging_Character();
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[1].GetComponent<Character_Script>().Copy_Character_Stat(Order[1]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[1].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST23:
                    my_turn = false;
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
                        Pick = false;
                        //�غ� �Ϸ� ��ư Ȱ��ȭ
                        if (Ready_Array == true)
                        {
                            Order[2].GetComponent<Character_Script>().character_Attack_Order = 3;
                            Order[3].GetComponent<Character_Script>().character_Attack_Order = 4;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[2].GetComponent<Character_Script>().Copy_Character_Stat(Order[2]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[2].GetComponent<Character_Script>().Debuging_Character();
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[3].GetComponent<Character_Script>().Copy_Character_Stat(Order[3]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[3].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
                case (int)ArrayPhase.FIRST45:
                    my_turn = false;
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
                        Pick = false;
                        if (Ready_Array == true)
                        {
                            Order[4].GetComponent<Character_Script>().character_Attack_Order = 5;
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[4].GetComponent<Character_Script>().Copy_Character_Stat(Order[4]);
                            PassData_.GetComponent<Arrayed_Character_Data>().Pass_Data[4].GetComponent<Character_Script>().Debuging_Character();
                            Debug.Log((ArrayPhase)Phase + "��ġ �Ϸ�");
                            PassData_.GetComponent<Arrayed_Character_Data>().initial_Ready();
                            Ready_Array = false;
                        }
                    }
                    break;
            }
        }
    }
    IEnumerator Time_Out()
    {
        bool Inventory_ID_Time_Out = true;
        while(Inventory_ID_Time_Out)
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

        Grids[random].GetComponent<Character_Script>().character_ID = Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().m_Character_ID;
        Grids[random].GetComponent<Character_Script>().Character_Setting(Inventory[Time_Out_Inventory].GetComponent<Inventory_ID>().m_Character_ID);
        Grids[random].GetComponent<Character_Script>().character_Num_Of_Grid = random + 1;
        Grids[random].GetComponent<Character_Script>().Debuging_Character();
        Grids[random].tag = "Character";
        Order.Add(Grids[random]);

        yield return null;
    }
    public void BanPick_Ready()
    {
        Ready_Array = true;
    }
    public void Get_Button(int num)// �κ��丮 Ŭ���� �߻�
    {
        if (my_turn == true && Pick == true)
            Character_instance = true;
        Prefeb_Character.GetComponent<Character_Script>().Character_Setting(num);
    }
}