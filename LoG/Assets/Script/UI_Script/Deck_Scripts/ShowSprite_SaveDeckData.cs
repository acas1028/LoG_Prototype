using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSprite_SaveDeckData : MonoBehaviour
{
    public Deck_Manager deckManager;
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
        Page_Num = 1; // 처음은 1로 고정
        Page = Deck_Data_Send.instance.transform.GetChild(Page_Num - 1).gameObject;
        IsDeckSaveexist = false;

        FindPageNum();

        isDeckSaveExist();
        Find_Save_Character();

        Show_Type_Sprite();
        Show_Property_Sprite();
        Show_Character_Sprite();
    }

    void isDeckSaveExist()
    {
        if (Deck_Data_Send.instance == null)
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
        if (TypeSlot == null)
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
        if (PropertySlot == null)
            return;

        //if (Save_Character.character_Skill.ToString() == "Attack_Confidence") //촬영을 위한 임시 코드. 추후 기본 값이 confidence가 되지 않도록 조정해야함. 
        //    return;

        PropertySlot.SetActive(true);
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


    void FindPageNum() // 현재 페이지 찾기
    {
        if (Page_Num == deckManager.NowPageIdx + 1)
            return;

        Page_Num = deckManager.NowPageIdx + 1;
    }

    public void ResetSpirteData() // 스프라이트 리셋 
    {
        
        if (TypeSlot == null)
            return;

        if (PropertySlot == null)
            return;

        Debug.Log("sss");

        TypeSlot.SetActive(false);
        PropertySlot.SetActive(false);
        Character_Sprite.SetActive(false);
        SetCharacter.SetActive(true);
        //TypeSlot = null;
        //PropertySlot = null;

    }

    public void SetSprite() // 스프라이트 적용
    {
        FindPageNum();
        Page = Deck_Data_Send.instance.transform.GetChild(Page_Num - 1).gameObject;



        Find_Save_Character();

        Show_Type_Sprite();
        Show_Property_Sprite();
        Show_Character_Sprite();
    }
}
