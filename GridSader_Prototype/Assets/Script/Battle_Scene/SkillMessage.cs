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
        Debug.Log("<color=red>�޽��� ���</color>");
        StartCoroutine(message(character,skillname));
    }

    IEnumerator message(GameObject character,string skillname)
    {
        Debug.LogFormat("<color=yellow>" + skillname + "�ߵ� , ������: {0}</color>", character.GetComponent<Character>().character_Attack_Order);
        Character ACS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���� " + skillname + " ��ų �ߵ�!";

        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
        Debug.LogFormat("<color=pink>" + skillname + "���� , ������: {0}</color>", character.GetComponent<Character>().character_Attack_Order);

    }

}
