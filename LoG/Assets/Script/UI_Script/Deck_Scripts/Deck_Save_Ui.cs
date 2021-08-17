using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Deck_Save_Ui : MonoBehaviour
{
    public GameObject[] save_Character;

    public GameObject[] character;

    public GameObject[] character_Type;

    public Sprite[] type_Images;

    bool is_Character_exist;
    bool is_character_Type_exist;

    

    private void Start()
    {
        is_Character_exist = false;
        is_character_Type_exist = false;
    }

    void Character_Exist()
    {
        if (is_Character_exist==true)
            return;
        
        for(int i=0; i<save_Character.Length;i++)
        {
            if(save_Character[i].GetComponent<Character>().GetType()!=null)
            {
                character[i].SetActive(true);
            }
        }

        is_Character_exist = true;
    }

    void Character_Type_Exist()
    {
        for(int i=0; i<save_Character.Length;i++)
        {
            if (save_Character[i].GetComponent<Character>().GetType() != null)
            {
                CharacterType type = save_Character[i].GetComponent<Character>().character_Type;
                if(type== CharacterType.Attacker)
                {
                    character_Type[i].GetComponent<Image>().sprite = type_Images[0];
                }

                else if(type == CharacterType.Balance)
                {
                    character_Type[i].GetComponent<Image>().sprite = type_Images[1];
                }

                else
                {
                    character_Type[i].GetComponent<Image>().sprite = type_Images[2];
                }
            }
        }
    }

    
}
