using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStats : MonoBehaviour
{
    public GameObject prefabCharacter;// �˾�â�� ��� ĳ����
    public GameObject inventory;
    public GameObject[] attack_Grid_Tile; //�˾�â�� ��� ���� ����
    
  
    public Text attack_Damage;// �˾�â�� ��� ĳ���� ���ݷ�
    public Text health_Point;// �˾�â�� ��� ĳ���� hp

    int lastPageNum;

   

    private void Start()
    {
        lastPageNum = Deck_Data_Send.instance.lastPageNum;
    }

    public void Character_Showing_Stats(int num) //ĳ���� ������ �˾�â�� ���� ���� �Լ�
    {
        prefabCharacter.GetComponent<Character>().Copy_Character_Stat(Deck_Data_Send.instance.Save_Data[lastPageNum, inventory.GetComponent<Inventory_ID>().inventory_ID - 1]);
        attack_Damage.text = prefabCharacter.GetComponent<Character>().character_Attack_Damage.ToString();
        health_Point.text = prefabCharacter.GetComponent<Character>().character_HP.ToString();

        for(int i=0; i<prefabCharacter.GetComponent<Character>().character_Attack_Range.Length; i++)
        {
            if(prefabCharacter.GetComponent<Character>().character_Attack_Range[i]== true)
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
