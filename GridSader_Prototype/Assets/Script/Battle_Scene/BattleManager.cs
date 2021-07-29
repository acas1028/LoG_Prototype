using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BattleManager : MonoBehaviourPunCallbacks
{
    public GridManager gridManager;
    public AlertMessage alertMessage;
    public GameObject[] bM_Character_Team1;
    public GameObject[] bM_Character_Team2;
    public GameObject Character_Prefab;

    public float bM_Timegap { get { return 2.0f; } }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    public int bM_Round { get; set; }

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
        if (Is_Preemptive())
            bM_Team1_Is_Preemitive = true;
        else
            bM_Team1_Is_Preemitive = false;

        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        bM_Round = 0;

        bM_Character_Team1 = new GameObject[5];
        bM_Character_Team2 = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team1[i] = Instantiate(Character_Prefab);
            bM_Character_Team2[i] = Instantiate(Character_Prefab);
        }

        BM_Character_Setting();
        StartCoroutine(Running_Phase());
    }


    // Update is called once per frame
    void Update()
    {
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
        bool result;
        if(bM_Round == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                result = SkillManager.Instance.AfterSetting(bM_Character_Team1[i]);
                if (result)
                {
                    StartCoroutine(bM_Character_Team1[i].GetComponent<Character_Action>().SetCharacterColor("green"));
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }
            for(int i = 0; i < 5; i++)
            {
                result = SkillManager.Instance.AfterSetting(bM_Character_Team2[i]);
                if (result)
                {
                    StartCoroutine(bM_Character_Team2[i].GetComponent<Character_Action>().SetCharacterColor("green"));
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                bM_Character_Team1[i].GetComponent<Character>().Debuging_Character();
                bM_Character_Team2[i].GetComponent<Character>().Debuging_Character();
            }

            // 캐릭터 세팅 이후 스킬체크 과정
        }
        while (bM_Round >= 0 && bM_Round < 10)
        {
            bM_Round++;
            yield return StartCoroutine(Battle(bM_Round));
        }

        Debug.Log("게임 종료");
        Finish_Game();
    }

    void BM_Character_Setting()
    {
        int dummy = 0;
        Arrayed_Data data = Arrayed_Data.instance;
        if (data == null)
            Debug.LogError("Arrayed_Data 인스턴스가 없습니다.");

        for (int i = 0; i < 5; i++)
        {
            Character Team1CS = bM_Character_Team1[i].GetComponent<Character>();
            GameObject Team1data = data.team1[i];
            Team1CS.Copy_Character_Stat(Team1data);
            Team1CS.character_Team_Number = 1;
            Team1CS.character_Number = Team1CS.character_Attack_Order;
            Team1CS.Debuging_Character();

            if (Team1CS.character_HP != 0)
                dummy++;
      
            Character Team2CS = bM_Character_Team2[i].GetComponent<Character>();
            GameObject Team2data = data.team2[i];
            Team2CS.Copy_Character_Stat(Team2data);
            Team2CS.character_Num_Of_Grid = Reverse_Enemy(Team2CS.character_Num_Of_Grid);
            Team2CS.character_Attack_Range = Enemy_AttackRange_Change(Team2CS);
            Team2CS.character_Team_Number = 2;
            Team2CS.character_Number = Team2CS.character_Attack_Order;
            Team2CS.Debuging_Character();
      
            if (Team2CS.character_HP != 0)
                dummy++;

            if (bM_Team1_Is_Preemitive)
            {
                Team1CS.character_Attack_Order = Team1CS.character_Attack_Order * 2 - 1;
                Team2CS.character_Attack_Order = Team2CS.character_Attack_Order * 2;
            }
            else
            {
                Team1CS.character_Attack_Order = Team1CS.character_Attack_Order * 2;
                Team2CS.character_Attack_Order = Team2CS.character_Attack_Order * 2 - 1;
            }

            int gridnum = bM_Character_Team1[i].GetComponent<Character>().character_Num_Of_Grid;
            bM_Character_Team1[i].transform.position = gridManager.Team1Map[gridnum - 1].transform.position;
            gridnum = bM_Character_Team2[i].GetComponent<Character>().character_Num_Of_Grid;
            bM_Character_Team2[i].transform.position = gridManager.Team2Map[gridnum - 1].transform.position;
        }
    }

    bool[] Enemy_AttackRange_Change(Character Team2CS)
    {
        bool[] dummy = new bool[9];

        for(int i = 0; i < 9; i++)
        {
            dummy[i] = false;
            dummy[i] = Team2CS.character_Attack_Range[Reverse_Enemy(i+1) - 1];
        }
        return dummy;
    }

    IEnumerator Character_Attack(GameObject attacker,GameObject[] enemy_Characters) //캐릭터 공격
    {
        bool result;
        Debug.LogFormat("<color=red>Character_Attack 코루틴 시작, 공격자: {0}</color>", attacker.GetComponent<Character>().character_Attack_Order);
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.

        result = SkillManager.Instance.BeforeAttack(attacker, enemy_Characters); // 스킬 발동 시점 체크
        if (result)
        {
            StartCoroutine(attacker.GetComponent<Character_Action>().SetCharacterColor("blue"));
            yield return new WaitForSeconds(bM_Timegap);
        }

        for (int j = 0; j < 9; j++)
        {
            if (attacker.GetComponent<Character>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
            {
                foreach (GameObject enemy_Character in enemy_Characters)
                {
                    if (enemy_Character.GetComponent<Character>().character_Num_Of_Grid == j + 1
                    && enemy_Character.GetComponent<Character>().character_Is_Allive)
                    {
                        attacker.GetComponent<Character_Action>().Character_Attack(enemy_Character);
                    }
                }
                if (attacker.GetComponent<Character>().character_Team_Number == 1)
                    gridManager.Create_Damaged_Grid_Team2(j + 1);
                else
                    gridManager.Create_Damaged_Grid_Team1(j + 1);
            }
        }
        alertMessage.gameObject.SetActive(true);
        alertMessage.Attack(attacker);

        yield return new WaitForSeconds(bM_Timegap);

        // 아래 코루틴이 끝날 때 까지 대기(반격)
        yield return StartCoroutine(Counter(attacker, enemy_Characters));

        result = SkillManager.Instance.AfterAttack(attacker, enemy_Characters); // 스킬 발동 시점 체크
        if (result)
        {
            StartCoroutine(attacker.GetComponent<Character_Action>().SetCharacterColor("blue"));
            yield return new WaitForSeconds(bM_Timegap);
        }

        Debug.LogFormat("<color=lightblue>Character_Attack 코루틴 종료, 공격자: {0}</color>", attacker.GetComponent<Character>().character_Attack_Order);
    }

    IEnumerator Counter(GameObject attacker, GameObject[] enemy_Characters)
    {
        for(int i = 0; i < 5; i++)
        {
            Character EnemyScript = enemy_Characters[i].GetComponent<Character>();
            if (EnemyScript.character_Counter == true && EnemyScript.character_Is_Allive == true)
            {
                Debug.Log("반격");
                enemy_Characters[i].GetComponent<Character_Action>().Character_Counter_Attack(attacker);
                alertMessage.gameObject.SetActive(true);
                alertMessage.Counter(enemy_Characters[i]);

                yield return new WaitForSeconds(Instance.bM_Timegap);
            }
        }
    }

    IEnumerator Battle(int Round) // 선공,후공에 따라 배틀을 진행한다.
    {
        Debug.LogFormat("<color=#FF69B4> Round: {0}</color>", Round);
        foreach(GameObject team1_Character in bM_Character_Team1)
        {
            if (team1_Character.GetComponent<Character>().character_Attack_Order == Round)
            {
                if (team1_Character.GetComponent<Character>().character_Is_Allive) // 팀1의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    yield return StartCoroutine(Character_Attack(team1_Character, bM_Character_Team2));
                }
                else
                {
                    alertMessage.gameObject.SetActive(true);
                    alertMessage.CantAttack(team1_Character);
                    yield return new WaitUntil(() => !alertMessage.gameObject.activeSelf);
                }
            }
            team1_Character.GetComponent<Character>().Debuging_Character();
        }
        
        foreach (GameObject team2_Character in bM_Character_Team2)
        {
            if (team2_Character.GetComponent<Character>().character_Attack_Order == Round)
            {
                if (team2_Character.GetComponent<Character>().character_Is_Allive) // 팀2의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    yield return StartCoroutine(Character_Attack(team2_Character, bM_Character_Team1));
                }
                else
                {
                    alertMessage.gameObject.SetActive(true);
                    alertMessage.GetComponent<AlertMessage>().CantAttack(team2_Character);
                    yield return new WaitUntil(() => !alertMessage.gameObject.activeSelf);
                }
            }
            team2_Character.GetComponent<Character>().Debuging_Character();
        }
    }

    void Calculate_Remain_HP() //남은 체력 계산
    {
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        for (int i = 0; i < 5; i++)
        {
            bM_Remain_HP_Team1 += bM_Character_Team1[i].GetComponent<Character>().character_HP;
            bM_Remain_HP_Team2 += bM_Character_Team2[i].GetComponent<Character>().character_HP;
        }
    }

    void Calculate_Remain_Character()
    {
        bM_Remain_Character_Team1 = 5;
        bM_Remain_Character_Team2 = 5;

        foreach(var Team1 in bM_Character_Team1)
        {
            if(Team1.GetComponent<Character>().character_HP <= 0)
            {
                bM_Remain_Character_Team1--;
            }
        }

        foreach (var Team2 in bM_Character_Team2)
        {
            if (Team2.GetComponent<Character>().character_HP <= 0)
            {
                bM_Remain_Character_Team2--;
            }
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
        Calculate_Remain_HP();
        Calculate_Remain_Character();

        if (bM_Remain_Character_Team1 < bM_Remain_Character_Team2)
        {
            // 패배

            alertMessage.gameObject.SetActive(true);
            alertMessage.Lose();
        }
        else if(bM_Remain_Character_Team2 < bM_Remain_Character_Team1)
        {
            // 승리

            alertMessage.gameObject.SetActive(true);
            alertMessage.Win();
        }
        else
        {
            // 체력 비교

            if (bM_Remain_HP_Team1 < bM_Remain_HP_Team2)
            {
                alertMessage.gameObject.SetActive(true);
                alertMessage.Lose();
            }
            else if(bM_Remain_HP_Team2 < bM_Remain_HP_Team1)
            {
                alertMessage.gameObject.SetActive(true);
                alertMessage.Win();
            }
            else
            {
                alertMessage.gameObject.SetActive(true);
                alertMessage.Message("비겼습니다!");
            }
        }
    }
}

