using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMessage_Explanation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skill_Message_explanation(string skillname)
    {
        switch (skillname)
        {
            case "처형자":
                this.GetComponent<Text>().text = "같은 범위를 한번더 공격";
                break;
            case "발악":
                this.GetComponent<Text>().text = "피해를 입으면, 공격력 증가";
                break;
            case "명사수":
                this.GetComponent<Text>().text = "약한 캐릭터 공격";
                break;
            case "천상의 보호막":
                this.GetComponent<Text>().text = "피해 1회 무시";
                break;
            case "옹골참":
                this.GetComponent<Text>().text = "1회 생존 및 반격";
                break;
            case "모 아니면 도":
                this.GetComponent<Text>().text = "처음은 공격력 증가, 마지막은 방어력 증가";
                break;
            case "축복":
                this.GetComponent<Text>().text = "후방 공격력 증가, 전방 방어력 증가";
                break;
            case "연막탄":
                this.GetComponent<Text>().text = "상대 공격력 감소";
                break;
            case "저주":
                this.GetComponent<Text>().text = "저주 사용";
                break;
            case "광역반격":
                this.GetComponent<Text>().text = "공격자 포함 4칸 반격";
                break;

                //추후에 추가 


        }
    }
}
