using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Script : MonoBehaviour
{
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
    public float character_Buffed_Attack { get; set; } // ���ϴ� ���� ������
    public float character_Buffed_Damaged { get; set; } // �޴� ���� ������
    public bool character_Divine_Shield { get; set; } // õ���� ��ȣ�� ��/�� true = ���� false = ����
    public bool character_Revivial { get; set; } // ��Ȱ ��/�� true = ���� false = ����

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
