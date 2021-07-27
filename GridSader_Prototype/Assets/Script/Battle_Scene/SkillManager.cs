using System.Collections;
using System.Collections.Generic;
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

    public void AfterSetting(GameObject character)
    {
        Character CCS = character.GetComponent<Character>();

        if(CCS.character_Skill == Character.Skill.Attack_Confidence)
        {
            StartCoroutine(Skill_Attack_Confidence(character));
        }

        if(CCS.character_Skill == Character.Skill.Balance_GBGH)
        {
            StartCoroutine(Skill_Balanced_GBGH(character));
        }

        if(CCS.character_Skill == Character.Skill.Balance_Union)
        {
            StartCoroutine(Skill_Balanced_Union(character));
        }
    }

    public void BeforeAttack(GameObject attacker,GameObject[] Damaged)
    {
        Character ACS = attacker.GetComponent<Character>();

        if(ACS.character_Skill == Character.Skill.Attack_Ranger)
        {
            StartCoroutine(SKill_Attack_Ranger(attacker,Damaged));
        }

        if(ACS.character_Skill == Character.Skill.Attack_Struggle)
        {
            StartCoroutine(Skill_Attack_Struggle(attacker));
        }

    }

    public void AfterAttack(GameObject attacker,GameObject[] Damaged)
    {
        Character ACS = attacker.GetComponent<Character>();

        if(ACS.character_Skill == Character.Skill.Defense_Disarm)
        {
            StartCoroutine(Skill_Defender_Disarm(attacker, Damaged));
        }

        if(ACS.character_Skill == Character.Skill.Attack_Executioner)
        {
            StartCoroutine(Skill_Attack_Executioner(attacker));
        }
    }

    // 공격형 스킬
    IEnumerator Skill_Attack_Confidence(GameObject character) // 자신감
    {
        Character ACS = character.GetComponent<Character>();
        ACS.character_Activate_Skill = true;

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
            ACS.character_Activate_Skill = false;
            yield break;
        }

        //스킬 발동 체크

        yield return new WaitForSeconds(2.0f);

        ACS.character_Attack_Damage += 40;
        ACS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"자신감");
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
    IEnumerator Skill_Attack_Executioner(GameObject attacker) // 처형자
    {
        Character ACS = attacker.GetComponent<Character>();
        ACS.character_Activate_Skill = true;

        if (ACS.character_is_Kill == 0)
        {
            ACS.character_Activate_Skill = false;
            yield break;
        }
        //스킬 발동 체크

        yield return new WaitForSeconds(2.0f);

        BattleManager.Instance.bM_Round--;
        ACS.character_is_Kill = 0;
        ACS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(attacker,"처형자");
    }                                       

    IEnumerator Skill_Attack_Struggle(GameObject character) // 발악
    {
        Character CCS = character.GetComponent<Character>();
        CCS.character_Activate_Skill = true;

        if(CCS.character_HP >= CCS.character_MaxHP / 3)
        {
            CCS.character_Activate_Skill = false;
            yield break;
        }

        yield return new WaitForSeconds(2.0f);

        CCS.character_Attack_Damage += 50;
        CCS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"발악");

    }

    IEnumerator SKill_Attack_Ranger(GameObject character,GameObject[] enemy) // 명사수 
    {
        Character CCS = character.GetComponent<Character>();
        CCS.character_Activate_Skill = true;

        bool enemyAllive = false;

        for(int i = 0; i < 5; i++)
        {
            if(enemy[i].GetComponent<Character>().character_Is_Allive == true)
                enemyAllive = true;
        }

        if (enemyAllive == false)
        {
            CCS.character_Activate_Skill = false;
            yield break;
        }
        yield return new WaitForSeconds(2.0f);

        int num = 0;
        int minHP = 0;

        minHP = enemy[0].GetComponent<Character>().character_HP;
        for (int i = 0; i < 5; i++)
        {
            if (minHP > enemy[i].GetComponent<Character>().character_HP)
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

        CCS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"명사수");
    }

    public int Skill_Attack_ArmorPiercer(GameObject character,GameObject enemy) // 철갑탄
    {
        Character ACS = character.GetComponent<Character>();
        Character DCS = enemy.GetComponent<Character>();
        int damage;

        if(DCS.character_Type == Character.Type.Defender)
        {
            damage = (ACS.character_Attack_Damage * (100 + ACS.character_Buffed_Attack + 20)) / 100;
        }
        else
        {
            damage = (ACS.character_Attack_Damage * (100 + ACS.character_Buffed_Attack)) / 100;
        }
        return damage;
    }

    IEnumerator Skill_Attack_DivineShield(GameObject character) // 천상의 보호막 
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator Skill_Attack_Sturdy(GameObject character) // 옹골참 
    {
        yield return new WaitForSeconds(2.0f);
    }


    // 밸런스형 스킬
    IEnumerator Skill_Balanced_GBGH(GameObject character) // 모아니면도
    {
        Character CCS = character.GetComponent<Character>();
        CCS.character_Activate_Skill = true;

        if (CCS.character_Attack_Order != 1 && CCS.character_Attack_Order != 2 && CCS.character_Attack_Order != 9 && CCS.character_Attack_Order != 10)
        {
            CCS.character_Activate_Skill = false;
            yield break;
        }

        yield return new WaitForSeconds(2.0f);

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

        CCS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"모 아니면 도");
    }

    IEnumerator Skill_Balanced_Union(GameObject character) // 결속
    {
        Character CCS = character.GetComponent<Character>();
        CCS.character_Activate_Skill = true;

        yield return new WaitForSeconds(2.0f);

        if (CCS.character_Team_Number == 1)
        {
            foreach (var union in BattleManager.Instance.bM_Character_Team1)
            {
                Character UCS = union.GetComponent<Character>();
                if (UCS.character_Num_Of_Grid == CCS.character_Union_Select)
                {
                    UCS.character_Buffed_Attack += 20;
                    UCS.character_Buffed_Damaged += 20;
                }
            }
        }
        else
        {
            foreach (var union in BattleManager.Instance.bM_Character_Team2)
            {
                Character UCS = union.GetComponent<Character>();
                if (UCS.character_Num_Of_Grid == CCS.character_Union_Select)
                {
                    UCS.character_Buffed_Attack += 20;
                    UCS.character_Buffed_Damaged += 20;
                }
            }
        }

        CCS.character_Activate_Skill = false;

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Message(character,"결속");

    }


    // 방어형 스킬
    IEnumerator Skill_Defender_Disarm(GameObject attacker,GameObject[] Damaged) // 무장해제 
    {
        Character ACS = attacker.GetComponent<Character>();
        ACS.character_Activate_Skill = true;

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
            ACS.character_Activate_Skill = false;
            yield break;
        }

        yield return new WaitForSeconds(2.0f);

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

        ACS.character_Activate_Skill = false;
    }
}
