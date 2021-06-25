using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStats : MonoBehaviour
{
    public GameObject prefabCharacter;
    public GameObject[] attack_Grid_Tile;
    public GameObject Pop_up_Character_image;
  
    public Text attack_Damage;
    public Text health_Point;

    private GameObject sprite_Data;
    
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
        
    }

    private void Start()
    {
        sprite_Data = GameObject.FindGameObjectWithTag("Sprite_Data");
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
                attack_Grid_Tile[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                attack_Grid_Tile[i].GetComponent<Image>().color = Color.white;
            }
        }


        Pop_up_Character_image.GetComponent<Image>().sprite = sprite_Data.GetComponent<Sprite_Data>().Character_Sprite[num - 1];
        Pop_up_Character_image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite_Data.GetComponent<Sprite_Data>().Character_Sprite[num - 1].rect.height);
        Pop_up_Character_image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite_Data.GetComponent<Sprite_Data>().Character_Sprite[num - 1].rect.width);
        
    }

    
}
