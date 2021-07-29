using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Red_Grid;     // 붉은 그리드
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


    // 전투중에 피격당한 범위에 붉은 그리드 생성 (1팀)
    // 나중에 맵 그래픽을 받을 시 새로 변경 필요
    public void Create_Damaged_Grid_Team1(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Red_Grid, Team1Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }

    //전투중에 피격당한 범위에 붉은 그리드 생성 (2팀)
    public void Create_Damaged_Grid_Team2(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Red_Grid, Team2Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }
}
 