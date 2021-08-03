using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_Skill : MonoBehaviour
{
    public enum Skill
    {
        Attack_Confidence = 1,          // 자신감
        Attack_Executioner,         // 처형자
        Attack_Struggle,            // 발악
        Attack_Ranger,              // 명사수
        Attack_ArmorPiercer,        // 철갑탄
        Attack_DivineShield,        // 천상의보호막
        Attack_Sturdy,              // 옹골참
        Balance_Union,              // 결속
        Balance_GBGH,               // 모아니면 도
        Defense_Disarm,             // 무장해제
    };

    public Skill Character_Skill;

    void Start()
    {
        
    }

    public void On_Skill_Button()
    {
        Character D_Character = Deck_Manager.instance.Current_Character.GetComponent<Character>();


        switch (Character_Skill)
        {
            case Skill.Attack_Confidence:
                D_Character.Character_Setting(3);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_Executioner:
                D_Character.Character_Setting(4);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_Struggle:
                D_Character.Character_Setting(6);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_Ranger:
                D_Character.Character_Setting(7);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_ArmorPiercer:
                D_Character.Character_Setting(8);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_DivineShield:
                D_Character.Character_Setting(9);
                D_Character.Debuging_Character();
                break;
            case Skill.Attack_Sturdy:
                D_Character.Character_Setting(10);
                D_Character.Debuging_Character();
                break;
            case Skill.Balance_Union:
                D_Character.Character_Setting(1);
                D_Character.Debuging_Character();
                break;
            case Skill.Balance_GBGH:
                D_Character.Character_Setting(5);
                D_Character.Debuging_Character();
                break;
            case Skill.Defense_Disarm:
                D_Character.Character_Setting(2);
                D_Character.Debuging_Character();
                break;
        }
    }
}
