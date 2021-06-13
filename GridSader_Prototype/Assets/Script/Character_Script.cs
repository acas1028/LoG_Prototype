using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Script : MonoBehaviour
{ 
    public int character_ID { get; set; } // 캐릭터 ID
    public bool character_Is_Allive { get; set; } // 캐릭터 생존 유무
    public int character_HP { get; set; } // 체력
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // 공격력
    public int character_Num_Of_Attack_Range { get; set; } // 공격 범위 숫자
    public int character_Num_Of_Grid { get; set; } // 그리드 넘버
    public int character_Attack_Order { get; set; } // 공격 순서
    public bool character_Is_Preemptive { get; set; } // 선공 후공 true = 선공 false = 후공
    public bool[] character_Attack_Range { get; set; } // 공격 범위
    public int character_Attack_Count { get; set; } // 공격 횟수
    public int character_Damaged { get; set; } // 받을 데미지
    public int character_Buffed_Attack { get; set; } // 가하는 피해 증가량
    public int character_Buffed_Damaged { get; set; } // 받는 피해 증가량
    public bool character_Divine_Shield { get; set; } // 천상의 보호막 유/무 true = 있음 false = 없음
    public bool character_Revivial { get; set; } // 부활 유/무 true = 있음 false = 없음

    List<Dictionary<string, object>> character_data; // 데이터 저장소

    // Debug

    public bool[] Debug_character_Attack_Range;
    public int Debug_character_Grid_Number;
    public int Debug_Character_Damage;
    public int Debug_Character_HP;
    public int Debug_Character_Attack_order;

    // Debug
    // Start is called before the first frame update
    void Start()
    {
        Debug_character_Attack_Range = new bool[9];
        Character_Reset();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void Character_Reset() // 캐릭터의 정보를 초기화한다.
    {
        character_ID = 0;
        character_Is_Allive = false;
        character_HP = 0;
        character_AP = 0;
        character_Attack_Damage = 0;
        character_Num_Of_Attack_Range = 0;
        character_Num_Of_Grid = 0;
        character_Attack_Order = 0;
        character_Is_Preemptive = false;
        character_Attack_Range = new bool[9]
            { false, false, false,
              false, false, false,
              false, false, false };
        character_Attack_Count = 0;
        character_Damaged = 0;
        character_Buffed_Attack = 0;
        character_Buffed_Damaged = 0;
        character_Divine_Shield = false;
        character_Revivial = false;
    }

    IEnumerator SetCharacterRed()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(2.0f);

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        yield break;
    }

    public void Character_Attack(GameObject enemy_Character) // 캐릭터 스크립트 내에 있는 공격 함수.
    {
        // 적 캐릭터를 받아와서, 그 캐릭터의 정보에 접근하여 받을 데미지에 공격력 만큼을 저장시킴.
        StartCoroutine(SetCharacterRed());
        
        Character_Script enemy_Character_Script;
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();

        enemy_Character_Script.character_Damaged = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;
        enemy_Character_Script.Character_Damaged(); // 받을 데미지에 값이 저장되자마자 피격 함수 발동
    }

    public void Character_Damaged() // 피격 함수
    {
        // 받을 데미지를 다시 계산.
        character_Damaged = (character_Damaged * (100 + character_Buffed_Damaged)) / 100;

        if(character_Divine_Shield) // 나중에 들어가겠지만 간단해서 넣어뒀음. 천상의보호막임.
        {
            character_Divine_Shield = false;
        }
        else // 천보없으면 체력닳아야죠?
            character_HP -= character_Damaged;

        if(character_HP <= 0) // 체력이 0이하가되면 체력을 0으로 초기화하고 사망함수 발동
        {
            character_HP = 0;
            Character_Dead();
        }
        character_Damaged = 0;
    }

    public void Character_Dead() // 캐릭터 사망 함수. 아마 나중에 무언가가 더 추가되겠지?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        Character_Reset();
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

        character_ID = (int)character_data[num]["ID"];
        if ((int)character_data[num]["Is_Alive"] == 0)
            character_Is_Allive = false;
        else
            character_Is_Allive = true;
        character_HP = (int)character_data[num]["HP"];
        character_AP = (int)character_data[num]["AP"];
        character_Attack_Damage = (int)character_data[num]["Attack_Damage"];
        character_Num_Of_Attack_Range = (int)character_data[num]["Num_Of_Attack_Range"];
        character_Num_Of_Grid = (int)character_data[num]["Num_Of_Grid"];
        character_Attack_Order = (int)character_data[num]["Attack_Order"];
        if ((int)character_data[num]["Is_Preemptive"] == 0)
            character_Is_Preemptive = false;
        else
            character_Is_Preemptive = true;
        character_Attack_Range = new bool[9];
        setting_Attack_Range(num);
        character_Attack_Count = (int)character_data[num]["Attack_Count"];
        character_Damaged = (int)character_data[num]["Damaged"];
        character_Buffed_Attack = (int)character_data[num]["Buffed_Attack"];
        character_Buffed_Damaged = (int)character_data[num]["Buffed_Damaged"];
        if ((int)character_data[num]["Divine_Shield"] == 0)
            character_Divine_Shield = false;
        else
            character_Divine_Shield = true;

        if ((int)character_data[num]["Revivial"] == 0)
            character_Revivial = false;
        else
            character_Revivial = true;
    }

    void setting_Attack_Range(int num)
    {
        int number = (int)character_data[num]["Attack_Range"];
        int arrayNumber = 0;

        while(number != 0)
        {
            if (number % 10 == 1)
                character_Attack_Range[arrayNumber] = false;
            else
                character_Attack_Range[arrayNumber] = true;
            arrayNumber++;
            number /= 10;
        }
    }

    public void Debuging_Character()
    {
        Debug_Character_HP = character_HP;
        Debug_character_Attack_Range = character_Attack_Range;
        Debug_character_Grid_Number = character_Num_Of_Grid;
        Debug_Character_Damage = character_Attack_Damage;
        Debug_Character_Attack_order = character_Attack_Order;
    }

    public void Copy_Character_Stat(GameObject copyObject) // 캐릭터스크립트 내의 변수들을 복사하는 함수
    {
        Character_Script copy = copyObject.GetComponent<Character_Script>();
        character_ID = copy.character_ID;
        character_Is_Allive = copy.character_Is_Allive;
        character_HP = copy.character_HP;
        character_AP = copy.character_AP;
        character_Attack_Damage = copy.character_Attack_Damage;
        for(int i = 0; i < 9; i++)
        {
            character_Attack_Range[i] = copy.character_Attack_Range[i];
        }
        character_Num_Of_Grid = copy.character_Num_Of_Grid;
        character_Attack_Order = copy.character_Attack_Order;
        character_Is_Preemptive = copy.character_Is_Preemptive;
        character_Attack_Range = copy.character_Attack_Range;
        character_Attack_Count = copy.character_Attack_Count;
        character_Damaged = copy.character_Damaged;
        character_Buffed_Attack = copy.character_Buffed_Attack;
        character_Buffed_Damaged = copy.character_Buffed_Damaged;
        character_Divine_Shield = copy.character_Divine_Shield;
        character_Revivial = copy.character_Revivial;
    }
}
