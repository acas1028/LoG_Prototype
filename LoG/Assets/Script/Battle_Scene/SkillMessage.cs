using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMessage : MonoBehaviour
{
    public GameObject[] skill_Animation_image;
    public GameObject[] skill_Name;
    public GameObject[] skill_explanation;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Message(GameObject character , string skillname)
    {

        if(character.GetComponent<Character>().character_Team_Number ==1)
        {
            if (skill_Animation_image[0].activeSelf == false)
                skill_Animation_image[0].SetActive(true);
            skill_Animation_image[0].GetComponent<Animator>().Play("Skill_Production_appear", -1, 0f);
            skill_Name[0].GetComponent<Text>().text = skillname;
            skill_explanation[0].GetComponent<SkillMessage_Explanation>().Skill_Message_explanation(skillname);
        }

        else
        {
            if (skill_Animation_image[1].activeSelf == false)
                skill_Animation_image[1].SetActive(true);
            skill_Animation_image[1].GetComponent<Animator>().Play("Skill_Production_Enemy_appear", - 1, 0f);
            skill_Name[1].GetComponent<Text>().text = skillname;
            skill_explanation[1].GetComponent<SkillMessage_Explanation>().Skill_Message_explanation(skillname);
        }

        
        //Character ACS = character.GetComponent<Character>();
       
        
        //Invoke("Disable", BattleManager.Instance.bM_Timegap);
        
        
    }

    

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
