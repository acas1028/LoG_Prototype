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
        Character ACS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = ACS.character_Team_Number + "�� " + ACS.character_Number + "�� ĳ���� " + skillname + " ��ų �ߵ�!";

        Invoke("Disable", 2.0f);
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
