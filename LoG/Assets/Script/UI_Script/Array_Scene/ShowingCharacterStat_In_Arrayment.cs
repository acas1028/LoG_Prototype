using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStat_In_Arrayment : MonoBehaviour
{
    GameObject grid;

    public GameObject[] attack_Grid_Tile; //팝업창에 띄울 공격 범위
    public Text attack_Damage;// 팝업창에 띄울 캐릭터 공격력
    public Text health_Point;// 팝업창에 띄울 캐릭터 hp

    public void ShowingStatInarray()
    {
        grid = this.transform.parent.transform.parent.gameObject.GetComponent<Arrayment_Popup_Script>().Grid;
        GameObject Character = grid.transform.Find("Character_Prefab").gameObject;

        attack_Damage.text = Character.GetComponent<Character>().character_Attack_Damage.ToString();
        health_Point.text = Character.GetComponent<Character>().character_HP.ToString();

        for (int i = 0; i < Character.GetComponent<Character>().character_Attack_Range.Length; i++)
        {
            if (Character.GetComponent<Character>().character_Attack_Range[i] == true)
            {
                attack_Grid_Tile[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                attack_Grid_Tile[i].GetComponent<Image>().color = Color.white;
            }
        }

    }
}
