using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;


public class Character_arrayment_showing : MonoBehaviourPunCallbacks
{
    public GameObject[] Mine_Grid_Color;
    public GameObject[] Opponent_Grid_Color;
    public List<GameObject> Character_List = new List<GameObject>();
    public GameObject cancel_Character;
    public bool[] temp; //cancel_character�� ���� ���� bool�� ���� ��������
    public Sprite red_Sprite;
    public Sprite blue_Sprite;
    public int my_Count;
    public int Oppenent_Count;
    public bool is_Mine;


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
        my_Count = 0;
        Oppenent_Count = 0;
        is_Sprite_Change = false;
    }

    private void Update()
    {

    }

    [PunRPC]
    void Showing_Character_arrayment()
    {
        if (Character_List.Count == 0)
            return;
        if (is_Sprite_Change == false)
            return;
        if(is_Mine==true)
        {
            if (Character_List.Count == 1)
            {
                for (int i = 0; i < Character_List[0].GetComponent<Character>().character_Attack_Range.Length; i++)
                {
                    if (Character_List[0].GetComponent<Character>().character_Attack_Range[i] == true)
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                    }
                }

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
            }
  
        }

        else
        {
            Character_List[0].GetComponent<Character>().character_Attack_Range = Enemy_AttackRange_Change(Character_List[0].GetComponent<Character>());
            if (Character_List.Count == 1)
            {
                for (int i = 0; i < Character_List[0].GetComponent<Character>().character_Attack_Range.Length; i++)
                {
                    if (Character_List[0].GetComponent<Character>().character_Attack_Range[i] == true)
                    {
                        Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                    }
                }
                
            }
            else if (Character_List.Count == 2)
            {
                for (int i = 0; i < Character_List[1].GetComponent<Character>().character_Attack_Range.Length; i++)
                {
                    if (Character_List[1].GetComponent<Character>().character_Attack_Range[i] == true)
                    {
                        if (Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite == red_Sprite)
                        {
                            Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = blue_Sprite;
                        }
                        else
                        {
                            Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                        }
                    }
                }
                
            }

        }

        is_Sprite_Change = false;
    }

    [PunRPC]
    void Arrayment_cancle_inGrid()
    {
        if (Character_List.Count == 0)
            return;


        if (is_Mine == true)
        {
            for (int i = 0; i < Opponent_Grid_Color.Length; i++)
            {
                if (Opponent_Grid_Color[i] == true)
                {
                    if (Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite == blue_Sprite)
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                    }
                    else
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = null;
                    }

                }
            }
        }

        else
        {
            for (int i = 0; i < Mine_Grid_Color.Length; i++)
            {
                if (Mine_Grid_Color[i] == true)
                {
                    if (Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite == blue_Sprite)
                    {
                        Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = red_Sprite;
                    }
                    else
                    {
                        Mine_Grid_Color[i].GetComponent<SpriteRenderer>().sprite = null;
                    }

                }
            }
        }

        Character_List.Remove(cancel_Character);

        cancel_Character = null;
    }

    public void Set_AttackRange_Ui(bool Is_activate)
    {
        if (Is_activate)
        {
            photonView.RPC("Showing_Character_arrayment", RpcTarget.All);
        }
            
        else
        {
            photonView.RPC("Arrayment_cancle_inGrid", RpcTarget.All);
        }
            
    }

    public int Reverse_Enemy(int num) 
    {
        int dummy = 0;
        switch (num)
        {
            case 1:
                dummy = 3;
                break;
            case 2:
                dummy = 2;
                break;
            case 3:
                dummy = 1;
                break;
            case 4:
                dummy = 6;
                break;
            case 5:
                dummy = 5;
                break;
            case 6:
                dummy = 4;
                break;
            case 7:
                dummy = 9;
                break;
            case 8:
                dummy = 8;
                break;
            case 9:
                dummy = 7;
                break;
        }
        return dummy;
    }

    bool[] Enemy_AttackRange_Change(Character Team2CS)
    {
        bool[] dummy = new bool[9];

        for (int i = 0; i < 9; i++)
        {
            dummy[i] = false;
            dummy[i] = Team2CS.character_Attack_Range[Reverse_Enemy(i + 1) - 1];
        }
        return dummy;
    }

    void Character_showing_arrayment_Reset()// �� ���������� �ʱ�ȭ ��Ű�� �Լ�
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

