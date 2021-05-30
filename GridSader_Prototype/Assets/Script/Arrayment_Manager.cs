using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrayment_Manager: MonoBehaviour
{
    private static Arrayment_Manager ArrayManager;
    private bool Character_instance = true;
    GameObject Character_Instantiate;
    [Tooltip("프리펩된 캐릭터")]
    public GameObject Prefeb_Character;
    public Queue<GameObject> Order = new Queue<GameObject>();
    private GameObject[] Grids;
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
        Grids = GameObject.FindGameObjectsWithTag("Null_Character");
    }
    void Update()
    {
        if(Character_instance==true)
        {
            Arrayment_Raycast();
        }

    }
    public void Arrayment_Raycast()
    {

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);

                if (hit.transform.CompareTag("Null_Character"))//Null_Character로 태그 되어 있는 물체에게 raycast가 닿으면.
                {

                Character_Instantiate = hit.collider.gameObject;
                Character_Instantiate.tag = "Character";//Character로 태그를 변경한다. 예외처리.
                Character_Instantiate.GetComponent<Character_Script>().character_ID = Prefeb_Character.GetComponent<Character_Script>().character_ID;
                Character_Instantiate.GetComponent<Character_Script>().Character_Setting(Character_Instantiate.GetComponent<Character_Script>().character_ID);
                for (int i = 0; i < Grids.Length; i++)
                {
                    if (Character_Instantiate == Grids[i])
                    {
                        Character_Instantiate.GetComponent<Character_Script>().character_Num_Of_Grid = i + 1;
                    }
                }
                Character_Instantiate.GetComponent<Character_Script>().Debuging_Character();
                Order.Enqueue(Character_Instantiate);
                Character_instance = false;
                }
                else
                {
                Debug.Log("Null Reperence");
                }

            }

    }
    public void Attack_Order()
    {
        for (int i = 0; i < 5; i++)//정상
        {
            GameObject TestOrder = Order.Dequeue();//정상
            TestOrder.GetComponent<Character_Script>().character_Attack_Order = i + 1;//정상
            TestOrder.GetComponent<Character_Script>().Debuging_Character();
        }
    }
    public void Get_Button(int num)
    {
        Character_instance = true;
        Prefeb_Character.GetComponent<Character_Script>().Character_Setting(num);
    }
}