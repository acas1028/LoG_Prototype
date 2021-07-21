using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Position_Changer : MonoBehaviour
{
    public GameObject[] Team1Map;
    public GameObject[] Team2Map;


    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        Position_Change();
    }

    public void Position_Change() // 할당된 위치배열에 따라 캐릭터 위치 변경
    {

        for (int i = 0; i < 5; i++) // bM_Character_Team1[i]의 1번부터 5번까지
        {
            for (int j = 0; j < 9; j++) // character_Num_Of_Grid의 1번부터 9번까지, 즉 1번 그리드~ 9번 그리드까지
            {
                if (BattleManager.Instance.bM_Character_Team1[i].GetComponent<Character>().character_Num_Of_Grid == (j+1)) // 배틀매니저의 캐릭터 그리드 값이 배열과 일치하면
                {
                    BattleManager.Instance.bM_Character_Team1[i].transform.position = Team1Map[j].transform.position; // 캐릭터의 위치를 위치배열로 이동
                    //Debug.Log("Grid_Position"+ BattleManager.Instance.bM_Character_Team1[i].GetComponent<Character_Action>().character_Num_Of_Grid);
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BattleManager.Instance.bM_Character_Team2[i].GetComponent<Character>().character_Num_Of_Grid == (j+1))
                {
                    BattleManager.Instance.bM_Character_Team2[i].transform.position = Team2Map[j].transform.position;
                }
            }
        }
    }
}
