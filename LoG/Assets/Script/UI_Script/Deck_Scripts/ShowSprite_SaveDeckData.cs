using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSprite_SaveDeckData : MonoBehaviour
{
    public GameObject DeckSave;
    public GameObject TypeSlot;
    public GameObject PropertySlot;
    public GameObject Character_Sprite;
    public GameObject SetCharacter;
    public Character Save_Character;
    public GameObject Page;
    public int deck_Character_Count;
    public int Page_Num;

    bool IsDeckSaveexist;
    

    void Start()
    {
        DeckSave = GameObject.FindGameObjectWithTag("Deck_Save_Character");
        Page_Num = 1; // ó���� 1�� ����
        Page = DeckSave.transform.GetChild(Page_Num - 1).gameObject;
        IsDeckSaveexist = false;

        isDeckSaveExist();
        Find_Save_Character();

        Show_Type_Sprite();
        Show_Property_Sprite();
        Show_Character_Sprite();
    }

    private void Update()
    {
        /*Change_property_image_in_Wrong()*/;   
    }

    void isDeckSaveExist()
    {
        if (DeckSave == null)
            return;

        IsDeckSaveexist = true;
    }

    void Find_Save_Character()
    {
        if (IsDeckSaveexist == false)
            return;

        Save_Character = Page.transform.GetChild(deck_Character_Count).gameObject.GetComponent<Character>();
    }

    public void Show_Type_Sprite()
    {
        if (IsDeckSaveexist == false)
            return;

        TypeSlot.SetActive(true);

        switch(Save_Character.character_Type)
        {
            case CharacterStats.CharacterType.Attacker:
                TypeSlot.GetComponent<Image>().sprite = TypeSlot.GetComponent<Deck_Type_Slot>().Type_Image[1];
                break;

            case CharacterStats.CharacterType.Balance:
                TypeSlot.GetComponent<Image>().sprite = TypeSlot.GetComponent<Deck_Type_Slot>().Type_Image[2];
                break;

            case CharacterStats.CharacterType.Defender:
                TypeSlot.GetComponent<Image>().sprite = TypeSlot.GetComponent<Deck_Type_Slot>().Type_Image[3];
                break;
        }

    }

    void Show_Property_Sprite()
    {
        if (IsDeckSaveexist == false)
            return;

        PropertySlot.GetComponent<Property_Slot>().Change_property(Save_Character.character_Skill.ToString());

    }

   

    //void Change_property_image_in_Wrong()
    //{
    //    if (Save_Character.character_Skill.ToString() == PropertySlot.GetComponent<Property_Slot>().property_Name)
    //        return;

    //    PropertySlot.GetComponent<Property_Slot>().Change_property(Save_Character.character_Skill.ToString());
    //}

    void Show_Character_Sprite()
    {
        if (IsDeckSaveexist == false)
            return;

        SetCharacter.SetActive(false);
        Character_Sprite.SetActive(true);
    }
}
