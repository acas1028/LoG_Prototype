using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergeManager : MonoBehaviour
{
    public GameObject synergemessage;

    private static SynergeManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static SynergeManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SynergeManager)) as SynergeManager;

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

    public IEnumerator CheckSynerge(GameObject[] team)
    {
        int attack = 0;
        int defense = 0;
        int balance = 0;

        foreach(var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            if (CCS.character_Type == CharacterStats.CharacterType.Attacker)
                attack++;
            if (CCS.character_Type == CharacterStats.CharacterType.Defender)
                defense++;
            if (CCS.character_Type == CharacterStats.CharacterType.Balance)
                balance++;
        }
        if (attack >= 1 && balance >= 1 && defense >= 1)
        {
            A1D1B1(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (attack == 2)
        {
            A2(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (attack >= 3)
        {
            A3(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (defense == 2)
        {
            D2(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (defense >= 3)
        {
            D3(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (balance == 2)
        {
            B2(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
        if (balance >= 3)
        {
            B3(team);
            yield return new WaitForSeconds(BattleManager.Instance.bM_AttackTimegap);
        }
    }

    void A1D1B1(GameObject[] team) // 공1방1밸1 시너지
    {
        foreach(var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            CCS.character_Counter_Probability += 10;
        }

        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "공1방1밸1");
    }
    void A2(GameObject[] team) // 공2 시너지
    {
        foreach (var character in team)
        {
            Character CCS = character.GetComponent<Character>();
            if(CCS.character_Type == CharacterStats.CharacterType.Attacker)
                CCS.character_Buffed_Attack += 20;
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "공2");
    }
    void A3(GameObject[] team) // 공3 시너지
    {
        foreach (var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            CCS.character_Buffed_Attack += 20;
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "공3");
    }
    void B2(GameObject[] team) // 밸2 시너지
    {
        foreach (var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            if(CCS.character_Type == CharacterStats.CharacterType.Balance)
            {
                CCS.character_Buffed_Attack += 10;
                CCS.character_Buffed_Damaged += 10;
            }
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "밸2");
    }
    void B3(GameObject[] team) // 밸3 시너지
    {
        foreach (var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            CCS.character_Buffed_Attack += 10;
            CCS.character_Buffed_Damaged += 10;
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "밸3");
    }

    void D2(GameObject[] team) // 방2 시너지
    {
        foreach(var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            if(CCS.character_Type == CharacterStats.CharacterType.Defender)
            {
                CCS.character_Buffed_Damaged -= 20;
            }
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "방2");
    }

    void D3(GameObject[] team) // 방3 시너지
    {
        foreach (var character in team)
        {
            Character CCS = character.GetComponent<Character>();

            CCS.character_Buffed_Damaged -= 20;
        }
        synergemessage.SetActive(true);
        synergemessage.GetComponent<SynergeMessage>().Message(team[0].GetComponent<Character>().character_Team_Number, "방3");
    }
}
