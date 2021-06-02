using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Position_Changer : MonoBehaviour
{
    public Vector3[] Grid_Position_Team1; // ��1 ��ġ�迭
    public Vector3[] Grid_Position_Team2; // ��2 ��ġ�迭


    void Start()
    {
        Grid_Position_Team1 = new Vector3[9];
        Grid_Position_Team2 = new Vector3[9];
        //Position_Change();
    }



    // Update is called once per frame
    void Update()
    {
        Position_Change();
    }

    public void Position_Change() // �Ҵ�� ��ġ�迭�� ���� ĳ���� ��ġ ����
    {
        
        Grid_Position_Team1[0] = new Vector3(-6.7f, 2.5f, 0f);
        Grid_Position_Team1[1] = new Vector3(-4.4f, 2.5f, 0f);
        Grid_Position_Team1[2] = new Vector3(-2.2f, 2.5f, 0f);
        Grid_Position_Team1[3] = new Vector3(-6.7f, 0f, 0f);
        Grid_Position_Team1[4] = new Vector3(-4.4f, 0f, 0f);
        Grid_Position_Team1[5] = new Vector3(-2.2f, 0f, 0f);
        Grid_Position_Team1[6] = new Vector3(-6.7f, -2f, 0f);
        Grid_Position_Team1[7] = new Vector3(-4.4f, -2f, 0f);
        Grid_Position_Team1[8] = new Vector3(-2.2f, -2f, 0f);

        Grid_Position_Team2[0] = new Vector3(2.2f, 2.5f, 0f);
        Grid_Position_Team2[1] = new Vector3(4.4f, 2.5f, 0f);
        Grid_Position_Team2[2] = new Vector3(6.7f, 2.5f, 0f);
        Grid_Position_Team2[3] = new Vector3(2.2f, 0f, 0f);
        Grid_Position_Team2[4] = new Vector3(4.4f, 0f, 0f);
        Grid_Position_Team2[5] = new Vector3(6.7f, 0f, 0f);
        Grid_Position_Team2[6] = new Vector3(2.2f, -2f, 0f);
        Grid_Position_Team2[7] = new Vector3(4.4f, -2f, 0f);
        Grid_Position_Team2[8] = new Vector3(6.7f, -2f, 0f);

        for (int i = 0; i < 5; i++) // bM_Character_Team1[i]�� 1������ 5������
        {
            for (int j = 0; j < 8; j++) // character_Num_Of_Grid�� 1������ 9������, �� 1�� �׸���~ 9�� �׸������
            {
                if (BattleManager.Instance.bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Grid == (j+1)) // ��Ʋ�Ŵ����� ĳ���� �׸��� ���� �迭�� ��ġ�ϸ�
                {
                    BattleManager.Instance.bM_Character_Team1[i].transform.position = Grid_Position_Team1[j]; // ĳ������ ��ġ�� ��ġ�迭�� �̵�
                    Debug.Log("Grid_Position"+ BattleManager.Instance.bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Grid);
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (BattleManager.Instance.bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Grid == (j+1))
                {
                    BattleManager.Instance.bM_Character_Team2[i].transform.position = Grid_Position_Team2[j];
                    //Instantiate(BattleManager.Instance.bM_Character_Team2[i], Grid_Position[j], Quaternion.identity);
                }
            }
        }
    }
}
