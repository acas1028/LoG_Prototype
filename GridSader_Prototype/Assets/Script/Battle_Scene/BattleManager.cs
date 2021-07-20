using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BattleManager : MonoBehaviourPunCallbacks
{
    public GameObject GridManager;
    public GameObject[] bM_Character_Team1;
    public GameObject[] bM_Character_Team2;
    public GameObject Character_Prefab;
    public GameObject AlertMessage;
    public GameObject DataSync;

    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    public int bM_Round { get; set; }

    public bool bM_Character_Setting_Finish { get; set; }

    public bool bM_Character_Battle_Start { get; set; }
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

    // Start is called before the first frame update


    void Start()
    {
        // 내가 방장일 때만 나는 선공이다.
        if (Is_Preemptive())
            bM_Team1_Is_Preemitive = true;
        else
            bM_Team1_Is_Preemitive = false;

        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        bM_Round = 0;
        bM_Character_Setting_Finish = false;
        bM_Character_Battle_Start = false;

        bM_Character_Team1 = new GameObject[5];
        bM_Character_Team2 = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team1[i] = Instantiate(Character_Prefab);
            bM_Character_Team2[i] = Instantiate(Character_Prefab);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(bM_Character_Setting_Finish == false && bM_Character_Battle_Start == false)
            BM_Character_Setting();

        if (bM_Character_Setting_Finish == true && bM_Character_Battle_Start == false)
            StartCoroutine(Running_Phase());


    }

    public bool Is_Preemptive()
    {
        object o_is_preemptive;
        bool is_preemptive;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPreemptive", out o_is_preemptive);
        is_preemptive = (bool)o_is_preemptive;

        return is_preemptive;
    }

    IEnumerator Running_Phase()
    {
        bM_Character_Battle_Start = true;
        while (bM_Round < 11)
        {
            Battle(bM_Round);

            yield return new WaitUntil(() => Check_Skill_Finish());
            yield return new WaitUntil(() => Check_Counter_Finish());
            Round_Finish();
            yield return new WaitForSeconds(2.0f);
        }

        Finish_Game();
    }

    void BM_Character_Setting()
    {
        int dummy = 0;
        DataSync = GameObject.FindGameObjectWithTag("Character_Sync");

        for (int i = 0; i < 5; i++)
        {
            Character_Script Team1CS = bM_Character_Team1[i].GetComponent<Character_Script>();
            GameObject Team1Sync = DataSync.GetComponent<Arrayed_Data>().team1[i];
            Team1CS.Copy_Character_Stat(Team1Sync);
            Team1CS.character_Team_Number = 1;
            Team1CS.Debuging_Character();

            Debug.Log("Team1" + Team1CS.character_Skill);
      
            if (Team1CS.character_HP != 0)
                dummy++;
      
            Character_Script Team2CS = bM_Character_Team2[i].GetComponent<Character_Script>();
            GameObject Team2Sync = DataSync.GetComponent<Arrayed_Data>().team2[i];
            Team2CS.Copy_Character_Stat(Team2Sync);
            Team2CS.character_Num_Of_Grid = Reverse_Enemy(Team2CS.character_Num_Of_Grid);
            Team2CS.character_Attack_Range = Enemy_AttackRange_Change(Team2CS);
            Team2CS.character_Team_Number = 2;
            Team2CS.Debuging_Character();
      
            if (Team2CS.character_HP != 0)
                dummy++;

            if (bM_Team1_Is_Preemitive)
                Team2CS.character_Attack_Order *= 2;
            else
                Team1CS.character_Attack_Order *= 2;
        }

        if (dummy == 10)
        {
            bM_Character_Setting_Finish = true;
        }


    }

    bool[] Enemy_AttackRange_Change(Character_Script Team2CS)
    {
        bool[] dummy = new bool[9];

        for(int i = 0; i < 9; i++)
        {
            dummy[i] = false;
            dummy[i] = Team2CS.character_Attack_Range[Reverse_Enemy(i+1) - 1];
        }
        return dummy;
    }

    bool Check_Counter_Finish()
    {
        foreach(GameObject team1 in bM_Character_Team1)
        {
            if (team1.GetComponent<Character_Script>().character_Counter == true)
                return false;
        }

        foreach(GameObject team2 in bM_Character_Team2)
        {
            if (team2.GetComponent<Character_Script>().character_Counter == true)
                return false;
        }

        return true;
    }

    bool Check_Skill_Finish()
    {
        foreach(GameObject team1 in bM_Character_Team1)
        {
            if (team1.GetComponent<Character_Script>().character_Activate_Skill == true)
                return false;
        }
        foreach (GameObject team2 in bM_Character_Team2)
        {
            if (team2.GetComponent<Character_Script>().character_Activate_Skill == true)
                return false;
        }

        return true;
    }

    void Round_Finish()
    {
        foreach(var team1 in bM_Character_Team1)
        {
            Character_Script Team1 = team1.GetComponent<Character_Script>();
            Team1.character_Activate_Skill = false;
            Team1.character_Counter = false;
            Team1.character_is_Kill = 0;
        }

        foreach (var team1 in bM_Character_Team2)
        {
            Character_Script Team1 = team1.GetComponent<Character_Script>();
            Team1.character_Activate_Skill = false;
            Team1.character_Counter = false;
            Team1.character_is_Kill = 0;
        }

        bM_Round++;
    }
    IEnumerator Character_Attack(GameObject attacker,GameObject[] enemy_Characters) //캐릭터 공격
    {
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.
        int attack_Count = attacker.GetComponent<Character_Script>().character_Attack_Count;

        while (attack_Count > 0)
        {
            for (int j = 0; j < 9; j++)
            {
                if (attacker.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
                {
                    foreach (GameObject enemy_Character in enemy_Characters)
                    {
                        if (enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid == j + 1
                        && enemy_Character.GetComponent<Character_Script>().character_Is_Allive)
                        {
                            attacker.GetComponent<Character_Script>().Character_Attack(enemy_Character);
                        }
                    }
                    if (attacker.GetComponent<Character_Script>().character_Team_Number == 1)
                        GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team2(j + 1);
                    else
                        GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team1(j + 1);
                }
            }
            AlertMessage.SetActive(true);
            AlertMessage.GetComponent<AlertMessage>().Attack(attacker.GetComponent<Character_Script>().character_Team_Number, attacker.GetComponent<Character_Script>().character_Attack_Order);
            attack_Count--;

            SkillManager.Instance.AfterAttack(attacker, enemy_Characters); // 스킬 발동 시점 체크

            yield return new WaitUntil(() => Check_Skill_Finish());

            StartCoroutine(Counter(attacker, enemy_Characters));

            yield return new WaitUntil(() => Check_Counter_Finish());

            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator Counter(GameObject attacker, GameObject[] enemy_Characters)
    {
        for(int i = 0; i < 5; i++)
        {
            Character_Script EnemyScript = enemy_Characters[i].GetComponent<Character_Script>();
            if (EnemyScript.character_Counter == true)
            {
                yield return new WaitForSeconds(2.0f);

                enemy_Characters[i].GetComponent<Character_Script>().Character_Counter_Attack(attacker);
                AlertMessage.SetActive(true);
                AlertMessage.GetComponent<AlertMessage>().Counter(EnemyScript.character_Team_Number, EnemyScript.character_Attack_Order);
            }
        }
    }

    void Battle(int Round) // 선공,후공에 따라 배틀을 진행한다.
    {
        
        foreach(GameObject team1_Character in bM_Character_Team1)
        {
            if (team1_Character.GetComponent<Character_Script>().character_Attack_Order == Round)
            {
                if (team1_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀1의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    StartCoroutine(Character_Attack(team1_Character, bM_Character_Team2));
                }
                else
                {
                    AlertMessage.SetActive(true);
                    AlertMessage.GetComponent<AlertMessage>().CantAttack(1, Round);
                }
            }
            team1_Character.GetComponent<Character_Script>().Debuging_Character();
        }
        
        foreach (GameObject team2_Character in bM_Character_Team2)
        {
            if (team2_Character.GetComponent<Character_Script>().character_Attack_Order == Round)
            {
                if (team2_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀2의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    StartCoroutine(Character_Attack(team2_Character, bM_Character_Team1));
                }
                else
                {
                    AlertMessage.SetActive(true);
                    AlertMessage.GetComponent<AlertMessage>().CantAttack(2, Round);
                }
            }
            team2_Character.GetComponent<Character_Script>().Debuging_Character();
        }
 

        Calculate_Remain_HP();
        Calculate_Remain_Character();
        Destory_Red_Grid();

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

    void Calculate_Remain_Character()
    {
        bM_Remain_Character_Team1 = 5;
        bM_Remain_Character_Team2 = 5;

        foreach(var Team1 in bM_Character_Team1)
        {
            if(Team1.GetComponent<Character_Script>().character_HP <= 0)
            {
                bM_Remain_Character_Team1--;
            }
        }

        foreach (var Team2 in bM_Character_Team2)
        {
            if (Team2.GetComponent<Character_Script>().character_HP <= 0)
            {
                bM_Remain_Character_Team2--;
            }
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

    void Finish_Game()
    {
        if(bM_Remain_Character_Team1 < bM_Remain_Character_Team2)
        {
            // 패배

            AlertMessage.SetActive(true);
            AlertMessage.GetComponent<AlertMessage>().Lose();
        }

        if(bM_Remain_Character_Team2 < bM_Remain_Character_Team1)
        {
            // 승리

            AlertMessage.SetActive(true);
            AlertMessage.GetComponent<AlertMessage>().Win();
        }

        if(bM_Remain_Character_Team1 == bM_Remain_Character_Team2)
        {
            // 체력 비교

            if (bM_Remain_HP_Team1 < bM_Remain_HP_Team2)
            {
                AlertMessage.SetActive(true);
                AlertMessage.GetComponent<AlertMessage>().Lose();
            }

            if(bM_Remain_HP_Team2 < bM_Remain_HP_Team1)
            {
                AlertMessage.SetActive(true);
                AlertMessage.GetComponent<AlertMessage>().Win();
            }
        }
    }
}

