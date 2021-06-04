using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    private static BattleManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static BattleManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(BattleManager)) as BattleManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    public GameObject[] bM_Character_Team1;
    public GameObject[] bM_Character_Team2;
    public int bM_Phase { get; set; }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }
    
    public GameObject Damaged_Grid;
    private float Debug_Delay;

    // Start is called before the first frame update
    void Start()
    {
        Debug_Delay = 0.0f;

        bM_Phase = 0;
        bM_Team1_Is_Preemitive = true;
        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;

        GameObject map = null;
        map = GameObject.Find("Castle_Background");
        if(map != null)
        {
            Debug.Log(map.name + " ������Ʈ�� �ҷ����µ� ����");
            map.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            Debug.LogError(map.name + " �ҷ����µ� ����");
        }
        
        GameObject Damaged_Gird = Instantiate(Damaged_Grid);

        for (float x= -6.7f; x<=-2f;)
        {
            for(float y=-2f; y<=3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;

            }

            x +=2.25f;
        }
        // (grid-1)%3 %3->�� ����
        for (float x = 2.2f; x <= 6.7f;)
        {

            for (float y = -2f; y <= 3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;

            }

            x += 2.25f;
        }
        // Instantiate(Damaged_Grid, new Vector3(-4.5f, 2.5f, 0), Quaternion.identity);
        // Instantiate(Damaged_Grid, new Vector3(-2.2f, 2.5f, 0), Quaternion.identity);
        
    }


    // Update is called once per frame
    void Update()
    {
        Debug_Delay += Time.deltaTime;

        if (Debug_Delay > 2) // ������.
        {
            Running_Phase();
            Debug_Delay = 0;
        }
    }

    void Running_Phase()
    {
        switch(bM_Phase)
        {
            case 0:
                BM_Character_Setting(); // �������̴� ���� ���濹��.
                bM_Phase++;
                break;
            case 1:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 2:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 3:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 4:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 5:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            default:
                break;      
        }
    }

    void BM_Character_Setting()
    {
        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team1[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Is_Preemptive = true; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().Debuging_Character();

            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Is_Preemptive = false; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().Debuging_Character();
        }
        
    }

    void Character_Attack(GameObject attacker,GameObject[] enemy_Characters,int attacked_Grid) //ĳ���� ����
    {
        // ���� �ϴ� ĳ���Ϳ�, ���� ��� ĳ���͵�, ���� �� ��ġ�� �޾ƿ´�.
        // ���� ��� ĳ���͵��� Ž���Ͽ�, ���� �� ��ġ�� �����ϰ�, ����ִ� ĳ���͸� �����Ѵ�.
        foreach(GameObject enemy_Character in enemy_Characters)
        {
            if (enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid == attacked_Grid
            && enemy_Character.GetComponent<Character_Script>().character_Is_Allive)
            {
                Debug.Log(attacker.GetComponent<Character_Script>().character_Num_Of_Grid + " attack " + enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid +
                    " by " + attacker.GetComponent<Character_Script>().character_Attack_Damage);
                attacker.GetComponent<Character_Script>().Character_Attack(enemy_Character);
            }
        }
    }
    void Battle(int phase) // ����,�İ��� ���� ��Ʋ�� �����Ѵ�.
    {
        if (bM_Team1_Is_Preemitive) // ���� �Ǻ� (���� ���� ����)
        {
            foreach(GameObject team1_Character in bM_Character_Team1)
            {
                if(team1_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                && team1_Character.GetComponent<Character_Script>().character_Is_Allive) // ��1�� ĳ���� �� ���ݼ����� ������� �Ȱ���, ����ִ� ĳ���Ͱ� ������ �����Ѵ�.
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (team1_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // ���ݹ�����ŭ �����Ѵ�.
                            Character_Attack(team1_Character,bM_Character_Team2, j + 1);
                    }
                }
                team1_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        else
        {
            foreach (GameObject team2_Character in bM_Character_Team2)
            {
                if (team2_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                 && team2_Character.GetComponent<Character_Script>().character_Is_Allive) // ��2�� ĳ���� �� ���ݼ����� ������� �Ȱ���, ����ִ� ĳ���Ͱ� ������ �����Ѵ�.
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (team2_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // ���ݹ�����ŭ �����Ѵ�.
                            Character_Attack(team2_Character, bM_Character_Team1, j + 1);
                    }
                }
                team2_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive; // ���İ� ����!

        Calculate_Remain_HP();

        Debug.Log("Phase " + bM_Phase + " Team1 ����ü�� = " + bM_Remain_HP_Team1);
        Debug.Log("Phase " + bM_Phase + " Team2 ����ü�� = " + bM_Remain_HP_Team2);
    }

    void Calculate_Remain_HP() //���� ü�� ���
    {
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        for (int i = 0; i < 5; i++)
        {
            bM_Remain_HP_Team1 += bM_Character_Team1[i].GetComponent<Character_Script>().character_HP;
            bM_Remain_HP_Team2 += bM_Character_Team2[i].GetComponent<Character_Script>().character_HP;
        }
    }
}

