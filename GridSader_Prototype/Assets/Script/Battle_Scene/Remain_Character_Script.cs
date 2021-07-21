using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remain_Character_Action : MonoBehaviour
{

    public int TeamNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TeamNumber == 1)
        {
            this.gameObject.GetComponent<Text>().text = "팀 1 남은 캐릭터 " + BattleManager.Instance.bM_Remain_Character_Team1;
        }
        if(TeamNumber == 2)
        {
            this.gameObject.GetComponent<Text>().text = "팀 2 남은 캐릭터 " + BattleManager.Instance.bM_Remain_Character_Team2;
        }
    }
}
