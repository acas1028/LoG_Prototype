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
    GameObject Character_Instantiate;
    [Tooltip("프리펩된 캐릭터")]
    public GameObject Prefeb_Character;
    public Queue<GameObject> Order = new Queue<GameObject>();
    private GameObject[] Grids;
    private GameObject[] Inventory;
    public GameObject[] Block_Inventory;
    public GameObject PopUp_UI;
    public GameObject Array_Cancle_Button;
    public GameObject PopUp_Manager;
    private GameObject Cancle_Character;
    private int Count = 0;

    public Text debugText;

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

            DontDestroyOnLoad(this.gameObject);
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (!ArrayManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Grids = GameObject.FindGameObjectsWithTag("Null_Character");
        Inventory = GameObject.FindGameObjectsWithTag("Character_inventory_Button");
    }
    void Update()
    {
        Arrayment_Raycast();
    }
    public void Arrayment_Raycast()
    {

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

                if (hit.transform.CompareTag("Character")&&Character_instance==false)
                {
                    PopUp_Manager.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(hit.collider.gameObject.GetComponent<Character_Script>().character_ID);
                    Cancle_Character = hit.transform.gameObject;
                    Array_Cancle_Button.SetActive(true);
                    PopUp_UI.SetActive(true);                   
                }
                if (hit.transform.CompareTag("Null_Character")&&Character_instance == true)//Null_Character로 태그 되어 있는 물체에게 raycast가 닿으면.
                {
                    Character_Instantiate = hit.collider.gameObject;
                    Character_Instantiate.tag = "Character";//Character로 태그를 변경한다. 예외처리.
                    Count++;
                    Character_Instantiate.GetComponent<Character_Script>().character_ID = Prefeb_Character.GetComponent<Character_Script>().character_ID;
                    Character_Instantiate.GetComponent<Character_Script>().Character_Setting(Character_Instantiate.GetComponent<Character_Script>().character_ID);
                    for (int i = 0; i < Grids.Length; i++)//캐릭터에 Grid_Number 삽입
                    {
                        if (Character_Instantiate == Grids[i])
                        {
                            Character_Instantiate.GetComponent<Character_Script>().character_Num_Of_Grid = i + 1;
                            debugText.text += ((i + 1) + ", ");
                        }
                    }
                    for (int i = 0; i < Inventory.Length; i++)//인벤토리 개별화
                    {
                    if (Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Character_Instantiate.GetComponent<Character_Script>().character_ID) //ID가 변동 되므로 수정 해야함.
                    {

                        Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = true;
                    }

                    }
                    Character_Instantiate.GetComponent<Character_Script>().Debuging_Character();
                    Order.Enqueue(Character_Instantiate);
                    PopUp_UI.SetActive(false);
                    Character_instance = false;
                }


            }

    }
    public void Cancle_Array()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if(Inventory[i].GetComponent<Inventory_ID>().m_Inventory_ID == Cancle_Character.GetComponent<Character_Script>().character_ID)
            {
                Inventory[i].GetComponent<Inventory_ID>().is_Arrayed = false;
            }
        }
        Cancle_Character.tag = "Null_Character";
        //Order.Enqueue(Cancle_Character);
        Cancle_Character.GetComponent<Character_Script>().character_ID = 0;
        Cancle_Character.GetComponent<Character_Script>().Character_Setting(Cancle_Character.GetComponent<Character_Script>().character_ID);
        Cancle_Character.GetComponent<Character_Script>().Debuging_Character();

    }
    public void Attack_Order()
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject TestOrder = Order.Dequeue();
            TestOrder.GetComponent<Character_Script>().character_Attack_Order = i + 1;
            TestOrder.GetComponent<Character_Script>().Debuging_Character();
        }
        Ready_Array = true;
    }
    public void Get_Button(int num)// 인벤토리 클릭시 발생
    {
        Character_instance = true;
        Prefeb_Character.GetComponent<Character_Script>().Character_Setting(num);
    }
}