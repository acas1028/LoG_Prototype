using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingCharacterStats : MonoBehaviour
{
    public GameObject prefabCharacter;// �˾�â�� ��� ĳ����
    public GameObject[] attack_Grid_Tile; //�˾�â�� ��� ���� ����
    
  
    public Text attack_Damage;// �˾�â�� ��� ĳ���� ���ݷ�
    public Text health_Point;// �˾�â�� ��� ĳ���� hp
    
    //private static ShowingCharacterStats PopUp_Manager;
    //public static ShowingCharacterStats PopUp_instance
    //{
    //    get
    //    {
    //        if (!PopUp_Manager)
    //        {
    //            PopUp_Manager = FindObjectOfType(typeof(ShowingCharacterStats)) as ShowingCharacterStats;

    //            if (PopUp_Manager == null)
    //                Debug.Log("no Singleton obj");
    //        }
    //        return PopUp_Manager;
    //    }
    //}

    public void Awake()
    {
  
    }

    private void Start()
    {
    }

    public void Character_Showing_Stats(int num) //ĳ���� ������ �˾�â�� ���� ���� �Լ�
    {
        prefabCharacter.GetComponent<Character>().Character_Setting(num);
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
