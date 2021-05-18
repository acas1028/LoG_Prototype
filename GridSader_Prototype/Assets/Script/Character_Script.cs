using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Script : MonoBehaviour
{
    public int character_ID { get; set; } // ĳ���� ID
    public bool character_Is_Allive { get; set; } // ĳ���� ���� ����
    public int character_HP { get; set; } // ü��
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // ���ݷ�
    public int character_Num_Of_Attack_Range { get; set; } // ���� ���� ����
    public int character_Num_Of_Greed { get; set; } // �׸��� �ѹ�
    public int character_Attack_Speed { get; set; } // ���� ����
    public bool character_Is_Preemptive { get; set; } // ���� �İ� true = ���� false = �İ�
    public bool[] character_Attack_Range { get; set; } // ���� ����
    public int character_Attack_Count { get; set; } // ���� Ƚ��
    public int character_Damaged { get; set; } // ���� ������
    public int character_Buffed_Attack { get; set; } // ���ϴ� ���� ������
    public int character_Buffed_Damaged { get; set; } // �޴� ���� ������
    public bool character_Divine_Shield { get; set; } // õ���� ��ȣ�� ��/�� true = ���� false = ����
    public bool character_Revivial { get; set; } // ��Ȱ ��/�� true = ���� false = ����

    List<Dictionary<string, object>> character_data; // ������ �����

    // Start is called before the first frame update
    void Start()
    {
        Character_Reset();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Character_Reset()
    {
        character_ID = 0;
        character_Is_Allive = false;
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
        character_Buffed_Attack = 0;
        character_Buffed_Damaged = 0;
        character_Divine_Shield = false;
        character_Revivial = false;
    }

    public void Character_Attack(GameObject enemy_Character)
    {
        Character_Script enemy_Character_Script;
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();

        enemy_Character_Script.character_Damaged = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;
    }

    public void Character_Damaged()
    {
        character_Damaged = (character_Damaged * (100 + character_Buffed_Damaged)) / 100;

        if(character_Divine_Shield)
        {
            character_Divine_Shield = false;
        }
        else
            character_HP -= character_Damaged;

        if(character_HP <= 0)
        {
            character_HP = 0;
            Character_Dead();
        }
    }

    public void Character_Dead()
    {
        Character_Reset();
    }

    public void Character_Setting(int num) // ������ ����
    {
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
        character_Num_Of_Greed = (int)character_data[num]["Num_Of_Greed"];
        character_Attack_Speed = (int)character_data[num]["Attack_Speed"];
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

            number /= 10;
        }
    }
}
