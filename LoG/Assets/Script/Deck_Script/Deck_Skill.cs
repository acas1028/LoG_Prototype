using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck_Skill : MonoBehaviour
{
    public enum Skill
    {
        Attack_Confidence = 1,      // 자신감
        Attack_Executioner,         // 처형자
        Attack_Struggle,            // 발악
        Attack_Ranger,              // 명사수
        Attack_ArmorPiercer,        // 철갑탄
        Attack_DivineShield,        // 천상의보호막
     // Attack_Sturdy,              // 옹골참
        Balance_Blessing,           // 축복
        Balance_GBGH,               // 모아니면 도
        Balance_Smoke,              // 연막탄
        Balance_Survivor,           // 생존자
        Balance_Curse,              // 저주
        Balance_WideCounter,        // 광역반격
        Balance_DestinyBond,        // 길동무 
        Defense_Disarm,             // 무장해제
    };

    public Skill Character_Skill;
    private bool is_selected = false;

    public void On_Skill_Button()
    {
        Deck_Manager cs = Deck_Manager.instance;
        if (cs.Current_Character.GetComponent<Character>().character_ID == 0)
        {
            if (this.gameObject.GetComponent<Deck_Skill>().is_selected == true)
                return;
            else
            {
                this.gameObject.GetComponent<Deck_Skill>().is_selected = true;
                cs.Skill_Button.Add(this.gameObject);
                Button SKill = this.gameObject.GetComponent<Button>();
                ColorBlock CB = SKill.colors;
                Color Red_Grid = Color.red;
                CB.normalColor = Red_Grid;
                CB.pressedColor = Red_Grid;
                CB.selectedColor = Red_Grid;
                SKill.colors = CB;

                Character D_Character = Deck_Manager.instance.Current_Character.GetComponent<Character>();
                switch (Character_Skill)
                {
                    case Skill.Attack_Confidence:
                        D_Character.Character_Setting(1);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Attack_Executioner:
                        D_Character.Character_Setting(2);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Attack_Struggle:
                        D_Character.Character_Setting(3);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Attack_Ranger:
                        D_Character.Character_Setting(4);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Attack_ArmorPiercer:
                        D_Character.Character_Setting(5);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Attack_DivineShield:
                        D_Character.Character_Setting(6);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_Blessing:
                        D_Character.Character_Setting(7);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_GBGH:
                        D_Character.Character_Setting(8);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_Smoke:
                        D_Character.Character_Setting(9);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_Survivor:
                        D_Character.Character_Setting(10);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_Curse:
                        D_Character.Character_Setting(11);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_WideCounter:
                        D_Character.Character_Setting(12);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Balance_DestinyBond:
                        D_Character.Character_Setting(13);
                        D_Character.Debuging_Character();
                        break;
                    case Skill.Defense_Disarm:
                        D_Character.Character_Setting(14);
                        D_Character.Debuging_Character();
                        break;
                }
            }
        }
    }

    public void Cancle_Skill(int num)
    {
        Deck_Manager cs = Deck_Manager.instance;
        for (int i=0;i< cs.Skill_Button.Count;i++)
        {
            if ((int)cs.Skill_Button[i].GetComponent<Deck_Skill>().Character_Skill==num)
            {
                cs.Skill_Button[i].GetComponent<Deck_Skill>().is_selected = false;
                Button SKill = cs.Skill_Button[i].GetComponent<Button>();
                ColorBlock CB = SKill.colors;
                Color white = Color.white;
                CB.normalColor = white;
                CB.pressedColor = white;
                CB.selectedColor = white;
                SKill.colors = CB;

            }
        }
    }
}
