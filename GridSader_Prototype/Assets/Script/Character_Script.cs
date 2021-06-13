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
    public int character_Num_Of_Grid { get; set; } // �׸��� �ѹ�
    public int character_Attack_Order { get; set; } // ���� ����
    public bool character_Is_Preemptive { get; set; } // ���� �İ� true = ���� false = �İ�
    public bool[] character_Attack_Range { get; set; } // ���� ����
    public int character_Attack_Count { get; set; } // ���� Ƚ��
    public int character_Damaged { get; set; } // ���� ������
    public int character_Buffed_Attack { get; set; } // ���ϴ� ���� ������
    public int character_Buffed_Damaged { get; set; } // �޴� ���� ������
    public bool character_Divine_Shield { get; set; } // õ���� ��ȣ�� ��/�� true = ���� false = ����
    public bool character_Revivial { get; set; } // ��Ȱ ��/�� true = ���� false = ����

    List<Dictionary<string, object>> character_data; // ������ �����

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

    public void Character_Reset() // ĳ������ ������ �ʱ�ȭ�Ѵ�.
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

    public void Character_Attack(GameObject enemy_Character) // ĳ���� ��ũ��Ʈ ���� �ִ� ���� �Լ�.
    {
        // �� ĳ���͸� �޾ƿͼ�, �� ĳ������ ������ �����Ͽ� ���� �������� ���ݷ� ��ŭ�� �����Ŵ.
        StartCoroutine(SetCharacterRed());
        
        Character_Script enemy_Character_Script;
        enemy_Character_Script = enemy_Character.GetComponent<Character_Script>();

        enemy_Character_Script.character_Damaged = (character_Attack_Damage * (100 + character_Buffed_Attack)) / 100;
        enemy_Character_Script.Character_Damaged(); // ���� �������� ���� ������ڸ��� �ǰ� �Լ� �ߵ�
    }

    public void Character_Damaged() // �ǰ� �Լ�
    {
        // ���� �������� �ٽ� ���.
        character_Damaged = (character_Damaged * (100 + character_Buffed_Damaged)) / 100;

        if(character_Divine_Shield) // ���߿� �������� �����ؼ� �־����. õ���Ǻ�ȣ����.
        {
            character_Divine_Shield = false;
        }
        else // õ�������� ü�´�ƾ���?
            character_HP -= character_Damaged;

        if(character_HP <= 0) // ü���� 0���ϰ��Ǹ� ü���� 0���� �ʱ�ȭ�ϰ� ����Լ� �ߵ�
        {
            character_HP = 0;
            Character_Dead();
        }
        character_Damaged = 0;
    }

    public void Character_Dead() // ĳ���� ��� �Լ�. �Ƹ� ���߿� ���𰡰� �� �߰��ǰ���?
    {
        Debug.Log(character_Num_Of_Grid + " is Dead");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        Character_Reset();
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

    public void Copy_Character_Stat(GameObject copyObject) // ĳ���ͽ�ũ��Ʈ ���� �������� �����ϴ� �Լ�
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
