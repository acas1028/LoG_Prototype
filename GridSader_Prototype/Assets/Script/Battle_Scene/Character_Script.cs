using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Character_Script : MonoBehaviour
{
    public enum Type
    {
        Attacker = 1,
        Defender,
        Balance
    };

    public enum Skill
    {
        Balance_Union = 1,
        Defense_Disarm,
        Attack_Confidence,
        Attack_Executioner,
        Balance_GbGH
    };

    // ���� �� ĳ���Ͱ� �⺻���� ������ �ִ� ����
    // Original Variables
    public int character_ID { get; set; } // ĳ���� ID
    public Type character_Type { get; set; }
    public Skill character_Skill { get; set; }
    public bool character_Is_Allive { get; set; } // ĳ���� ���� ����
    public int character_HP { get; set; } // ü��
    public int character_AP { get; set; } // AP
    public int character_Attack_Damage { get; set; } // ���ݷ�
    public int character_Num_Of_Grid { get; set; } // �׸��� �ѹ�
    public int character_Attack_Order { get; set; } // ���� ����
    public bool[] character_Attack_Range { get; set; } // ���� ����

    // ���� �� Ȱ��ȭ�Ǵ� ����
    // Battle-Oriented Variables
    public int character_Attack_Count { get; set; } // ���� Ƚ��(���� ���� ����)
    public int character_Team_Number { get; set; } // �� ����
    public int character_Buffed_Attack { get; set; } // ���ϴ� ���� ������
    public int character_Buffed_Damaged { get; set; } // �޴� ���� ������
    public bool character_Counter { get; set; } //�ش� �Ͽ� �ǰݴ��Ͽ�, ī���͸� ġ���� �Ǵ��ϴ� ����
    public bool character_Activate_Skill { get; set; }
    public int character_is_Kill { get; set; } // �ش� �Ͽ� ���� �׿������� �Ǵ��ϴ� ����
    public bool character_Divine_Shield { get; set; } // õ���� ��ȣ�� ��/�� true = ���� false = ����
    public bool character_Revivial { get; set; } // ��Ȱ ��/�� true = ���� false = ����

    List<Dictionary<string, object>> character_data; // ������ �����

    // Debug

    public Type Debug_Type;
    public Skill Debug_Skill;
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

    public void Character_Reset() // ĳ������ ������ �ʱ�ȭ�Ѵ�.
    {

        character_ID = 0;
        character_Type = Type.Attacker;
        character_Skill = Skill.Attack_Confidence;
        character_Is_Allive = false;
        character_HP = 0;
        character_AP = 0;
        character_Attack_Damage = 0;
        character_Num_Of_Grid = 0;
        character_Attack_Order = 0;
        character_Attack_Range = new bool[9]
            { false, false, false,
              false, false, false,
              false, false, false };

        character_Attack_Count = 0;
        character_Buffed_Attack = 0;
        character_Buffed_Damaged = 0;
        character_Divine_Shield = false;
        character_Activate_Skill = false;
        character_Revivial = false;
        character_Counter = false;
        character_is_Kill = 0;
    }

    IEnumerator SetCharacterRed()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(2.0f);

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        yield break;
    }

    public void Character_Attack(GameObject enemy_Character) // ĳ���� ��ũ��Ʈ ���� �ִ� ���� �Լ�.
    {
        // �� ĳ���͸� �޾ƿͼ�, �� ĳ������ ������ �����Ͽ� ���� �������� ���ݷ� ��ŭ�� �����Ŵ.
        StartCoroutine(SetCharacterRed());

        Character_Script enemy_Character_Script;
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();

        int damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;
        enemy_Character_Script.Character_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�
        character_Buffed_Attack = 0;
    }

    public void Character_Damaged(GameObject attacker, int damage) // �ǰ� �Լ�
    {
        // ���� �������� �ٽ� ���.

        Character_Counter();

        int final_damage = (damage * (100 + character_Buffed_Damaged)) / 100;

        character_HP -= final_damage;

        if(character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        {
            character_HP = 0;
            Character_Dead(attacker);
        }
        character_Buffed_Damaged = 0;
    }

    public void Character_Counter_Attack(GameObject enemy_Character) //ī���� �ߵ�
    {
        StartCoroutine(SetCharacterRed());

        Character_Script enemy_Character_Script;
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();

        int damage = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100 / 2;
        enemy_Character_Script.Character_Counter_Damaged(this.gameObject, damage); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�

        character_Counter = false;
    }

    public void Character_Counter_Damaged(GameObject attacker, int damage) // ī���� �ߵ�
    {
        int final_damage = (damage * (100 + character_Buffed_Damaged)) / 100;

        character_HP -= final_damage;

        if (character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        {
            character_HP = 0;
            Character_Dead(attacker);
        }
    }

    public void Character_Counter()
    {
        character_Counter = true;
    }

    public void Character_Dead(GameObject attacker) // ĳ���� ��� �Լ�. �Ƹ� ���߿� ���𰡰� �� �߰��ǰ���?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        attacker.GetComponent<Character_Script>().character_is_Kill++;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        character_Is_Allive = false;
        character_Counter = false;
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
        character_AP = (int)character_data[num]["AP"];
        character_Attack_Damage = (int)character_data[num]["Attack_Damage"];
        character_Attack_Range = new bool[9];
        setting_Attack_Range(num);
    }

    void setting_type(int num)
    {
        if((string)character_data[num]["Type"] == "������")
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
        Debug.Log(character_data[num]["Type"]);
        Debug.Log(character_Type);
    }
    
    void setting_skill(int num)
    {
        if((string)character_data[num]["Skill"] == "���")
        {
            character_Skill = Skill.Balance_Union;
        }

        if ((string)character_data[num]["Skill"] == "��������")
        {
            character_Skill = Skill.Defense_Disarm;
        }

        if ((string)character_data[num]["Skill"] == "�ڽŰ�")
        {
            character_Skill = Skill.Attack_Confidence;
        }

        if ((string)character_data[num]["Skill"] == "ó����")
        {
            character_Skill = Skill.Attack_Executioner;
        }

        if ((string)character_data[num]["Skill"] == "��ƴϸ鵵")
        {
            character_Skill = Skill.Balance_GbGH;
        }
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
        Debug_Skill = character_Skill;
        Debug_Type = character_Type;
        Debug_Character_HP = character_HP;
        Debug_character_Attack_Range = character_Attack_Range;
        Debug_character_Grid_Number = character_Num_Of_Grid;
        Debug_Character_Damage = character_Attack_Damage;
        Debug_Character_Attack_order = character_Attack_Order;
    }

    public void Copy_Character_Stat(GameObject copyObject) // ĳ���ͽ�ũ��Ʈ ���� �������� �����ϴ� �Լ�
    {
        Character_Script copy = copyObject.GetComponent<Character_Script>();
        character_Skill = copy.character_Skill;
        character_Type = copy.character_Type;
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
        character_Attack_Range = copy.character_Attack_Range;

        character_Buffed_Attack = copy.character_Buffed_Attack;
        character_Buffed_Damaged = copy.character_Buffed_Damaged;
        character_Divine_Shield = copy.character_Divine_Shield;
        character_Revivial = copy.character_Revivial;
    }
}
