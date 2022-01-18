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
        //Page_Num = 1; // ó���� 1�� ����
        //Page = Deck_Data_Send.instance.transform.GetChild(Page_Num - 1).gameObject; //������...
        IsDeckSaveexist = false;

        FindPageNum();
        Debug.Log(Page_Num);

        Page = Deck_Data_Send.instance.transform.GetChild(Page_Num - 1).gameObject; 

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
            case CharacterStats.CharacterType.Null:
                TypeSlot.GetComponent<Image>().sprite = null;
                break;

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

        PropertySlot.SetActive(true);
        PropertySlot.GetComponent<Property_Slot>().Change_property(Save_Character.character_Skill.ToString());

        

    }

  

    void Show_Character_Sprite()
    {
        if (IsDeckSaveexist == false)
            return;

        SetCharacter.SetActive(false);
        Character_Sprite.SetActive(true);
    }


    void FindPageNum() // ���� ������ ã��
    {
        if (Page_Num == deckManager.NowPageIdx + 1)
            return;

        Page_Num = deckManager.NowPageIdx + 1;
    }

    public void ResetSpirteData() // ��������Ʈ ���� 
    {
        
        if (TypeSlot == null)
            return;

        if (PropertySlot == null)
            return;

        TypeSlot.SetActive(false);
        PropertySlot.SetActive(false);
        Character_Sprite.SetActive(false);
        SetCharacter.SetActive(true);
      

    }

    public void CharacterSpriteReset() //ĳ���� ���빰�� ���� ������ ĳ���� ��������Ʈ�� off�ǰ� ��.
    {
        if (TypeSlot.GetComponent<Image>().sprite != null)
            return;
        if (PropertySlot.GetComponent<Image>().sprite != null)
            return;

        Character_Sprite.SetActive(false);
        SetCharacter.SetActive(true);

    }

    public void SetSprite() // ��������Ʈ ����
    {
        FindPageNum();
        Page = Deck_Data_Send.instance.transform.GetChild(Page_Num - 1).gameObject;



        Find_Save_Character();

        Show_Type_Sprite();
        Show_Property_Sprite();
        Show_Character_Sprite();

        CharacterSpriteReset();
    }
}
