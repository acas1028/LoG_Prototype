using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;


public class Character_arrayment_showing : MonoBehaviourPunCallbacks
{
    public GameObject[] Opponent_Grid_Color;
    public List<GameObject> Character_List = new List<GameObject>();
    public GameObject cancel_Character;
    public bool[] temp; //cancel_character의 공격 범위 bool로 전부 가져오기
    public Sprite red_Sprite;
    public Sprite blue_Sprite;

    public bool is_Sprite_Change;

    private static Character_arrayment_showing Showing_arrayment_manager;
    public static Character_arrayment_showing Showing_arrayment_instance
    {
        get
        {
            if(!Showing_arrayment_manager)
            {
                Showing_arrayment_manager = FindObjectOfType(typeof(Character_arrayment_showing)) as Character_arrayment_showing;

                if (Showing_arrayment_manager == null)
                    Debug.Log("no Singleton obj");
            }
            return Showing_arrayment_manager;
        }
    }


    private void Awake()
    {
        if (Showing_arrayment_manager == null)
        {
            Showing_arrayment_manager = this;
        }
        else if(!Showing_arrayment_manager != this)
        {
            Destroy(gameObject);
        }    
    }




    private void Start()
    {
        is_Sprite_Change = false;
    }

    private void Update()
    {
        Showing_Character_arrayment();
        Arrayment_cancle_inGrid();


    }

    void Showing_Character_arrayment()
    {
        if (Character_List.Count == 0)
            return;
        if (is_Sprite_Change == false)
            return;

        if (Character_List.Count == 1)
        {
            for (int i = 0; i < Character_List[0].GetComponent<Character>().character_Attack_Range.Length; i++)
            {
                if (Character_List[0].GetComponent<Character>().character_Attack_Range[i] == true)
                {
                    Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                }
            }
            is_Sprite_Change = false;
        }

        else if (Character_List.Count == 2)
        {
            for (int i = 0; i < Character_List[1].GetComponent<Character>().character_Attack_Range.Length; i++)
            {
                if (Character_List[1].GetComponent<Character>().character_Attack_Range[i] == true)
                {
                    if (Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite == red_Sprite)
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = blue_Sprite;
                    }

                    else
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                    }
                }

            }
            is_Sprite_Change = false;
        }


    }

    void Arrayment_cancle_inGrid()
    {
        if (Character_List.Count == 0)
            return;

        if (temp == null)
            return;

        for(int i=0; i<temp.Length;i++)
        {
            if(temp[i]==true)
            {
                if(Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite ==blue_Sprite)
                {
                    Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                }
                else
                {
                    Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = null;
                }
                
            }
        }

        Character_List.Remove(cancel_Character);

        temp = null;
        cancel_Character = null;
    }

    public void Set_AttackRange_Ui(bool Is_activate)
    {
        if (Is_activate)
        {
            photonView.RPC("Showing_Character_arrayment", RpcTarget.All);
            is_Sprite_Change = true;
        }
            
        else
        {
            photonView.RPC("Arrayment_cancle_inGrid", RpcTarget.All);
        }
            
    }

    void Character_showing_arrayment_Reset()// 판 지날때마다 초기화 시키는 함수
    {
        for(int i=0; i<Opponent_Grid_Color.Length;i++)
        {
            Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = null;
        }
        Character_List.Clear();

        temp = null;
        cancel_Character = null;
    }
}

