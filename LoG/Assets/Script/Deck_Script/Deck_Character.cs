using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck_Character : MonoBehaviour
{
    Deck_Manager deckManager;
    public bool Set_Active_Character = false;
    public CharacterSpriteManager characterSprites;

    private void Awake()
    {
        deckManager = FindObjectOfType<Deck_Manager>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < deckManager.Set_Character_.Length; i++)
        {
            if(this.gameObject == deckManager.Set_Character_[i])
            {
                deckManager.Set_Character_[i].SetActive(false);
                return;
            }
        }
        Set_Active_Character = true;

        deckManager.Current_Character = this.gameObject;

    }

    public void Check_Stat()
    {
        deckManager.Current_Character = this.gameObject;
        int num = (int)this.gameObject.GetComponentInChildren<Character>().character_Skill;
        deckManager.Pre_Skill = deckManager.Skill_Button[num];
    }

    //public void Change_Type(int num)
    //{
    //    GetComponent<Image>().sprite = Type_Image[num];
    //    characterType = (CharacterType)num;
    //}

    public void Change_Character_Skin(int num)
    {
        GetComponent<Image>().sprite = characterSprites.characterSprites[num];
    }
}
