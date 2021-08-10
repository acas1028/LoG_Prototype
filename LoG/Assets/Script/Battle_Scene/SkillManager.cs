using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject skillmessage;

    private static SkillManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static SkillManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SkillManager)) as SkillManager;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AfterSetting(GameObject character,GameObject[] enemys)
    {
        bool result;
        Character CCS = character.GetComponent<Character>();

        if(CCS.character_Skill == Character.Skill.Attack_Confidence)
        {
            result = Skill_Attack_Confidence(character);
            if (result) return true;
        }

        if (CCS.character_Skill == Character.Skill.Attack_DivineShield)
        {
            result = Skill_Attack_DivineShield(character);
            if (result) return true;
        }

        if (CCS.character_Skill == Character.Skill.Balance_GBGH)
        {
            result = Skill_Balanced_GBGH(character);
            if (result) return true;
        }

        if(CCS.character_Skill == Character.Skill.Balance_Blessing)
        {
            result = Skill_Balanced_Blessing(character);
            if (result) return true;
        }

        if (CCS.character_Skill == Character.Skill.Balance_Smoke)
        {
            result = Skill_Balanced_Smoke(character,enemys);
            if (result) return true;
        }

        return false;
    }
    public bool BeforeAttack(GameObject attacker,GameObject[] Damaged)
    {
        bool result;
        Character ACS = attacker.GetComponent<Character>();

        if(ACS.character_Skill == Character.Skill.Attack_Ranger)
        {
            result = SKill_Attack_Ranger(attacker,Damaged);
            if (result) return true;
        }

        if(ACS.character_Skill == Character.Skill.Attack_Struggle)
        {
            result = Skill_Attack_Struggle(attacker);
            if (result) return true;
        }

        if(ACS.character_Skill == Character.Skill.Attack_ArmorPiercer)
        {
            result = Skill_Attack_ArmorPiercer(attacker,Damaged);
            if (result) return true;
        }

        return false;
    }
    public bool AfterCounterAttack(GameObject attacker,GameObject[] Damaged)
    {
        bool result;
        Character ACS = attacker.GetComponent<Character>();

        if(ACS.character_Skill == Character.Skill.Attack_Executioner)
        {
            result = Skill_Attack_Executioner(attacker);
            if (result) return true;
        }

        return false;
    }
    public bool BeforeCounterAttack(GameObject attacker,GameObject[] Damaged)
    {
        bool result;
        Character ACS = attacker.GetComponent<Character>();


        if (ACS.character_Skill == Character.Skill.Defense_Disarm)
        {
            result = Skill_Defender_Disarm(attacker, Damaged);
            if (result) return true;
        }

        return false;
    }
    public bool AfterDead(GameObject deadCharacter)
    {
        bool result;
        Character DCS = deadCharacter.GetComponent<Character>();

        if(DCS.character_Skill == Character.Skill.Balance_Curse)
        {
            result = Skill_Balanced_Curse(deadCharacter);
            if (result) return true;
        }

        if(DCS.character_Skill == Character.Skill.Balance_DestinyBond)
        {
            result = Skill_Balanced_DestinyBond(deadCharacter);
            if (result) return true;
        }
        return false;
    }

    public bool CounterAttacking(GameObject attacker,GameObject counterAttacker)
    {
        bool result;
        if(counterAttacker.GetComponent<Character>().character_Skill == Character.Skill.Balance_WideCounter)
        {
            result = Skill_Balanced_WideCounter(attacker, counterAttacker);
            if (result) return true;
        }
        return false;
    }
    // 공격형 스킬
    bool Skill_Attack_Confidence(GameObject character) // 자신감
    {
        Character ACS = character.GetComponent<Character>();

        bool check = false;
        switch (ACS.character_Num_Of_Grid)
        {
            case 1:
                check = Check_Arround(ACS, 2, 4);
                break;
            case 2:
                check = Check_Arround(ACS, 1, 3, 5);
                break;
            case 3:
                check = Check_Arround(ACS, 2, 6);
                break;
            case 4:
                check = Check_Arround(ACS, 1, 5, 7);
                break;
            case 5:
                check = Check_Arround(ACS, 2, 4, 6, 8);
                break;
            case 6:
                check = Check_Arround(ACS, 3, 5, 9);
                break;
            case 7:
                check = Check_Arround(ACS, 4, 8);
                break;
            case 8:
                check = Check_Arround(ACS, 5, 7, 9);
                break;
            case 9:
                check = Check_Arround(ACS, 6, 8);
                break;
        }

        if(!check)
        {
            return false;
        }

        //스킬 발동 체크
        ACS.character_Attack_Damage += 40;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"자신감");

        return true;
    }
    bool Check_Arround(Character ACS,int num1,int num2,int num3,int num4)
    {
        int dum = 0;

        if(ACS.character_Team_Number == 1)
        {
            foreach(var team1 in BattleManager.Instance.bM_Character_Team1)
            {
                Debug.Log("체크 어라운드 : " + dum);
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
                if (CCS.character_Num_Of_Grid == num3) dum++;
                if (CCS.character_Num_Of_Grid == num4) dum++;
            }
        }
        
        if(ACS.character_Team_Number == 2)
        {
            foreach (var team1 in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
                if (CCS.character_Num_Of_Grid == num3) dum++;
                if (CCS.character_Num_Of_Grid == num4) dum++;
            }
        }

        if (dum >= 2)
            return true;
        return false;
    }
    bool Check_Arround(Character ACS,int num1, int num2, int num3)
    {
        int dum = 0;

        if (ACS.character_Team_Number == 1)
        {
            foreach (var team1 in BattleManager.Instance.bM_Character_Team1)
            {
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
                if (CCS.character_Num_Of_Grid == num3) dum++;
            }
        }

        if (ACS.character_Team_Number == 2)
        {
            foreach (var team1 in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
                if (CCS.character_Num_Of_Grid == num3) dum++;
            }
        }

        if (dum >= 2)
            return true;
        return false;
    }
    bool Check_Arround(Character ACS,int num1, int num2)
    {
        int dum = 0;

        if (ACS.character_Team_Number == 1)
        {
            foreach (var team1 in BattleManager.Instance.bM_Character_Team1)
            {
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
            }
        }

        if (ACS.character_Team_Number == 2)
        {
            foreach (var team1 in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = team1.GetComponent<Character>();
                if (CCS.character_Num_Of_Grid == num1) dum++;
                if (CCS.character_Num_Of_Grid == num2) dum++;
            }
        }

        if (dum >= 2)
            return true;
        return false;
    }
    bool Skill_Attack_Executioner(GameObject attacker) // 처형자
    {
        Character ACS = attacker.GetComponent<Character>();

        if (ACS.character_is_Kill == 0)
        {
            return false;
        }
        //스킬 발동 체크

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(attacker, "처형자");

        BattleManager.Instance.bM_Round--;
        ACS.character_is_Kill = 0;

        return true;
    }                                       
    bool Skill_Attack_Struggle(GameObject character) // 발악
    {
        Character CCS = character.GetComponent<Character>();

        if (CCS.character_HP > (CCS.character_MaxHP / 10) * 9)
        {
            return false;
        }


        for(int i  = 9; i > 0; i++)
        {
            if (CCS.character_HP <= (CCS.character_MaxHP / 10) * i)
                CCS.character_Buffed_Attack += 10;
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"발악");

        return true;
    }
    bool SKill_Attack_Ranger(GameObject character,GameObject[] enemy) // 명사수 
    {
        Character CCS = character.GetComponent<Character>();

        bool enemyAllive = false;

        for(int i = 0; i < 5; i++)
        {
            if(enemy[i].GetComponent<Character>().character_Is_Allive == true)
                enemyAllive = true;
        }

        if (enemyAllive == false)
        {
            return false;
        }

        int num = 0;
        int minHP = 0;

        minHP = enemy[0].GetComponent<Character>().character_HP;
        for (int i = 0; i < 5; i++)
        {
            if (minHP > enemy[i].GetComponent<Character>().character_HP && enemy[i].GetComponent<Character>().character_Is_Allive)
            {
                minHP = enemy[i].GetComponent<Character>().character_HP;
                num = i;
            } 
        }
        for(int i = 0; i < 9; i++)
        {
            CCS.character_Attack_Range[i] = false;
        }
        Debug.Log(enemy[num].GetComponent<Character>().character_Num_Of_Grid - 1);
        CCS.character_Attack_Range[enemy[num].GetComponent<Character>().character_Num_Of_Grid - 1] = true;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"명사수");

        return true;
    }
    public int ArmorPiercer(GameObject character,GameObject enemy) // 철갑탄
    {
        Character ACS = character.GetComponent<Character>();
        Character DCS = enemy.GetComponent<Character>();
        int damage;

        if(DCS.character_Type == Character.Type.Defender)
        {
            damage = (ACS.character_Attack_Damage * (100 + ACS.character_Buffed_Attack + 60)) / 100;
        }
        else
        {
            damage = (ACS.character_Attack_Damage * (100 + ACS.character_Buffed_Attack)) / 100;
        }
        return damage;
    }
    bool Skill_Attack_ArmorPiercer(GameObject character, GameObject[] enemys)
    {
        Character CCS = character.GetComponent<Character>();

        bool haveDefender = false;

        foreach (var enemy in enemys)
        {

            for (int i = 0; i < 9; i++)
            {
                if (CCS.character_Attack_Range[i])
                {
                    if (enemy.GetComponent<Character>().character_Num_Of_Grid == i + 1 && enemy.GetComponent<Character>().character_Type == Character.Type.Defender)
                    {
                        haveDefender = true;
                    }
                }
            }
        }

        if (!haveDefender)
            CCS.character_Buffed_Attack += 10;

        return false;
    }
    bool Skill_Attack_DivineShield(GameObject character) // 천상의 보호막 
    {
        Character CCS = character.GetComponent<Character>();
        CCS.character_Divine_Shield = true;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character, "천상의 보호막");

        return true;
    }

    //bool Skill_Attack_Sturdy(GameObject character) // 옹골참 
    //{
    //    Character CCS = character.GetComponent<Character>();
    //    if (CCS.character_Sturdy == false)
    //        return false;


    //    skillmessage.SetActive(true);
    //    skillmessage.GetComponent<SkillMessage>().Message(character, "옹골참");

    //    return true;
    //}


    // 밸런스형 스킬
    bool Skill_Balanced_GBGH(GameObject character) // 모아니면도
    {
        Character CCS = character.GetComponent<Character>();

        if (CCS.character_Attack_Order != 1 && CCS.character_Attack_Order != 2 && CCS.character_Attack_Order != 9 && CCS.character_Attack_Order != 10)
        {
            return false;
        }

        if (BattleManager.Instance.bM_Team1_Is_Preemitive == true)
        {
            if (CCS.character_Team_Number == 1 && CCS.character_Attack_Order == 1)
            {
                CCS.character_Buffed_Attack += 20;
            }
            if (CCS.character_Team_Number == 1 && CCS.character_Attack_Order == 9)
            {
                CCS.character_Buffed_Damaged += 20;
            }
            if (CCS.character_Team_Number == 2 && CCS.character_Attack_Order == 2)
            {
                CCS.character_Buffed_Attack += 20;
            }
            if (CCS.character_Team_Number == 2 && CCS.character_Attack_Order == 10)
            {
                CCS.character_Buffed_Damaged += 20;
            }
        }
        else
        {
            if (CCS.character_Team_Number == 1 && CCS.character_Attack_Order == 2)
            {
                CCS.character_Buffed_Attack += 20;
            }
            if (CCS.character_Team_Number == 1 && CCS.character_Attack_Order == 10)
            {
                CCS.character_Buffed_Damaged += 20;
            }
            if (CCS.character_Team_Number == 2 && CCS.character_Attack_Order == 1)
            {
                CCS.character_Buffed_Attack += 20;
            }
            if (CCS.character_Team_Number == 2 && CCS.character_Attack_Order == 9)
            {
                CCS.character_Buffed_Damaged += 20;
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"모 아니면 도");

        return true;
    }
    bool Skill_Balanced_Blessing(GameObject character) // 축복
    {
        Character CCS = character.GetComponent<Character>();

        if (CCS.character_Team_Number == 1)
        {
            if (CCS.character_Num_Of_Grid % 3 == 1)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Damaged += 20;
                }
            }
            if (CCS.character_Num_Of_Grid % 3 == 2)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Damaged += 20;
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Attack += 20;
                }
            }

            if (CCS.character_Num_Of_Grid % 3 == 0)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Attack += 20;
                }
            }
        }

        if (CCS.character_Team_Number == 2)
        {
            if (CCS.character_Num_Of_Grid % 3 == 1)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Damaged += 20;
                }
            }
            if (CCS.character_Num_Of_Grid % 3 == 2)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Damaged += 20;
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Attack += 20;
                }
            }

            if (CCS.character_Num_Of_Grid % 3 == 0)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Attack += 20;
                }
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character, "축복");

        return true;
    }
    bool Blessing_Dead(GameObject character) // 축복 캐릭터 사망 시
    {
        Character CCS = character.GetComponent<Character>();

        if (CCS.character_Team_Number == 1)
        {
            if (CCS.character_Num_Of_Grid % 3 == 1)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Damaged -= 20;
                }
            }
            if (CCS.character_Num_Of_Grid % 3 == 2)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Damaged -= 20;
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Attack -= 20;
                }
            }

            if (CCS.character_Num_Of_Grid % 3 == 0)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Attack -= 20;
                }
            }
        }

        if (CCS.character_Team_Number == 2)
        {
            if (CCS.character_Num_Of_Grid % 3 == 1)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Damaged -= 20;
                }
            }
            if (CCS.character_Num_Of_Grid % 3 == 2)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid - 1)
                        TCS.character_Buffed_Damaged -= 20;
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Attack -= 20;
                }
            }

            if (CCS.character_Num_Of_Grid % 3 == 0)
            {
                foreach (var team in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == CCS.character_Num_Of_Grid + 1)
                        TCS.character_Buffed_Attack -= 20;
                }
            }
        }

        return false;
    }
    bool Skill_Balanced_Smoke(GameObject character,GameObject[] enemys)
    {
        Character CCS = character.GetComponent<Character>();

        foreach(var enemy in enemys)
        {
            if(CCS.character_Num_Of_Grid == BattleManager.Instance.Reverse_Enemy(enemy.GetComponent<Character>().character_Num_Of_Grid))
            {
                enemy.GetComponent<Character>().character_Buffed_Attack -= 40;
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character, "연막탄");

        return true;
    }
    bool SKill_Balanced_Survivor(GameObject character)
    {
        Character CCS = character.GetComponent<Character>();

        CCS.character_Buffed_Attack += 20;
        CCS.character_HP += 20;

        // 잠시 보류
        return true;
    }
    bool Skill_Balanced_Curse(GameObject character)
    {
        Character CCS = character.GetComponent<Character>();
        int cursedgrid = BattleManager.Instance.Reverse_Enemy(CCS.character_Num_Of_Grid);
        if (cursedgrid % 3 == 1)
        {
            if(CCS.character_Team_Number == 1)
            {
                foreach(var team2 in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team2.GetComponent<Character>();
                    if(TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if(TCS.character_Num_Of_Grid == cursedgrid + 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
            if(CCS.character_Team_Number == 2)
            {
                foreach (var team1 in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team1.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid + 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
        }
        if(CCS.character_Num_Of_Grid % 3 == 2)
        {
            if (CCS.character_Team_Number == 1)
            {
                foreach (var team2 in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team2.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid + 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid - 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
            if (CCS.character_Team_Number == 2)
            {
                foreach (var team1 in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team1.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid + 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid - 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
        }
        if(CCS.character_Num_Of_Grid % 3 == 0)
        {
            if (CCS.character_Team_Number == 1)
            {
                foreach (var team2 in BattleManager.Instance.bM_Character_Team2)
                {
                    Character TCS = team2.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid - 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
            if (CCS.character_Team_Number == 2)
            {
                foreach (var team1 in BattleManager.Instance.bM_Character_Team1)
                {
                    Character TCS = team1.GetComponent<Character>();
                    if (TCS.character_Num_Of_Grid == cursedgrid)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                    if (TCS.character_Num_Of_Grid == cursedgrid - 1)
                    {
                        TCS.character_HP -= (TCS.character_MaxHP / 10 * 3);
                        TCS.character_Buffed_Attack -= 30;
                        // 카운터 확률
                    }
                }
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character, "저주");

        return true;
    }
    bool Skill_Balanced_WideCounter(GameObject attacker, GameObject counterAttacker)
    {
        switch(attacker.GetComponent<Character>().character_Num_Of_Grid)
        {
            case 1:
                CounterArround(counterAttacker, 1, 2, 4);
                break;
            case 2:
                CounterArround(counterAttacker, 2, 1, 3, 5);
                break;
            case 3:
                CounterArround(counterAttacker, 3, 2, 6);
                break;
            case 4:
                CounterArround(counterAttacker, 4, 1, 5, 7);
                break;
            case 5:
                CounterArround(counterAttacker, 5, 2, 4, 6, 8);
                break;
            case 6:
                CounterArround(counterAttacker, 6, 3, 5, 9);
                break;
            case 7:
                CounterArround(counterAttacker, 7, 4, 8);
                break;
            case 8:
                CounterArround(counterAttacker, 8, 5, 7, 9);
                break;
            case 9:
                CounterArround(counterAttacker, 9, 6, 8);
                break;
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(counterAttacker, "광역반격");

        return true;
    }

    void CounterArround(GameObject counterAttacker, int num1, int num2, int num3)
    { 
        if(counterAttacker.GetComponent<Character>().character_Team_Number == 1)
        {
            foreach(var character in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = character.GetComponent<Character>();

                if(CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if(CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team2(num1);
                GridManager.Instance.Create_Damaged_Grid_Team2(num2);
                GridManager.Instance.Create_Damaged_Grid_Team2(num3);
            }
        }
        else
        {
            foreach(var character in BattleManager.Instance.bM_Character_Team1)
            {
                Character CCS = character.GetComponent<Character>();

                if (CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team1(num1);
                GridManager.Instance.Create_Damaged_Grid_Team1(num2);
                GridManager.Instance.Create_Damaged_Grid_Team1(num3);
            }
        }
    }
    void CounterArround(GameObject counterAttacker, int num1, int num2, int num3,int num4)
    {
        if (counterAttacker.GetComponent<Character>().character_Team_Number == 1)
        {
            foreach (var character in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = character.GetComponent<Character>();

                if (CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num4)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team2(num1);
                GridManager.Instance.Create_Damaged_Grid_Team2(num2);
                GridManager.Instance.Create_Damaged_Grid_Team2(num3);
                GridManager.Instance.Create_Damaged_Grid_Team2(num4);
            }
        }
        else
        {
            foreach (var character in BattleManager.Instance.bM_Character_Team1)
            {
                Character CCS = character.GetComponent<Character>();

                if (CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num4)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team1(num1);
                GridManager.Instance.Create_Damaged_Grid_Team1(num2);
                GridManager.Instance.Create_Damaged_Grid_Team1(num3);
                GridManager.Instance.Create_Damaged_Grid_Team1(num4);
            }
        }
    }
    void CounterArround(GameObject counterAttacker, int num1, int num2, int num3, int num4, int num5)
    {
        if (counterAttacker.GetComponent<Character>().character_Team_Number == 1)
        {
            foreach (var character in BattleManager.Instance.bM_Character_Team2)
            {
                Character CCS = character.GetComponent<Character>();

                if (CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num4)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num5)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team2(num1);
                GridManager.Instance.Create_Damaged_Grid_Team2(num2);
                GridManager.Instance.Create_Damaged_Grid_Team2(num3);
                GridManager.Instance.Create_Damaged_Grid_Team2(num4);
                GridManager.Instance.Create_Damaged_Grid_Team2(num5);
            }
        }
        else
        {
            foreach (var character in BattleManager.Instance.bM_Character_Team1)
            {
                Character CCS = character.GetComponent<Character>();

                if (CCS.character_Num_Of_Grid == num1)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num2)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num3)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num4)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                if (CCS.character_Num_Of_Grid == num5)
                {
                    StopCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                    StartCoroutine(counterAttacker.GetComponent<Character_Action>().Attack(character, true));
                }
                GridManager.Instance.Create_Damaged_Grid_Team1(num1);
                GridManager.Instance.Create_Damaged_Grid_Team1(num2);
                GridManager.Instance.Create_Damaged_Grid_Team1(num3);
                GridManager.Instance.Create_Damaged_Grid_Team1(num4);
                GridManager.Instance.Create_Damaged_Grid_Team1(num5);
            }
        }
    }
    bool Skill_Balanced_DestinyBond(GameObject deadCharacter)
    {
        int rand = Random.Range(0, 1);
        GameObject killedBy = deadCharacter.GetComponent<Character>().killedBy;

        if (rand == 0)
        {
            killedBy.GetComponent<Character_Action>().Character_Dead(deadCharacter);
        }
        else
        {
            killedBy.GetComponent<Character>().character_HP -= killedBy.GetComponent<Character>().character_HP / 2;
            killedBy.GetComponent<Character>().character_Buffed_Attack -= 50;
            // 카운터 확률
        }
        return true;
    }
    // 방어형 스킬
    bool Skill_Defender_Disarm(GameObject attacker,GameObject[] Damaged) // 무장해제 
    {
        Character ACS = attacker.GetComponent<Character>();

        int dum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (ACS.character_Attack_Range[i] == true)
            {
                foreach (var damaged in Damaged)
                {
                    if (damaged.GetComponent<Character>().character_Num_Of_Grid == i + 1)
                    { 
                        dum++;
                    }
                }
            }
        }


        if (dum == 0)
        {
            return false;
        }

        for (int i = 0; i < 9; i++)
        {
            if (ACS.character_Attack_Range[i] == true)
            {
                foreach (var damaged in Damaged)
                {
                    if (damaged.GetComponent<Character>().character_Num_Of_Grid == i + 1)
                    {
                        damaged.GetComponent<Character>().character_Attack_Damage -= 40;
                    }
                }
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(attacker,"무장해제");

        return true;
    }
}
