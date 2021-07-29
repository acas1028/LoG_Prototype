using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Red_Grid;     // ���� �׸���
    public GameObject[] Team1Map;
    public GameObject[] Team2Map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // �����߿� �ǰݴ��� ������ ���� �׸��� ���� (1��)
    // ���߿� �� �׷����� ���� �� ���� ���� �ʿ�
    public void Create_Damaged_Grid_Team1(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Red_Grid, Team1Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }

    //�����߿� �ǰݴ��� ������ ���� �׸��� ���� (2��)
    public void Create_Damaged_Grid_Team2(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Red_Grid, Team2Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }
}
 