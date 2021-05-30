using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static BattleManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static BattleManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug_Delay += Time.deltaTime;

        if (Debug_Delay > 2)
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
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1;
            bM_Character_Team1[i].GetComponent<Character_Script>().Debuging_Character();

            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Is_Preemptive = false; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1;
            bM_Character_Team2[i].GetComponent<Character_Script>().Debuging_Character();
        }
      
    }
    void Character_Attack(GameObject attacker,GameObject[] enemy_Characters,int attacked_Grid) //캐릭터 공격
    {
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
    void Battle(int phase) // 선공,후공에 따라 배틀을 진행한다.
    {
        if (bM_Team1_Is_Preemitive)
        {
            foreach(GameObject team1_Character in bM_Character_Team1)
            {
                if(team1_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                && team1_Character.GetComponent<Character_Script>().character_Is_Allive)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (team1_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true)
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
                 && team2_Character.GetComponent<Character_Script>().character_Is_Allive)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (team2_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true)
                            Character_Attack(team2_Character, bM_Character_Team1, j + 1);
                    }
                }
                team2_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive;

        Calculate_Remain_HP();

        Debug.Log("Phase " + bM_Phase + " Team1 남은체력 = " + bM_Remain_HP_Team1);
        Debug.Log("Phase " + bM_Phase + " Team2 남은체력 = " + bM_Remain_HP_Team2);
    }

    void Calculate_Remain_HP() //남은 체력 계산
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
