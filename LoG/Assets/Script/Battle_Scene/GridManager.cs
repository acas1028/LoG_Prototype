using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Mine_Red_Grid;     // ���� �׸���(��)
    public GameObject Opponent_Red_Grid; // ���� �׸���(��)
    public GameObject[] Team1Character_Position;
    public GameObject[] Team2Character_Postion;
    public GameObject[] Team1_Map;
    public GameObject[] Team2_Map;

    private static GridManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static GridManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GridManager)) as GridManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

    }
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
        GameObject redGrid = Instantiate(Mine_Red_Grid, Team1_Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }

    //�����߿� �ǰݴ��� ������ ���� �׸��� ���� (2��)
    public void Create_Damaged_Grid_Team2(int Damaged_Grid_Num)
    {
        GameObject redGrid = Instantiate(Opponent_Red_Grid, Team2_Map[Damaged_Grid_Num - 1].transform.position, Quaternion.identity);
        Destroy(redGrid, BattleManager.Instance.bM_AttackTimegap);
    }
}
 