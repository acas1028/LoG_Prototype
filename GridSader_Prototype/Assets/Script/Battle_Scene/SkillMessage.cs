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
        StartCoroutine(message(character,skillname));
    }

    IEnumerator message(GameObject character,string skillname)
    {
        Character ACS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "팀 " + ACS.character_Number + "번 캐릭터 " + skillname + " 스킬 발동!";

        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }

}
