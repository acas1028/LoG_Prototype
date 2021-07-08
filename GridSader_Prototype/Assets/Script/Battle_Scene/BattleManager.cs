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


    public int bM_Phase { get; set; }
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

        bM_Phase = 0;

        // 내가 방장일 때만 나는 선공이다.
        if (PhotonNetwork.IsMasterClient)
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

 

    IEnumerator Running_Phase()
    {
        bM_Character_Battle_Start = true;
        while (bM_Phase < 6)
        {
            Battle(bM_Phase, bM_Team1_Is_Preemitive);

            yield return new WaitUntil(() => Check_Round_Finish());
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
      
            if (Team1CS.character_HP != 0)
                dummy++;
      
            Character_Script Team2CS = bM_Character_Team2[i].GetComponent<Character_Script>();
            GameObject Team2Sync = DataSync.GetComponent<Arrayed_Data>().team2[i];
            Team2CS.Copy_Character_Stat(Team2Sync);
            Team2CS.character_Num_Of_Grid = Reverse_Enemy(Team2CS.character_Num_Of_Grid);
            Team2CS.character_Team_Number = 2;
            Team2CS.Debuging_Character();
      
            if (Team2CS.character_HP != 0)
                dummy++;

            if (dummy == 10)
            {
                bM_Character_Setting_Finish = true;
            }
        }
    }

    bool Check_Round_Finish()
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
    void Character_Attack(GameObject attacker,GameObject[] enemy_Characters) //캐릭터 공격
    {
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.
        for (int j = 0; j < 9; j++)
        {
            if (attacker.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
            {
                foreach (GameObject enemy_Character in enemy_Characters)
                {
                    if (enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid == j + 1
                    && enemy_Character.GetComponent<Character_Script>().character_Is_Allive)
                    {
                        Debug.Log(attacker.GetComponent<Character_Script>().character_Num_Of_Grid + " attack " + enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid +
                            " by " + attacker.GetComponent<Character_Script>().character_Attack_Damage);
                        attacker.GetComponent<Character_Script>().Character_Attack(enemy_Character);
                    }
                }
                GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team2(j + 1);
            }
        }
    }

    void Enemy_Character_Attack(GameObject attacker, GameObject[] enemy_Characters)
    {
        for (int j = 0; j < 9; j++)
        {
            if (attacker.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
            {
                foreach (GameObject enemy_Character in enemy_Characters)
                {
                    if (enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid == Reverse_Enemy(j + 1)
                    && enemy_Character.GetComponent<Character_Script>().character_Is_Allive)
                    {
                        Debug.Log(attacker.GetComponent<Character_Script>().character_Num_Of_Grid + " attack " + enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid +
                            " by " + attacker.GetComponent<Character_Script>().character_Attack_Damage);
                        attacker.GetComponent<Character_Script>().Character_Attack(enemy_Character);
                    }
                }
                GridManager.GetComponent<DamagedGrid>().Create_Damaged_Grid_Team1(Reverse_Enemy(j + 1));
            }
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

    void Battle(int phase,bool team1_Is_Preemitive) // 선공,후공에 따라 배틀을 진행한다.
    {
        if (team1_Is_Preemitive) // 선공 판별  bM_Round = 0;
        {
            foreach(GameObject team1_Character in bM_Character_Team1)
            {
                if (team1_Character.GetComponent<Character_Script>().character_Attack_Order == phase)
                {
                    if (team1_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀1의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                    {
                        Character_Attack(team1_Character, bM_Character_Team2);
                        AlertMessage.SetActive(true);
                        AlertMessage.GetComponent<AlertMessage>().Attack(1, phase);
                        StartCoroutine(Counter(team1_Character, bM_Character_Team2));
                    }
                    else
                    {
                        AlertMessage.SetActive(true);
                        AlertMessage.GetComponent<AlertMessage>().CantAttack(1, phase);
                    }
                }
                team1_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        else  // bM_Round = 1;
        {
            foreach (GameObject team2_Character in bM_Character_Team2)
            {
                if (team2_Character.GetComponent<Character_Script>().character_Attack_Order == phase)
                {
                    if (team2_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀2의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                    {
                        Enemy_Character_Attack(team2_Character, bM_Character_Team1);
                        AlertMessage.SetActive(true);
                        AlertMessage.GetComponent<AlertMessage>().Attack(2, phase);
                        StartCoroutine(Counter(team2_Character, bM_Character_Team1));
                    }
                    else
                    {
                        AlertMessage.SetActive(true);
                        AlertMessage.GetComponent<AlertMessage>().CantAttack(2, phase);
                    }
                }
                team2_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }

        Calculate_Remain_HP();
        Calculate_Remain_Character();
        Destory_Red_Grid();

        Debug.Log("Phase " + bM_Phase + " Team1 남은체력 = " + bM_Remain_HP_Team1);
        Debug.Log("Phase " + bM_Phase + " Team2 남은체력 = " + bM_Remain_HP_Team2);

        bM_Round++;
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive;
        if (bM_Round == 2)
        {
            bM_Phase++;
            bM_Round = 0;
        }
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

