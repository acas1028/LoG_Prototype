using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

public class Deck_Skill : MonoBehaviour
{
    public CharacterSkill Character_Skill;
    public bool is_selected = false;

    public void On_Skill_Button()
    {
        Deck_Manager cs = Deck_Manager.instance;
        Character D_Character = Deck_Manager.instance.Current_Character.GetComponentInChildren<Character>();

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
            this.gameObject.GetComponent<Deck_Skill>().is_selected = true;
            cs.Skill_List.Add(this.gameObject);
            Button SKill = this.gameObject.GetComponent<Button>();
            ColorBlock CB = SKill.colors;
            Color Red_Grid = Color.red;
            CB.normalColor = Red_Grid;
            CB.pressedColor = Red_Grid;
            CB.selectedColor = Red_Grid;
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
                case CharacterSkill.Balance_Blessing:
                    D_Character.Character_Setting(7);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_GBGH:
                    D_Character.Character_Setting(8);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Smoke:
                    D_Character.Character_Setting(9);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Survivor:
                    D_Character.Character_Setting(10);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_Curse:
                    D_Character.Character_Setting(11);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_WideCounter:
                    D_Character.Character_Setting(12);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Balance_DestinyBond:
                    D_Character.Character_Setting(13);
                    D_Character.Debuging_Character();
                    break;
                case CharacterSkill.Defense_Disarm:
                    D_Character.Character_Setting(14);
                    D_Character.Debuging_Character();
                    break;
            }
        }
        
    }
    public void Cancle_Skill()
    {
        Debug.Log("Äµ½½");
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
        
}
