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
    }

    public void BeforeAttack(GameObject attacker,GameObject[] Damaged)
    {
        Character ACS = attacker.GetComponent<Character>();

        if(ACS.character_Skill == Character.Skill.Balance_GbGH)
        {
            StartCoroutine(Skill_Balanced_GbGH(attacker));
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

    public void Damaged()
    {

    }

    public void Counter()
    {

    }

    public void Check_Stat()
    {

    }

    public void Check_Dead()
    {

    }

    IEnumerator Skill_Attack_Confidence(GameObject character)
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
        skillmessage.GetComponent<SkillMessage>().Confidence(character);
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

    IEnumerator Skill_Balanced_GbGH(GameObject attacker)
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator Skill_Attack_Executioner(GameObject attacker)
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
        skillmessage.GetComponent<SkillMessage>().Executioner(attacker);
    }

    IEnumerator Skill_Defender_Disarm(GameObject attacker,GameObject[] Damaged)
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
        skillmessage.GetComponent<SkillMessage>().Disarm(attacker);

        ACS.character_Activate_Skill = false;
    }
}
