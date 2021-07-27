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

    public void Message(GameObject character , string skillname)
    {
        Debug.Log("<color=red>메시지 출력</color>");
        StartCoroutine(message(character,skillname));
    }

    IEnumerator message(GameObject character,string skillname)
    {
        Debug.LogFormat("<color=yellow>" + skillname + "발동 , 공격자: {0}</color>", character.GetComponent<Character>().character_Attack_Order);
        Character ACS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "팀 " + ACS.character_Number + "번 캐릭터 " + skillname + " 스킬 발동!";

        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
        Debug.LogFormat("<color=pink>" + skillname + "종료 , 공격자: {0}</color>", character.GetComponent<Character>().character_Attack_Order);

    }

}
