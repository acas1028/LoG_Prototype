using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject skillmessage;

    private static SkillManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static SkillManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
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

    public void BeforeAttack(GameObject attacker,GameObject Damaged)
    {
        if(attacker.GetComponent<Character_Script>().character_Skill == Character_Script.Skill.Attack_Confidence)
        {
           StartCoroutine(Skill_Attack_Confidence(attacker));
        }

        else if(attacker.GetComponent<Character_Script>().character_Skill == Character_Script.Skill.Balance_GbGH)
        {
            StartCoroutine(Skill_Balanced_GbGH(attacker));
        }
    }

    public void AfterAttack(GameObject attacker,GameObject[] Damaged)
    {
        if(attacker.GetComponent<Character_Script>().character_Skill == Character_Script.Skill.Defense_Disarm)
        {
            StartCoroutine(Skill_Disarm(attacker, Damaged));
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

    IEnumerator Skill_Attack_Confidence(GameObject attacker)
    {

        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator Skill_Balanced_GbGH(GameObject attacker)
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator Skill_Disarm(GameObject attacker,GameObject[] Damaged)
    {
        Character_Script ACS = attacker.GetComponent<Character_Script>();
        ACS.character_Activate_Skill = true;

        int dum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (ACS.character_Attack_Range[i] == true)
            {
                foreach (var damaged in Damaged)
                {
                    if (damaged.GetComponent<Character_Script>().character_Num_Of_Grid == i + 1)
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
                    if (damaged.GetComponent<Character_Script>().character_Num_Of_Grid == i + 1)
                    {
                        damaged.GetComponent<Character_Script>().character_Attack_Damage -= 40;
                    }
                }
            }
        }

        skillmessage.SetActive(true);
        skillmessage.GetComponent<SkillMessage>().Disarm(attacker);

        ACS.character_Activate_Skill = false;
    }
}
