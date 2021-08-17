using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;

namespace CharacterStats
{
    public enum CharacterType
    {
        Attacker = 1,
        Defender,
        Balance
    };

    public enum CharacterSkill
    {
        Attack_Confidence,          // 자신감
        Attack_Executioner,         // 처형자
        Attack_Struggle,            // 발악
        Attack_Ranger,              // 명사수
        Attack_ArmorPiercer,        // 철갑탄
        Attack_DivineShield,        // 천상의보호막
        Attack_Sturdy,              // 옹골참(기합)
        Balance_Blessing,           // 축복
        Balance_GBGH,               // 모아니면 도
        Balance_Smoke,              // 연막탄
        Balance_Survivor,           // 생존자
        Balance_Curse,              // 저주
        Balance_WideCounter,        // 광역반격
        Balance_DestinyBond,        // 길동무 
        Defense_Disarm,             // 무장해제
        Defense_Coward,             // 겁쟁이
        Defense_Patience,           // 인내심
        Defense_Responsibility,     // 책임감
        Defense_Barrier,            // 방벽
        Defense_Encourage,          // 격려
        Defense_Thronmail           // 가시갑옷

    };
}

// 캐릭터의 스탯을 가지고 있는 클래스
public class Character : MonoBehaviour
{
    public Sprite[] Character_Sprite;

    // 전투 전 캐릭터가 기본으로 가지고 있는 변수
    // Original Variables
    public int character_ID { get; set; } // 캐릭터 ID
    public CharacterType character_Type { get; set; }
    public CharacterSkill character_Skill { get; set; }
    public bool character_Is_Allive { get; set; } // 캐릭터 생존 유무
    public int character_HP { get; set; } // 현재 체력

    public int character_MaxHP { get; set; }
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // 공격력
    public int character_Num_Of_Grid { get; set; } // 그리드 넘버
    public int character_Attack_Order { get; set; } // 공격 순서
    public bool[] character_Attack_Range { get; set; } // 공격 범위
    public int character_Counter_Probability { get; set; } // 반격확률
    // 전투 중 활성화되는 변수
    // Battle-Oriented Variables
    public int character_Number { get; set; }  // ~~번 캐릭터 공격!할때 쓰는 변수
    public int character_Team_Number { get; set; } // 팀 구분
    public int character_Buffed_Attack { get; set; } // 가하는 피해 증가량
    public int character_Buffed_Damaged { get; set; } // 받는 피해 증가량
    public bool character_Counter { get; set; } //해당 턴에 피격당하여, 카운터를 치는지 판단하는 변수
    public bool character_is_Killed { get; set; } //  해당 턴에 사망했는지를 판단하는 변수
    public int character_is_Kill { get; set; } // 해당 턴에 적을 죽였는지를 판단하는 변수
    public bool character_Divine_Shield { get; set; } // 천상의 보호막 유/무 true = 있음 false = 없음
    public bool is_patience_buffed { get; set; } // 인내심 버프가 켜져있는가? (인내심캐릭터만 적용)
    public GameObject killedBy { get; set; }
    public bool is_hit_this_turn { get; set; } //피격 시 발동되는 스킬을 위한 변수

    public bool is_overkill { get; set; }

    protected List<Dictionary<string, object>> character_data; // 데이터 저장소

    // Debug
    public CharacterType Debug_Type;
    public CharacterSkill Debug_Skill;
    public bool[] Debug_character_Attack_Range;
    public int Debug_character_Grid_Number;
    public int Debug_Character_Damage;
    public int Debug_Character_HP;
    public int Debug_Character_Attack_order;

    private void Awake()
    {
        Character_Reset();
        Debug_character_Attack_Range = new bool[9];
    }

    // Update is called once per frame
    void Update()
    {
        Character_Sprite_Setting();
    }

    public void Character_Sprite_Setting()
    {
        if (gameObject.CompareTag("Null_Character"))
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        else
        {
            // 현재 ID 개수(14개)만큼의 스프라이트가 없어서 임시로 적용함
            // gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[Character_ID];
            gameObject.GetComponent<SpriteRenderer>().sprite = Character_Sprite[(character_ID - 1) % 7 + 1];
        }
    }

    public void Character_Reset() // 캐릭터의 정보를 초기화한다.
    {
        character_ID = 0;
        character_Type = CharacterType.Attacker;
        character_Skill = CharacterSkill.Attack_Confidence;
        character_Is_Allive = true;
        character_MaxHP = 0;
        character_HP = 0;
        character_AP = 0;
        character_Attack_Damage = 0;
        character_Num_Of_Grid = 0;
        character_Attack_Order = 0;
        character_Attack_Range = new bool[9]
            { false, false, false,
              false, false, false,
              false, false, false };
        character_Counter_Probability = 0;
        character_Buffed_Attack = 0;
        character_Buffed_Damaged = 0;
        character_Divine_Shield = false;
        is_patience_buffed = false;
        character_Counter = false;
        character_is_Kill = 0;
        character_Number = 0;
        character_is_Killed = false;
        killedBy = null;
        is_hit_this_turn = false;
        is_overkill = false;
    }

    public void Debuging_Character()
    {
        Debug_Skill = character_Skill;
        Debug_Type = character_Type;
        Debug_Character_HP = character_HP;
        Debug_character_Attack_Range = character_Attack_Range;
        Debug_character_Grid_Number = character_Num_Of_Grid;
        Debug_Character_Damage = character_Attack_Damage;
        Debug_Character_Attack_order = character_Attack_Order;
    }

    protected void setting_type(int num)
    {
        if ((string)character_data[num]["Type"] == "공격형")
        {
            character_Type = CharacterType.Attacker;
        }

        if ((string)character_data[num]["Type"] == "밸런스형")
        {
            character_Type = CharacterType.Balance;
        }

        if ((string)character_data[num]["Type"] == "방어형")
        {
            character_Type = CharacterType.Defender;
        }
        //Debug.Log(character_data[num]["Type"]);
        //Debug.Log(character_Type);
    }

    protected void setting_skill(int num)
    {
        switch ((string)character_data[num]["Skill"])
        {
            case "자신감":
                character_Skill = CharacterSkill.Attack_Confidence;
                break;
            case "처형자":
                character_Skill = CharacterSkill.Attack_Executioner;
                break;
            case "발악":
                character_Skill = CharacterSkill.Attack_Struggle;
                break;
            case "명사수":
                character_Skill = CharacterSkill.Attack_Ranger;
                break;
            case "철갑탄":
                character_Skill = CharacterSkill.Attack_ArmorPiercer;
                break;
            case "천상의보호막":
                character_Skill = CharacterSkill.Attack_DivineShield;
                break;
            case "옹골참":
                character_Skill = CharacterSkill.Attack_Sturdy;
                break;
            case "축복":
                character_Skill = CharacterSkill.Balance_Blessing;
                break;
            case "모아니면도":
                character_Skill = CharacterSkill.Balance_GBGH;
                break;
            case "무장해제":
                character_Skill = CharacterSkill.Defense_Disarm;
                break;
            case "연막탄":
                character_Skill = CharacterSkill.Balance_Smoke;
                break;
            case "생존자":
                character_Skill = CharacterSkill.Balance_Survivor;
                break;
            case "저주":
                character_Skill = CharacterSkill.Balance_Curse;
                break;
            case "광역반격":
                character_Skill = CharacterSkill.Balance_WideCounter;
                break;
            case "길동무":
                character_Skill = CharacterSkill.Balance_DestinyBond;
                break;
            case "겁쟁이":
                character_Skill = CharacterSkill.Defense_Coward;
                break;
            case "인내심":
                character_Skill = CharacterSkill.Defense_Patience;
                break;
            case "책임감":
                character_Skill = CharacterSkill.Defense_Responsibility;
                break;
            case "방벽":
                character_Skill = CharacterSkill.Defense_Barrier;
                break;
            case "격려":
                character_Skill = CharacterSkill.Defense_Encourage;
                break;
            case "가시갑옷":
                character_Skill = CharacterSkill.Defense_Thronmail;
                break;


        }
    }

    protected void setting_Attack_Range(int num)
    {
        int number = (int)character_data[num]["Attack_Range"];
        int arrayNumber = 0;

        while (number != 0)
        {
            if (number % 10 == 1)
                character_Attack_Range[arrayNumber] = false;
            else
                character_Attack_Range[arrayNumber] = true;
            arrayNumber++;
            number /= 10;
        }
    }

    public void Character_Setting(int num) // 데이터 세팅
    {
        //데이터 세팅.

        // 혹시나 사용법 궁금할까봐 남기는 주석
        // character_data 에 모든 데이터들이 저장되고,
        // 그 데이터의 사용법은 이러하다
        // character_data[원하는 행(가로줄)]["원하는 변수"]
        // 쓸일없기를 바람. 어차피 초기화용도임.

        character_data = CSVReader.Read("Character_DB");

        Character_Reset();


        character_Is_Allive = true;
        character_ID = (int)character_data[num]["ID"];
        setting_type(num);
        setting_skill(num);
        character_HP = (int)character_data[num]["HP"];
        character_MaxHP = character_HP;
        character_AP = (int)character_data[num]["AP"];
        character_Attack_Damage = (int)character_data[num]["Attack_Damage"];
        character_Attack_Range = new bool[9];
        setting_Attack_Range(num);
        character_Counter_Probability = (int)character_data[num]["Counter_Probability"];
    }

    public void Copy_Character_Stat(GameObject copyObject) // 캐릭터스크립트 내의 변수들을 복사하는 함수
    {
        Character copy = copyObject.GetComponent<Character>();
        character_Skill = copy.character_Skill;
        character_Type = copy.character_Type;
        character_ID = copy.character_ID;
        character_Is_Allive = copy.character_Is_Allive;
        character_HP = copy.character_HP;
        character_AP = copy.character_AP;
        character_Attack_Damage = copy.character_Attack_Damage;
        for (int i = 0; i < 9; i++)
        {
            character_Attack_Range[i] = copy.character_Attack_Range[i];
        }
        character_Num_Of_Grid = copy.character_Num_Of_Grid;
        character_Attack_Order = copy.character_Attack_Order;
        character_Attack_Range = copy.character_Attack_Range;

        character_Buffed_Attack = copy.character_Buffed_Attack;
        character_Buffed_Damaged = copy.character_Buffed_Damaged;
        character_Divine_Shield = copy.character_Divine_Shield;
        character_Counter_Probability = copy.character_Counter_Probability;

    }
}
