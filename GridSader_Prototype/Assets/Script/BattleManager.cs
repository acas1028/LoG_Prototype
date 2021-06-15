using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleManager : MonoBehaviourPunCallbacks
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

    public GameObject GridManager;
    public GameObject[] bM_Character_Team1;
    public GameObject[] bM_Character_Team2;
    public GameObject Character_Prefab;
    public int bM_Phase { get; set; }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    public int bM_Round { get; set; }

    // Start is called before the first frame update
    void Start()
    {

        bM_Phase = 0;
        bM_Team1_Is_Preemitive = true;
        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        bM_Round = 0;

        bM_Character_Team1 = new GameObject[5];
        bM_Character_Team2 = new GameObject[5];

        for(int i = 0; i < 5; i++)
        {
            bM_Character_Team1[i] = Instantiate(Character_Prefab);
            bM_Character_Team2[i] = Instantiate(Character_Prefab);
        }
    }


    // Update is called once per frame
    void Update()
    {
        StartBattle();
    }

    void StartBattle()
    {
        if (bM_Phase != 0) return;

        BM_Character_Setting();
        bM_Phase++;
        StartCoroutine(Running_Phase());    
    }

    IEnumerator Running_Phase()
    {
        while (bM_Phase < 6)
        {
            Battle(bM_Phase, bM_Team1_Is_Preemitive);
            bM_Round++;
            bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive;
            if(bM_Round == 2)
            {
                bM_Phase++;
                bM_Round = 0;
            }    
            yield return new WaitForSeconds(3.0f);
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
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Grid = Reverse_Enemy(i + 1); // Debuging // 좌우반전
            bM_Character_Team2[i].GetComponent<Character_Script>().Debuging_Character();
        }
        
    }

    void Character_Attack(GameObject attacker,GameObject[] enemy_Characters,int attacked_Grid) //캐릭터 공격
    {
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.
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
    void Battle(int phase,bool team1_Is_Preemitive) // 선공,후공에 따라 배틀을 진행한다.
    {
        if (team1_Is_Preemitive) // 선공 판별 
        {
            foreach(GameObject team1_Character in bM_Character_Team1)
            {
                if(team1_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                && team1_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀1의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (team1_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
                        {
                            Character_Attack(team1_Character, bM_Character_Team2, j + 1);
                            GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team2(j+1);
                        }
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
                 && team2_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀2의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (team2_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
                        {
                            Character_Attack(team2_Character, bM_Character_Team1, Reverse_Enemy(j+1)); // 좌우반전
                            GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team1(Reverse_Enemy(j + 1)); // 좌우반전
                        }
                    }
                }
                team2_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }

        Calculate_Remain_HP();
        Destory_Red_Grid();

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

    void Destory_Red_Grid()
    {
        GameObject[] red_Grid = GameObject.FindGameObjectsWithTag("BattleScene_Damaged_Grid");

        foreach(var redGrid in red_Grid)
        {
            Destroy(redGrid, 2.0f);
        }
    }

    int Reverse_Enemy(int num) // 적 공격 시 공격범위를 좌우반전시킴.
    {
        int dummy = 0;
        switch(num)
        {
            case 1:
                dummy = 3;
                break;
            case 2:
                dummy = 2;
                break;
            case 3:
                dummy = 1;
                break;
            case 4:
                dummy = 6;
                break;
            case 5:
                dummy = 5;
                break;
            case 6:
                dummy = 4;
                break;
            case 7:
                dummy = 9;
                break;
            case 8:
                dummy = 8;
                break;
            case 9:
                dummy = 7;
                break;
        }
        return dummy;
    }
}

