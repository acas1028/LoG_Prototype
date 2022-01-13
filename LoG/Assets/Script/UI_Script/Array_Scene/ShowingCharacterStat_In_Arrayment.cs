using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStat_In_Arrayment : MonoBehaviour
{
    GameObject grid;

    public GameObject[] attack_Grid_Tile; //ÆË¾÷Ã¢¿¡ ¶ç¿ï °ø°Ý ¹üÀ§
    public Text attack_Damage;// ÆË¾÷Ã¢¿¡ ¶ç¿ï Ä³¸¯ÅÍ °ø°Ý·Â
    public Text health_Point;// ÆË¾÷Ã¢¿¡ ¶ç¿ï Ä³¸¯ÅÍ hp
    public Text property;

    public Sprite EmptyGrid;
    public Sprite NotEmptyGrid;

    public void ShowingStatInarray()
    {
        grid = this.transform.parent.transform.parent.gameObject.GetComponent<Arrayment_Popup_Script>().Grid;
        GameObject Character = grid.transform.Find("Character_Prefab").gameObject;

        attack_Damage.text = Character.GetComponent<Character>().character_Attack_Damage.ToString();
        health_Point.text = Character.GetComponent<Character>().character_HP.ToString();
        property.text = this.GetComponent<Property_Name_To_Korean>().PropertyNameTOKorean(Character.GetComponent<Character>().character_Skill.ToString());

        for (int i = 0; i < Character.GetComponent<Character>().character_Attack_Range.Length; i++)
        {
            if (Character.GetComponent<Character>().character_Attack_Range[i] == true)
            {
                attack_Grid_Tile[i].GetComponent<Image>().sprite = NotEmptyGrid;
            }
            else
            {
                attack_Grid_Tile[i].GetComponent<Image>().sprite =EmptyGrid;
            }
        }

    }
}
