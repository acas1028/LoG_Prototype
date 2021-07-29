using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showing_AttackRange_inGrid : MonoBehaviour
{
    GameObject[] null_Character;
    GameObject Temp_Character;

    int[] Character_Attack_range;

    private void Start()
    {
        null_Character = GameObject.FindGameObjectsWithTag("Null_Character");
        for(int i=0; i<Character_Attack_range.Length;i++)
        {
            Character_Attack_range[i] = 0;
        }
    }

    void Find_array_character()
    {
        for (int i = 0; i < null_Character.Length; i++)
        {
            if (null_Character[i].tag != "Null_Character")
            {
                null_Character[i].GetComponent<Character>().Copy_Character_Stat(Temp_Character);
            }

        }
    }

    void Array_Character_Range()
    {
        if (Temp_Character!=null)
        {
            for (int i = 0; i < Temp_Character.GetComponent<Character>().character_Attack_Range.Length; i++)
            {
                if (Temp_Character.GetComponent<Character>().character_Attack_Range[i] == true)
                {
                    Character_Attack_range[i]++;
                }
            }
        }
    }


}
