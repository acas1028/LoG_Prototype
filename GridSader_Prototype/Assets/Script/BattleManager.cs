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
        for(int i = 0; i < 5; i++)
        {
            Instantiate(bM_Character_Team1[i]);
        }
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bM_Character_Team2[i]);
        }
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
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Speed = i + 1; // Debuging
            Debug.Log("team1 " + i + bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Speed);
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Is_Preemptive = true; // Debuging
        }
        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Speed = i + 1; // Debuging
            Debug.Log("team2 " + i + bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Speed);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Is_Preemptive = false; // Debuging
        }
    }

    void Battle(int phase)
    {
        if (bM_Team1_Is_Preemitive)
        {
            for (int i = 0; i < 5; i++)
            {
                if (bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Speed == phase)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Range[j] == true ||
                           bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Greed == j)
                        {
                            bM_Character_Team1[i].GetComponent<Character_Script>().Character_Attack(bM_Character_Team2[i]);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Speed == phase)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Range[j] == true ||
                           bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Greed == j)
                        {
                            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Attack(bM_Character_Team1[i]);
                        }
                    }
                }
            }
        }
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive;
        bM_Phase++;
    }
}
