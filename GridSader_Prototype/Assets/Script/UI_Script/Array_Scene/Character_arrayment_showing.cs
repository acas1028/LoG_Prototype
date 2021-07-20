using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_arrayment_showing : MonoBehaviour
{
    public GameObject[] Opponent_Grid_Color;
    public GameObject[] grid_Character;
    public List<GameObject> Character_List = new List<GameObject>();
    public Color red_color;
    public Color blue_color;



    private void Start()
    {
        grid_Character = GameObject.FindGameObjectsWithTag("Null_Character");
    }

    private void Update()
    {
        Find_Array_character();
        Showing_Character_arrayment();


    }

    void Find_Array_character()
    {
        for (int i = 0; i < grid_Character.Length; i++)
        {
            if (grid_Character[i].tag == "Character")
            {

                if (Character_List.Contains(grid_Character[i]))
                {
                    return;
               }
                Character_List.Add(grid_Character[i]);
            }
        }
    }

    void Showing_Character_arrayment()
    {
        if (Character_List.Count == 0)
            return;

        if (Character_List.Count == 1)
        {
            for (int i = 0; i < Character_List[0].GetComponent<Character_Script>().character_Attack_Range.Length; i++)
            {
                if (Character_List[0].GetComponent<Character_Script>().character_Attack_Range[i] == true)
                {
                    Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().color = red_color;
                }
            }
        }

        else if (Character_List.Count == 2)
        {
            for (int i = 0; i < Character_List[1].GetComponent<Character_Script>().character_Attack_Range.Length; i++)
            {
                if (Character_List[1].GetComponent<Character_Script>().character_Attack_Range[i] == true)
                {
                    if (Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().color == red_color)
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().color = blue_color;
                    }

                    else
                    {
                        Opponent_Grid_Color[i].GetComponent<SpriteRenderer>().color = red_color;
                    }
                }

            }
        }


    }
}

