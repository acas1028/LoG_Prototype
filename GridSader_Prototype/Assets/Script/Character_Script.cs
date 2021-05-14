using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Script : MonoBehaviour
{
    public int character_HP { get; set; } // 체력
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // 공격력
    public int character_Num_Of_Attack_Range { get; set; } // 공격 범위 숫자
    public int character_Num_Of_Greed { get; set; } // 그리드 넘버
    public int character_Attack_Speed { get; set; } // 공격 순서
    public bool character_Is_Preemptive { get; set; } // 선공 후공 true = 선공 false = 후공
    public bool[] character_Attack_Range { get; set; } // 공격 범위
    public int character_Attack_Count { get; set; } // 공격 횟수
    public int character_Damaged { get; set; } // 받을 데미지
    public float character_Buffed_Attack { get; set; } // 가하는 피해 증가량
    public float character_Buffed_Damaged { get; set; } // 받는 피해 증가량
    public bool character_Divine_Shield { get; set; } // 천상의 보호막 유/무 true = 있음 false = 없음
    public bool character_Revivial { get; set; } // 부활 유/무 true = 있음 false = 없음

    public Character_Script enemy_Character_Script;


    // Start is called before the first frame update
    void Start()
    {
        character_HP = 0;
        character_AP = 0;
        character_Attack_Damage = 0;
        character_Num_Of_Attack_Range = 0;
        character_Num_Of_Greed = 0;
        character_Attack_Speed = 0;
        character_Is_Preemptive = false;
        character_Attack_Range = new bool[9]
            { false, false, false,
              false, false, false,
              false, false, false };
        character_Attack_Count = 0;
        character_Damaged = 0;
        character_Buffed_Attack = 0.0f;
        character_Buffed_Damaged = 0.0f;
        character_Divine_Shield = false;
        character_Revivial = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Character_Attack(GameObject enemy_Character)
    {
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();


    }
}
