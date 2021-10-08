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
    private GameObject Type_Obj;

    public void On_Skill_Button()
    {
        Deck_Manager cs = Deck_Manager.instance;
        Character D_Character = Deck_Manager.instance.Current_Character.GetComponentInChildren<Character>();
        bool overlap = false;

        for(int i=0;i<cs.Skill_List.Count;i++)
        {
            if(cs.Skill_List[i]==this.gameObject)
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
                cs.Reset_Grid();
            }
            cs.Pre_Skill = this.gameObject;
            cs.Skill_List.Add(this.gameObject);
            Button SKill = this.gameObject.GetComponent<Button>();
            ColorBlock CB = SKill.colors;
            Color yellow_Grid = Color.yellow;
            CB.normalColor = yellow_Grid;
            CB.pressedColor = yellow_Grid;
            CB.selectedColor = yellow_Grid;
            SKill.colors = CB;

            switch (Character_Skill)
            {
                case CharacterSkill.Attack_Confidence:
                    D_Character.Character_Setting(1);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_Executioner:
                    D_Character.Character_Setting(2);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_Struggle:
                    D_Character.Character_Setting(3);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_Ranger:
                    D_Character.Character_Setting(4);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_ArmorPiercer:
                    D_Character.Character_Setting(5);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_DivineShield:
                    D_Character.Character_Setting(6);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Attack_Sturdy:
                    D_Character.Character_Setting(7);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Blessing:
                    D_Character.Character_Setting(8);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_GBGH:
                    D_Character.Character_Setting(9);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Smoke:
                    D_Character.Character_Setting(10);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Survivor:
                    D_Character.Character_Setting(11);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Curse:
                    D_Character.Character_Setting(12);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_WideCounter:
                    D_Character.Character_Setting(13);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_DestinyBond:
                    D_Character.Character_Setting(14);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Disarm:
                    D_Character.Character_Setting(15);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Coward:
                    D_Character.Character_Setting(16);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Patience:
                    D_Character.Character_Setting(17);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Responsibility:
                    D_Character.Character_Setting(18);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Barrier:
                    D_Character.Character_Setting(19);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Encourage:
                    D_Character.Character_Setting(20);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Thronmail:
                    D_Character.Character_Setting(21);
                    D_Character.Debuging_Character();
                    break;
            }

            Sync_Type();

        }
    }
    public void Cancle_Skill()
    {
        Deck_Manager cs = Deck_Manager.instance;
        cs.Pre_Skill.GetComponent<Deck_Skill>().is_selected = false;
        cs.Skill_List.Remove(cs.Pre_Skill);
        Button B_SKill = cs.Pre_Skill.GetComponent<Button>();
        ColorBlock CB = B_SKill.colors;
        Color white = Color.white;
        CB.normalColor = white;
        CB.pressedColor = white;
        CB.selectedColor = white;
        B_SKill.colors = CB;
    }
    private void Sync_Type()
    {
        Deck_Manager cs = Deck_Manager.instance;

        int j = 0;
        while (j < 7)
        {
            if (cs.Current_Character == cs.Character_Slot[j])
            {
                cs.Slot_Type[j].GetComponent<Deck_Type_Slot>().Change_Type((int)Type_Obj.GetComponent<Type_Object>().Type);
                cs.Character_Slot[j].GetComponent<CharacterSpriteforType>().ChangeSprite((int)Type_Obj.GetComponent<Type_Object>().Type);
                break;
            }
            j++;
        }
        
    }

}
