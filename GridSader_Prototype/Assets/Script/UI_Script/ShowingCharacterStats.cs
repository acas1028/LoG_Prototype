using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStats : MonoBehaviour
{
    public GameObject prefabCharacter;
    public GameObject[] attack_Grid_Tile;

    public Text attack_Damage;
    public Text health_Point;

    private static ShowingCharacterStats PopUp_Manager;
    public static ShowingCharacterStats PopUp_instance
    {
        get
        {
            if (!PopUp_Manager)
            {
                PopUp_Manager = FindObjectOfType(typeof(ShowingCharacterStats)) as ShowingCharacterStats;

                if (PopUp_Manager == null)
                    Debug.Log("no Singleton obj");
            }
            return PopUp_Manager;
        }
    }

    public void Awake()
    {
        attack_Grid_Tile = GameObject.FindGameObjectsWithTag("Attack_Grid_Tile");
        for(int i=0; i<attack_Grid_Tile.Length; i++)
        {
            attack_Grid_Tile[i].SetActive(false);
        }
    }


    public void Character_Showing_Stats(int num) //캐릭터 스탯을 팝업창에 띄우기 위한 함수
    {
        prefabCharacter.GetComponent<Character_Script>().Character_Setting(num);
        attack_Damage.text = prefabCharacter.GetComponent<Character_Script>().character_Attack_Damage.ToString();
        health_Point.text = prefabCharacter.GetComponent<Character_Script>().character_HP.ToString();

        for(int i=0; i<prefabCharacter.GetComponent<Character_Script>().character_Attack_Range.Length; i++)
        {
            if(prefabCharacter.GetComponent<Character_Script>().character_Attack_Range[i]== true)
            {
                attack_Grid_Tile[i].SetActive(true);
            }
            else
            {
                attack_Grid_Tile[i].SetActive(false);
            }
        }
         

        
    }

    
}
