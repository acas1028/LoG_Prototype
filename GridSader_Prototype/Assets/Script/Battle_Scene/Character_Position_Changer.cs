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

    public void Position_Change() // �Ҵ�� ��ġ�迭�� ���� ĳ���� ��ġ ����
    {

        for (int i = 0; i < 5; i++) // bM_Character_Team1[i]�� 1������ 5������
        {
            for (int j = 0; j < 9; j++) // character_Num_Of_Grid�� 1������ 9������, �� 1�� �׸���~ 9�� �׸������
            {
                if (BattleManager.Instance.bM_Character_Team1[i].GetComponent<Character>().character_Num_Of_Grid == (j+1)) // ��Ʋ�Ŵ����� ĳ���� �׸��� ���� �迭�� ��ġ�ϸ�
                {
                    BattleManager.Instance.bM_Character_Team1[i].transform.position = Team1Map[j].transform.position; // ĳ������ ��ġ�� ��ġ�迭�� �̵�
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
