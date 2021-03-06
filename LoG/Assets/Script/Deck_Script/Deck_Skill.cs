using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Deck_Skill : MonoBehaviour
{
    public CharacterSkill Character_Skill;
    public bool is_selected = false;
    [SerializeField]
    private string property_Name;
    [SerializeField]
    private GameObject Type_Obj;

    Deck_Manager deckManager;

    private void Awake()
    {
        deckManager = FindObjectOfType<Deck_Manager>();
    }

    public void On_Skill_Button()
    {
        bool b_isNull = false;
        Character D_Character = deckManager.Current_Character.GetComponentInChildren<Character>();
        bool overlap = false;
        for (int i = 0; i < deckManager.Skill_List.Count; i++)
        {
            if (deckManager.Skill_List[i]==this.gameObject)
            {
                overlap = true;
                return;
            }
        }
        if(overlap)
        {
            return;
        }

        if (this.gameObject.GetComponent<Deck_Skill>().is_selected == true)
        {
            Cancle_Skill();
            return;
        }
        else
        {
            if (D_Character.character_ID != 0)
            {
                D_Character.Character_Reset();
                Cancle_Skill();
                deckManager.Reset_Grid();
            }
            switch (Character_Skill)
            {
                case CharacterSkill.Attack_Confidence:
                    D_Character.Character_Setting(1);
                    break;
                case CharacterSkill.Attack_Executioner:
                    D_Character.Character_Setting(2);
                    break;
                case CharacterSkill.Attack_Struggle:
                    D_Character.Character_Setting(3);
                    break;
                case CharacterSkill.Attack_Ranger:
                    D_Character.Character_Setting(4);
                    break;
                case CharacterSkill.Attack_ArmorPiercer:
                    D_Character.Character_Setting(5);
                    break;
                case CharacterSkill.Attack_DivineShield:
                    D_Character.Character_Setting(6);
                    break;
                case CharacterSkill.Attack_Sturdy:
                    D_Character.Character_Setting(7);
                    break;
                case CharacterSkill.Attack_Null:
                    D_Character.Character_Setting(8);
                    b_isNull = true;
                    break;
                case CharacterSkill.Balance_Blessing:
                    D_Character.Character_Setting(9);
                    break;
                case CharacterSkill.Balance_GBGH:
                    D_Character.Character_Setting(10);
                    break;
                case CharacterSkill.Balance_Smoke:
                    D_Character.Character_Setting(11);
                    break;
                case CharacterSkill.Balance_Survivor:
                    D_Character.Character_Setting(12);
                    break;
                case CharacterSkill.Balance_Curse:
                    D_Character.Character_Setting(13);
                    break;
                case CharacterSkill.Balance_WideCounter:
                    D_Character.Character_Setting(14);
                    break;
                case CharacterSkill.Balance_DestinyBond:
                    D_Character.Character_Setting(15);
                    break;
                case CharacterSkill.Balance_Null:
                    D_Character.Character_Setting(16);
                    b_isNull = true;
                    break;
                case CharacterSkill.Defense_Disarm:
                    D_Character.Character_Setting(17);
                    break;
                case CharacterSkill.Defense_Coward:
                    D_Character.Character_Setting(18);
                    break;
                case CharacterSkill.Defense_Patience:
                    D_Character.Character_Setting(19);
                    break;
                case CharacterSkill.Defense_Responsibility:
                    D_Character.Character_Setting(20);
                    break;
                case CharacterSkill.Defense_Barrier:
                    D_Character.Character_Setting(21);
                    break;
                case CharacterSkill.Defense_Encourage:
                    D_Character.Character_Setting(22);
                    break;
                case CharacterSkill.Defense_Thornmail:
                    D_Character.Character_Setting(23);
                    break;
                case CharacterSkill.Defense_Null:
                    D_Character.Character_Setting(24);
                    b_isNull = true;
                    break;
            }
            if (!b_isNull)
            {
                Debug.Log("?? ???????? ??????");
                deckManager.Pre_Skill = this.gameObject;
                deckManager.Skill_List.Add(this.gameObject);
                Button SKill = this.gameObject.GetComponent<Button>();
                ColorBlock CB = SKill.colors;
                Color yellow_Grid = Color.yellow;
                CB.normalColor = yellow_Grid;
                CB.pressedColor = yellow_Grid;
                CB.selectedColor = yellow_Grid;
                SKill.colors = CB;
                this.gameObject.GetComponent<Deck_Skill>().is_selected = true;
            }
            PropertySendSlot();
            Sync_Type();
        }
    }
    public void Cancle_Skill()
    {
        deckManager.Pre_Skill.GetComponent<Deck_Skill>().is_selected = false;
        deckManager.Skill_List.Remove(deckManager.Pre_Skill);
        Button B_SKill = deckManager.Pre_Skill.GetComponent<Button>();
        ColorBlock CB = B_SKill.colors;
        Color white = Color.white;
        CB.normalColor = white;
        CB.pressedColor = white;
        CB.selectedColor = white;
        B_SKill.colors = CB;
    }

    private void Sync_Type()
    {
        int j = 0;
        while (j < 7)
        {
            if (deckManager.Current_Character == deckManager.Character_Slot[j])
            {
                deckManager.Slot_Type[j].GetComponent<Deck_Type_Slot>().Change_Type((int)Type_Obj.GetComponent<Type_Object>().Type);
                deckManager.Character_Slot[j].GetComponent<Deck_Character>().Change_Character_Skin((int)Type_Obj.GetComponent<Type_Object>().Type);
                //cs.Character_Slot[j].GetComponent<CharacterSpriteforType>().ChangeSprite((int)Type_Obj.GetComponent<Type_Object>().Type);
                break;
            }
            j++;
        }
        
    }

    private void PropertySendSlot()
    {
        int j = 0;

        while (j < 7)
        {
            if (deckManager.Current_Character == deckManager.Character_Slot[j])
            {
                deckManager.Slot_Property[j].GetComponent<Property_Slot>().Change_property(property_Name);
                break;
            }
            j++;
        }
    }
}
