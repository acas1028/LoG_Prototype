using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStats : MonoBehaviour
{
    public GameObject prefabCharacter;// 팝업창에 띄울 캐릭터
    public GameObject[] attack_Grid_Tile; //팝업창에 띄울 공격 범위

    public GameObject stageData; //pve 스테이지 데이터


    public Text attack_Damage;// 팝업창에 띄울 캐릭터 공격력
    public Text health_Point;// 팝업창에 띄울 캐릭터 hp
    public Text property_text; //팝업창에 띄울 캐릭터 특성

    public Sprite EmptyGrid;
    public Sprite NotEmptyGrid;

    public bool is_Pve;

    int lastPageNum;

    

    private void Start()
    {
        lastPageNum = Deck_Data_Send.instance.lastPageNum;
        prefabCharacter.GetComponent<Character>().Character_Reset();
        
    }
    


    public void Character_Showing_Stats(int inventory_Num) //캐릭터 스탯을 팝업창에 띄우기 위한 함수
    {
        
        if(is_Pve==true)
        {
            StageDataController stageDataController = stageData.GetComponent<StageDataController>();

            prefabCharacter.GetComponent<Character>().PVE_Player_Character_Setting(inventory_Num - 1 + 5, stageDataController.StageName);
        }

        else
        {
            prefabCharacter.GetComponent<Character>().Copy_Character_Stat(Deck_Data_Send.instance.Save_Data[lastPageNum, inventory_Num - 1]);
        }
        
        attack_Damage.text = prefabCharacter.GetComponent<Character>().character_Attack_Damage.ToString();
        health_Point.text = prefabCharacter.GetComponent<Character>().character_HP.ToString();
        property_text.text = this.GetComponent<Property_Name_To_Korean>().PropertyNameTOKorean(prefabCharacter.GetComponent<Character>().character_Skill.ToString());

        for(int i=0; i<prefabCharacter.GetComponent<Character>().character_Attack_Range.Length; i++)
        {
            if(prefabCharacter.GetComponent<Character>().character_Attack_Range[i]== true)
            {
                attack_Grid_Tile[i].GetComponent<Image>().sprite = NotEmptyGrid;
            }
            else
            {
                attack_Grid_Tile[i].GetComponent<Image>().sprite = EmptyGrid;
            }
        }

    }

    

    
}
