using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;
using Photon.Pun;

namespace CharacterStats
{
    public enum CharacterType
    {
        Attacker = 1,
        Defender,
        Balance,
        Null
    };

    public enum CharacterSkill
    {
        Attack_Confidence,          // �ڽŰ�
        Attack_Executioner,         // ó����
        Attack_Struggle,            // �߾�
        Attack_Ranger,              // ����
        Attack_ArmorPiercer,        // ö��ź
        Attack_DivineShield,        // õ���Ǻ�ȣ��
        Attack_Sturdy,              // �˰���(����)
        Balance_Blessing,           // �ູ
        Balance_GBGH,               // ��ƴϸ� ��
        Balance_Smoke,              // ����ź
        Balance_Survivor,           // ������
        Balance_Curse,              // ����
        Balance_WideCounter,        // �����ݰ�
        Balance_DestinyBond,        // �浿�� 
        Defense_Disarm,             // ��������
        Defense_Coward,             // ������
        Defense_Patience,           // �γ���
        Defense_Responsibility,     // å�Ӱ�
        Defense_Barrier,            // �溮
        Defense_Encourage,          // �ݷ�
        Defense_Thronmail,          // ���ð���
        Null                        // ��ų�� �����

    };
}

// ĳ������ ������ ������ �ִ� Ŭ����
public class Character : MonoBehaviourPunCallbacks
{
    public CharacterSpriteManager spriteManager;

    // ���� �� ĳ���Ͱ� �⺻���� ������ �ִ� ����
    // Original Variables
    public int character_ID; // ĳ���� ID
    public CharacterType character_Type;
    public CharacterSkill character_Skill;
    public bool character_Is_Allive; // ĳ���� ���� ����
    public int character_HP; // ���� ü��

    public int character_MaxHP { get; set; }
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage; // ���ݷ�
    public int character_Num_Of_Grid; // �׸��� �ѹ�
    public int character_Attack_Order; // ���� ����
    public bool[] character_Attack_Range; // ���� ����
    public int character_Counter_Probability { get; set; } // �ݰ�Ȯ��
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
    public bool is_patience_buffed { get; set; } // �γ��� ������ �����ִ°�? (�γ���ĳ���͸� ����)
    public GameObject killedBy { get; set; }
    public bool is_hit_this_turn { get; set; } //�ǰ� �� �ߵ��Ǵ� ��ų�� ���� ����

    public bool is_overkill { get; set; }
    public int stack_Survivor { get; set; }

    protected List<Dictionary<string, object>> character_data; // ������ �����

    private void Awake()
    {
        Character_Reset();
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void InitializeCharacterSprite()
    {
        spriteManager.SetInitialSprite(character_ID);
        spriteManager.SetSortingLayer(character_Num_Of_Grid);
    }

    public void Character_Reset() // ĳ������ ������ �ʱ�ȭ�Ѵ�.
    {
        character_ID = 0;
        character_Type = CharacterType.Null;  //�ʱ�ȭ �κп��� ������ ���� ������ �Ǿ� �־� �����س��ҽ��ϴ�.
        character_Skill = CharacterSkill.Null;
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

    protected void setting_type(int num)
    {
        if ((string)character_data[num]["Type"] == "������")
        {
            character_Type = CharacterType.Attacker;
        }

        if ((string)character_data[num]["Type"] == "�뷱����")
        {
            character_Type = CharacterType.Balance;
        }

        if ((string)character_data[num]["Type"] == "�����")
        {
            character_Type = CharacterType.Defender;
        }
        //Debug.Log(character_data[num]["Type"]);
        //Debug.Log(character_Type);
    }

    protected void setting_skill(int num)
    {
        Debug.Log((string)character_data[num]["Skill"]);

        switch ((string)character_data[num]["Skill"])
        {
            case "�ڽŰ�":
                character_Skill = CharacterSkill.Attack_Confidence;
                break;
            case "ó����":
                character_Skill = CharacterSkill.Attack_Executioner;
                break;
            case "�߾�":
                character_Skill = CharacterSkill.Attack_Struggle;
                break;
            case "����":
                character_Skill = CharacterSkill.Attack_Ranger;
                break;
            case "ö��ź":
                character_Skill = CharacterSkill.Attack_ArmorPiercer;
                break;
            case "õ���Ǻ�ȣ��":
                character_Skill = CharacterSkill.Attack_DivineShield;
                break;
            case "����":
                character_Skill = CharacterSkill.Attack_Sturdy;
                break;
            case "�ູ":
                character_Skill = CharacterSkill.Balance_Blessing;
                break;
            case "��ƴϸ鵵":
                character_Skill = CharacterSkill.Balance_GBGH;
                break;
            case "��������":
                character_Skill = CharacterSkill.Defense_Disarm;
                break;
            case "����ź":
                character_Skill = CharacterSkill.Balance_Smoke;
                break;
            case "������":
                character_Skill = CharacterSkill.Balance_Survivor;
                break;
            case "����":
                character_Skill = CharacterSkill.Balance_Curse;
                break;
            case "�����ݰ�":
                character_Skill = CharacterSkill.Balance_WideCounter;
                break;
            case "�浿��":
                character_Skill = CharacterSkill.Balance_DestinyBond;
                break;
            case "������":
                character_Skill = CharacterSkill.Defense_Coward;
                break;
            case "�γ���":
                character_Skill = CharacterSkill.Defense_Patience;
                break;
            case "å�Ӱ�":
                character_Skill = CharacterSkill.Defense_Responsibility;
                break;
            case "�溮":
                character_Skill = CharacterSkill.Defense_Barrier;
                break;
            case "�ݷ�":
                character_Skill = CharacterSkill.Defense_Encourage;
                break;
            case "���ð���":
                character_Skill = CharacterSkill.Defense_Thronmail;
                break;


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
        character_Counter_Probability = (int)character_data[num]["Counter_Probability"];
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
        character_Counter_Probability = copy.character_Counter_Probability;
        stack_Survivor = copy.stack_Survivor;

    }

    // PVE �������� �Լ�
    public void PVE_Player_Character_Setting(int num,string StageName)
    {
        character_data = CSVReader.Read(StageName);

        Character_Reset();

        character_Is_Allive = true;
        character_ID = (int)character_data[num]["ID"];
        setting_type(num);
        setting_skill(num);
        character_HP = (int)character_data[num]["HP"];
        character_MaxHP = character_HP;
        character_Attack_Damage = (int)character_data[num]["Attack_Damage"];
        character_Counter_Probability = (int)character_data[num]["Counter_Probability"];
        Setting_AttackRange(num);
    }

    public void PVE_Enemy_Character_Setting(int num,string StageName)
    {
        character_data = CSVReader.Read(StageName);

        Character_Reset();

        character_Is_Allive = true;
        character_ID = (int)character_data[num]["ID"];
        setting_type(num);
        setting_skill(num);
        character_HP = (int)character_data[num]["HP"];
        character_MaxHP = character_HP;
        character_Attack_Damage = (int)character_data[num]["Attack_Damage"];
        character_Counter_Probability = (int)character_data[num]["Counter_Probability"];
        character_Num_Of_Grid = (int)character_data[num]["Grid_Position"];
        Setting_AttackRange(num);
    }

    public void Setting_AttackRange(int num)
    {
        for(int j = 0; j < 9; j++)
        {
            character_Attack_Range[j] = false;
        }

        int attack_Range_Value = (int)character_data[num]["Attack_Range"];

        int i = 8;

        while(attack_Range_Value % 10 != 0)
        {
            if (attack_Range_Value % 10 == 2)
                character_Attack_Range[i] = true;
            else
                character_Attack_Range[i] = false;
            attack_Range_Value /= 10;

            i--;
        }
    }
}
