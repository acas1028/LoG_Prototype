using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disarm(GameObject attacker)
    {
        StartCoroutine(CDisarm(attacker));
    }

    IEnumerator CDisarm(GameObject attacker)
    {
        Character_Script ACS = attacker.GetComponent<Character_Script>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "팀 " + ACS.character_Attack_Order + "번 캐릭터 무장해제 스킬 발동!";

        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }
}
