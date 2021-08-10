using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ������ ������ ������ �ִ� Ŭ����
public class Character : MonoBehaviour
{
    public enum Type
    {
        Attacker = 1,
        Defender,
        Balance
    };

    public enum Skill
    {
        Attack_Confidence,          // �ڽŰ�
        Attack_Executioner,         // ó����
        Attack_Struggle,            // �߾�
        Attack_Ranger,              // ����
        Attack_ArmorPiercer,        // ö��ź
        Attack_DivineShield,        // õ���Ǻ�ȣ��
     // Attack_Sturdy,              // �˰���
        Balance_Blessing,           // �ູ
        Balance_GBGH,               // ��ƴϸ� ��
        Balance_Smoke,              // ����ź
        Balance_Survivor,           // ������
        Balance_Curse,              // ����
        Balance_WideCounter,        // �����ݰ�
        Balance_DestinyBond,        // �浿�� 
        Defense_Disarm,             // ��������
    };

    // ���� �� ĳ���Ͱ� �⺻���� ������ �ִ� ����
    // Original Variables
    public int character_ID { get; set; } // ĳ���� ID
    public Type character_Type { get; set; }
    public Skill character_Skill { get; set; }
    public bool character_Is_Allive { get; set; } // ĳ���� ���� ����
    public int character_HP { get; set; } // ���� ü��

    public int character_MaxHP { get; set; }
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // ���ݷ�
    public int character_Num_Of_Grid { get; set; } // �׸��� �ѹ�
    public int character_Attack_Order { get; set; } // ���� ����
    public bool[] character_Attack_Range { get; set; } // ���� ����

    // ���� �� Ȱ��ȭ�Ǵ� ����
    // Battle-Oriented Variables
    public int character_Number { get; set; }  // ~~�� ĳ���� ����!�Ҷ� ���� ����
    public int character_Team_Number { get; set; } // �� ����
    public int character_Buffed_Attack { get; set; } // ���ϴ� ���� ������
    public int character_Buffed_Damaged { get; set; } // �޴� ���� ������
    public bool character_Counter { get; set; } //�ش� �Ͽ� �ǰݴ��Ͽ�, ī���͸� ġ���� �Ǵ��ϴ� ����
    public bool character_is_Killed { get; set; } //  �ش� �Ͽ� ����ߴ����� �Ǵ��ϴ� ����
    public int character_is_Kill { get; set; } // �ش� �Ͽ� ���� �׿������� �Ǵ��ϴ� ����
    public bool character_Divine_Shield { get; set; } // õ���� ��ȣ�� ��/�� true = ���� false = ����
    public bool character_Sturdy { get; set; } // �˰��� �ߵ�
    public GameObject killedBy { get; set; }

    protected List<Dictionary<string, object>> character_data; // ������ �����

    // Debug
    public Type Debug_Type;
    public Skill Debug_Skill;
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
        
    }

    public void Character_Reset() // ĳ������ ������ �ʱ�ȭ�Ѵ�.
    {
        character_ID = 0;
        character_Type = Type.Attacker;
        character_Skill = Skill.Attack_Confidence;
        character_Is_Allive = false;
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

        character_Buffed_Attack = 0;
        character_Buffed_Damaged = 0;
        character_Divine_Shield = false;
        character_Sturdy = false;
        character_Counter = false;
        character_is_Kill = 0;
        character_Number = 0;
        character_is_Killed = false;
        killedBy = null;
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
        if ((string)character_data[num]["Type"] == "������")
        {
            character_Type = Type.Attacker;
        }

        if ((string)character_data[num]["Type"] == "�뷱����")
        {
            character_Type = Type.Balance;
        }

        if ((string)character_data[num]["Type"] == "�����")
        {
            character_Type = Type.Defender;
        }
        //Debug.Log(character_data[num]["Type"]);
        //Debug.Log(character_Type);
    }

    protected void setting_skill(int num)
    {
        switch ((string)character_data[num]["Skill"])
        {
            case "�ڽŰ�":
                character_Skill = Skill.Attack_Confidence;
                break;
            case "ó����":
                character_Skill = Skill.Attack_Executioner;
                break;
            case "�߾�":
                character_Skill = Skill.Attack_Struggle;
                break;
            case "����":
                character_Skill = Skill.Attack_Ranger;
                break;
            case "ö��ź":
                character_Skill = Skill.Attack_ArmorPiercer;
                break;
            case "õ���Ǻ�ȣ��":
                character_Skill = Skill.Attack_DivineShield;
                break;
            case "�ູ":
                character_Skill = Skill.Balance_Blessing;
                break;
            case "��ƴϸ鵵":
                character_Skill = Skill.Balance_GBGH;
                break;
            case "��������":
                character_Skill = Skill.Defense_Disarm;
                break;
            case "����ź":
                character_Skill = Skill.Balance_Smoke;
                break;
            case "������":
                character_Skill = Skill.Balance_Survivor;
                break;
            case "����":
                character_Skill = Skill.Balance_Curse;
                break;
            case "�����ݰ�":
                character_Skill = Skill.Balance_WideCounter;
                break;
            case "�浿��":
                character_Skill = Skill.Balance_DestinyBond;
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

    public void Character_Setting(int num) // ������ ����
    {
        //������ ����.

        // Ȥ�ó� ���� �ñ��ұ�� ����� �ּ�
        // character_data �� ��� �����͵��� ����ǰ�,
        // �� �������� ������ �̷��ϴ�
        // character_data[���ϴ� ��(������)]["���ϴ� ����"]
        // ���Ͼ��⸦ �ٶ�. ������ �ʱ�ȭ�뵵��.

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
    }

    public void Copy_Character_Stat(GameObject copyObject) // ĳ���ͽ�ũ��Ʈ ���� �������� �����ϴ� �Լ�
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
        character_Sturdy = copy.character_Sturdy;

    }
}
