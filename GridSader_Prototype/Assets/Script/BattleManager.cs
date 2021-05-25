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
    public List<GameObject> bM_Character_Team1;
    public List<GameObject> bM_Character_Team2;
    public int bM_Phase { get; set; }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        

        bM_Phase = 0;
        bM_Team1_Is_Preemitive = true;
        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Running_Phase();
    }

    void Running_Phase()
    {
        switch(bM_Phase)
        {
            case 0:
                BM_Character_Setting();
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
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Greed = i;
            bM_Character_Team1[i].GetComponent<Character_Script>().Debuging_Character();

            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Is_Preemptive = false; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Greed = i;
            bM_Character_Team2[i].GetComponent<Character_Script>().Debuging_Character();
        }
      
    }

    void Battle(int phase)
    {
        if (bM_Team1_Is_Preemitive)
        {
            for (int i = 0; i < 5; i++)
            {
                if (bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Order == phase)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Range[j] == true &&
                           bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Greed == j)
                        {
                            bM_Character_Team1[i].GetComponent<Character_Script>().Character_Attack(bM_Character_Team2[i]);
                            Debug.Log("Team1 num " + i + 1 + " Attack " + bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Damage);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Order == phase)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Range[j] == true &&
                           bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Greed == j)
                        {
                            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Attack(bM_Character_Team1[i]);
                            Debug.Log("Team2 num " + i + 1 + " Attack " + bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Damage);
                        }
                    }
                }
            }
        }
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive;

        Calculate_Remain_HP();

        Debug.Log("Phase " + bM_Phase + " Team1 ����ü�� = " + bM_Remain_HP_Team1);
        Debug.Log("Phase " + bM_Phase + " Team2 ����ü�� = " + bM_Remain_HP_Team2);
    }

    void Calculate_Remain_HP()
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
