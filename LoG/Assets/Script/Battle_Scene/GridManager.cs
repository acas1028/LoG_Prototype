using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Mine_Red_Grid;     // 붉은 그리드(나)
    public GameObject Opponent_Red_Grid; // 붉은 그리드(적)
    public GameObject[] Team1Character_Position;
    public GameObject[] Team2Character_Postion;
    public GameObject[] Team1_Map;
    public GameObject[] Team2_Map;
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
        GameObject redGrid = Instantiate(Mine_Red_Grid, Team1_Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }

    //전투중에 피격당한 범위에 붉은 그리드 생성 (2팀)
    public void Create_Damaged_Grid_Team2(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Opponent_Red_Grid, Team2_Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }
}
 