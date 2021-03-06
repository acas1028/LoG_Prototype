using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;

using Photon.Pun;
using Photon.Realtime;

public class BattleManager : MonoBehaviourPunCallbacks
{
    private UI_Manager uiManager;

    public AlertMessage alertMessage;
    public List<GameObject> bM_Character_Team1;
    public List<GameObject> bM_Character_Team2;
    public GameObject Character_Prefab;

    public float bM_Timegap { get { return 2.0f; } }
    public float bM_AttackTimegap { get { return 1.0f; } }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    public int bM_Phase { get; set; }

    int roundWinCount;
    int roundCount;

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


    IEnumerator Start()
    {
        uiManager = FindObjectOfType<UI_Manager>();

        object o_isPVE;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);

        if ((bool)o_isPVE == true) {
            bM_Team1_Is_Preemitive = true;
        }
        else if ((bool)o_isPVE == false) {
            if (Is_Preemptive())
                bM_Team1_Is_Preemitive = true;
            else
                bM_Team1_Is_Preemitive = false;
        }
        else {
            Debug.LogError("[Room CustomProperties] IsPVE 프로퍼티를 설정하지 않았습니다. 종료합니다.");
            yield return null;
        }

        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        bM_Phase = 0;
        roundWinCount = 0;
        roundCount = 0;

        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team1.Add(PhotonNetwork.Instantiate("Character_Action_Prefab", Vector3.zero, Quaternion.identity));

            if (PhotonNetwork.OfflineMode || (bool)o_isPVE == true)
                bM_Character_Team2.Add(Instantiate(Character_Prefab));
            // 온라인 환경에서 bM_Character_Team2 의 캐릭터 인스턴스는 Character_Action 스크립트의 Start 부분에서 등록된다.
        }

        yield return new WaitUntil(() => { return bM_Character_Team2.Count >= 5; });
        BM_Character_Setting();
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
        bool result;
        yield return StartCoroutine(SynergeManager.Instance.CheckSynerge(bM_Character_Team1));
        yield return StartCoroutine(SynergeManager.Instance.CheckSynerge(bM_Character_Team2));

        if(bM_Phase == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                result = SkillManager.Instance.AfterSetting(bM_Character_Team1[i], bM_Character_Team2);
                if (result)
                {
                    StartCoroutine(bM_Character_Team1[i].GetComponent<Character_Action>().SetCharacterColor("green"));
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }
            for(int i = 0; i < 5; i++)
            {
                result = SkillManager.Instance.AfterSetting(bM_Character_Team2[i], bM_Character_Team1);
                if (result)
                {
                    StartCoroutine(bM_Character_Team2[i].GetComponent<Character_Action>().SetCharacterColor("green"));
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }

            // 캐릭터 세팅 이후 스킬체크 과정
        }
        while (bM_Phase >= 0 && bM_Phase < 10)
        {
            bM_Phase++;
            yield return StartCoroutine(Battle(bM_Phase));
        }

        Debug.Log("게임 종료");
        Finish_Game();
    }

    void Round_Finish()
    {
        foreach(var team1 in bM_Character_Team1)
        {
            Character CCS = team1.GetComponent<Character>();

            CCS.character_is_Kill = 0;
            CCS.character_is_Killed = false;
            CCS.is_hit_this_turn = false;
        }

        foreach (var team2 in bM_Character_Team2)
        {
            Character CCS = team2.GetComponent<Character>();

            CCS.character_is_Kill = 0;
            CCS.character_is_Killed = false;
            CCS.is_hit_this_turn = false;
        }
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
            Team1CS.InitializeCharacterSprite();
            

            if (Team1CS.character_HP != 0)
                dummy++;
      
            Character Team2CS = bM_Character_Team2[i].GetComponent<Character>();
            GameObject Team2data = data.team2[i];
            Team2CS.Copy_Character_Stat(Team2data);
            Team2CS.character_Num_Of_Grid = Reverse_Enemy(Team2CS.character_Num_Of_Grid);
            Team2CS.character_Attack_Range = Enemy_AttackRange_Change(Team2CS);
            Team2CS.character_Team_Number = 2;
            Team2CS.character_Number = Team2CS.character_Attack_Order;
            Team2CS.InitializeCharacterSprite();
      
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

            Vector3 gridPos;

            gridPos = GridManager.Instance.Team1Character_Position[Team1CS.character_Num_Of_Grid - 1].transform.position;
            bM_Character_Team1[i].GetComponent<Character_Action>().SetStartPos(gridPos);
            gridPos = GridManager.Instance.Team2Character_Position[Team2CS.character_Num_Of_Grid - 1].transform.position;
            bM_Character_Team2[i].GetComponent<Character_Action>().SetStartPos(gridPos);
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

    IEnumerator Character_Attack(GameObject attacker, List<GameObject> enemy_Characters) //캐릭터 공격
    {
        bool result;
        Debug.LogFormat("<color=red>Character_Attack 코루틴 시작, 공격자: {0}</color>", attacker.GetComponent<Character>().character_Attack_Order);
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.

        result = SkillManager.Instance.BeforeAttack(attacker, enemy_Characters); // 스킬 발동 시점 체크
        if (result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(attacker.GetComponent<Character_Action>().SkillAttack());
        }

        for (int j = 0; j < 9; j++)
        {
            if (attacker.GetComponent<Character>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
            {
                if (attacker.GetComponent<Character>().character_Team_Number == 1)
                    GridManager.Instance.Create_Damaged_Grid_Team2(j + 1);
                else
                    GridManager.Instance.Create_Damaged_Grid_Team1(j + 1);

                foreach (GameObject enemy_Character in enemy_Characters)
                {
                    if (enemy_Character.GetComponent<Character>().character_Num_Of_Grid == j + 1)
                    {
                        StopCoroutine(attacker.GetComponent<Character_Action>().Attack(enemy_Character, false));
                        StartCoroutine(attacker.GetComponent<Character_Action>().Attack(enemy_Character, false));
                    }
                }
            }
        }

       
        alertMessage.gameObject.SetActive(true);
        alertMessage.Attack(attacker);
        yield return new WaitForSeconds(bM_Timegap);

        result = Check_Character_Dead();
        if(result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(Dead());
        }

        result = Check_Character_Hitted();
        if(result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(Hitted());
        }

        result = SkillManager.Instance.BeforeCounterAttack(attacker, enemy_Characters);
        if(result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(attacker.GetComponent<Character_Action>().SetCharacterColor("blue"));
        }
        // 아래 코루틴이 끝날 때 까지 대기(반격)
        yield return StartCoroutine(Counter(attacker, enemy_Characters));

        result = Check_Character_Dead();
        if (result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(Dead());
        }

        result = Check_Character_Hitted();
        if (result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(Hitted());
        }

        result = SkillManager.Instance.AfterCounterAttack(attacker, enemy_Characters); // 스킬 발동 시점 체크
        if (result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(attacker.GetComponent<Character_Action>().SkillAttack());
        }

        result = Check_Character_Dead();
        if (result)
        {
            alertMessage.gameObject.SetActive(false);
            yield return StartCoroutine(Dead());
        }

        Debug.LogFormat("<color=lightblue>Character_Attack 코루틴 종료, 공격자: {0}</color>", attacker.GetComponent<Character>().character_Attack_Order);
    }

    bool Check_Character_Dead()
    {
        foreach(var character in bM_Character_Team1)
        {
            if (character.GetComponent<Character>().character_is_Killed == true)
                return true;
        }

        foreach (var character in bM_Character_Team2)
        {
            if (character.GetComponent<Character>().character_is_Killed == true)
                return true;
        }

        return false;
    }
    IEnumerator Dead()
    {
        bool result;

        for(int i = 0; i < 5; i++)
        {
            Character Team1Script = bM_Character_Team1[i].GetComponent<Character>();

            if (Team1Script.character_is_Killed == true)
            {
                result = SkillManager.Instance.BeforeDead(bM_Character_Team1[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    yield return StartCoroutine((bM_Character_Team1[i].GetComponent<Character>() as Character_Action).SkillAttack());
                }

                result = SkillManager.Instance.CowardCheck(bM_Character_Team1[i]);
                if(result)
                {
                    alertMessage.gameObject.SetActive(false);
                    GameObject CowardCharacter = FindCowardCharacter(1);
                    yield return StartCoroutine((CowardCharacter.GetComponent<Character>() as Character_Action).SkillAttack());
                }

                result = SkillManager.Instance.SurvivorCheck(bM_Character_Team1[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    GameObject CowardCharacter = FindSurvivorCharacter(1);
                    yield return StartCoroutine((CowardCharacter.GetComponent<Character>() as Character_Action).SkillAttack());
                }
                // 분리해보자.
            }
            if(Team1Script.character_is_Killed == true)
            {
                StartCoroutine(bM_Character_Team1[i].GetComponent<Character_Action>().Dead());
                alertMessage.gameObject.SetActive(true);
                alertMessage.Dead(bM_Character_Team1[i]);
                yield return new WaitForSeconds(Instance.bM_Timegap);
            }

            Character Team2Script = bM_Character_Team2[i].GetComponent<Character>();

            if (Team2Script.character_is_Killed == true)
            {
                result = SkillManager.Instance.BeforeDead(bM_Character_Team2[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    yield return StartCoroutine((bM_Character_Team2[i].GetComponent<Character>() as Character_Action).SkillAttack());
                }

                result = SkillManager.Instance.CowardCheck(bM_Character_Team2[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    GameObject CowardCharacter = FindCowardCharacter(2);
                    yield return StartCoroutine((CowardCharacter.GetComponent<Character>() as Character_Action).SkillAttack());
                }

                result = SkillManager.Instance.SurvivorCheck(bM_Character_Team2[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    GameObject CowardCharacter = FindSurvivorCharacter(2);
                    yield return StartCoroutine((CowardCharacter.GetComponent<Character>() as Character_Action).SkillAttack());
                }
            }
            if (Team2Script.character_is_Killed == true)
            {
                StartCoroutine(bM_Character_Team2[i].GetComponent<Character_Action>().Dead());
                alertMessage.gameObject.SetActive(true);
                alertMessage.Dead(bM_Character_Team2[i]);
                yield return new WaitForSeconds(Instance.bM_Timegap);
            }
        }


    }

    GameObject FindCowardCharacter(int TeamNumber)
    {
        if(TeamNumber == 1)
        {
            for(int i = 0; i < 5; i++)
            {
                foreach (var team in bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Skill == CharacterSkill.Defense_Coward)
                    {
                        return team;
                    }
                }
            }
        }

        if (TeamNumber == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var team in bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Skill == CharacterSkill.Defense_Coward)
                    {
                        return team;
                    }
                }
            }
        }

        return null;
    }

    GameObject FindSurvivorCharacter(int TeamNumber)
    {
        if (TeamNumber == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var team in bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Skill == CharacterSkill.Balance_Survivor)
                    {
                        return team;
                    }
                }
            }
        }

        if (TeamNumber == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var team in bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Skill == CharacterSkill.Balance_Survivor)
                    {
                        return team;
                    }
                }
            }
        }

        return null;
    }


    bool Check_Character_Hitted()
    {
        foreach (var character in bM_Character_Team1)
        {
            if (character.GetComponent<Character>().is_hit_this_turn == true)
                return true;
        }

        foreach (var character in bM_Character_Team2)
        {
            if (character.GetComponent<Character>().is_hit_this_turn == true)
                return true;
        }
        return false;
    }

    IEnumerator Hitted()
    {
        bool result;

        for (int i = 0; i < 5; i++)
        {
            Character Team1Script = bM_Character_Team1[i].GetComponent<Character>();

            if (Team1Script.is_hit_this_turn == true)
            {
                result = SkillManager.Instance.AfterHitted(bM_Character_Team1[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    yield return StartCoroutine((bM_Character_Team1[i].GetComponent<Character>() as Character_Action).SkillAttack());
                }
            }

            Character Team2Script = bM_Character_Team2[i].GetComponent<Character>();

            if (Team2Script.is_hit_this_turn == true)
            {
                result = SkillManager.Instance.AfterHitted(bM_Character_Team2[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    yield return StartCoroutine((bM_Character_Team2[i].GetComponent<Character>() as Character_Action).SkillAttack());
                }
            }
        }
    }

    IEnumerator Counter(GameObject attacker, List<GameObject> enemy_Characters)
    {
        bool result;
        for(int i = 0; i < 5; i++)
        {
            Character EnemyScript = enemy_Characters[i].GetComponent<Character>();
            if (EnemyScript.character_Counter == true && EnemyScript.character_Is_Allive == true)
            {
                result = SkillManager.Instance.CounterAttacking(attacker, enemy_Characters[i]);
                if (result)
                {
                    alertMessage.gameObject.SetActive(false);
                    yield return StartCoroutine(enemy_Characters[i].GetComponent<Character_Action>().SkillAttack());
                }
                else
                {
                    StartCoroutine(enemy_Characters[i].GetComponent<Character_Action>().Attack(attacker, true));
                    alertMessage.gameObject.SetActive(true);
                    alertMessage.Counter(enemy_Characters[i]);
                }
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
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }
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
                    yield return new WaitForSeconds(bM_Timegap);
                }
            }
        }
        Round_Finish();
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

    public int Reverse_Enemy(int num) // 적 공격 시 공격범위를 좌우반전시킴.
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

    bool GetServerVariables()
    {
        object o_roundCount;
        object o_roundWinCount;

        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoundCount", out o_roundCount);
        if (o_roundCount == null)
        {
            Debug.LogError("Failed to get 'RoundCount' from server");
            return false;
        }
        roundCount = (int)o_roundCount;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("RoundWinCount", out o_roundWinCount);
        if (o_roundWinCount == null)
        {
            Debug.LogError("Failed to get 'RoundWinCount' from server");
            return false;
        }
        roundWinCount = (int)o_roundWinCount;

        return true;
    }

    void Finish_Game()
    {
        if (GetServerVariables() == false)
        {
            Debug.LogError("Failed to get server variables so that can't implement finishing match");
            return;
        }    

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
            roundWinCount++;
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
                roundWinCount++;
            }
            else
            {
                alertMessage.gameObject.SetActive(true);
                alertMessage.Draw();
            }
        }

        object o_isPVE;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsPVE", out o_isPVE);

        if ((bool)o_isPVE) {
            uiManager.ShowMatchResult(roundWinCount > 0 ? true : false);
            Invoke("LoadArraymentScene", 4f);
            return;
        }

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable() { { "RoundWinCount", roundWinCount } };
        PhotonNetwork.SetPlayerCustomProperties(table);

        if (PhotonNetwork.IsMasterClient) {
            table = new ExitGames.Client.Photon.Hashtable() { { "RoundCount", roundCount + 1 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(table);
        }
    }

    void LoadArraymentScene()
    {
        PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.ARRAYMENT_SCENE);
    }

    void LoadPveScene() {
        PhotonNetwork.LoadLevel((int)Move_Scene.ENUM_SCENE.PVE_SCENE);
    }

    #region 포톤 콜백 함수
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        uiManager.ShowMatchResult(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {
        object o_roundWinCount;
        targetPlayer.CustomProperties.TryGetValue("RoundWinCount", out o_roundWinCount);

        if ((int)o_roundWinCount >= 2) {
            uiManager.ShowMatchResult(roundWinCount >= 2 ? true : false);
            Invoke("LoadArraymentScene", 4f);
        }
    }
    #endregion

}

