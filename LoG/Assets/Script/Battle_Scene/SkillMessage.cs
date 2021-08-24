using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMessage : MonoBehaviour
{
    public GameObject skill_Animation_image;
    public GameObject skill_explanation;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Message(GameObject character , string skillname)
    {
        skill_Animation_image.GetComponent<Animator>().Play("Skill_Production_appear", -1, 0f);

        
        Character ACS = character.GetComponent<Character>();
        this.GetComponent<Text>().text = skillname;
        
        skill_explanation.GetComponent<SkillMessage_Explanation>().Skill_Message_explanation(skillname);
        
        //Invoke("Disable", BattleManager.Instance.bM_Timegap);
        
        
    }

    

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
