using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Mine_Red_Grid;     // 붉은 그리드(나)
    public GameObject Opponent_Red_Grid; // 붉은 그리드(적)
    public GameObject[] Team1Character_Position;
    public GameObject[] Team2Character_Position;
    public GameObject[] Team1_Map;
    public GameObject[] Team2_Map;
    public GameObject BuffedEffect;
    public GameObject NerfedEffect;
    private static GridManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GridManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
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

    public void Create_Buffed_Grid(int Team_Num,int Buffed_Grid_Num)
    {
        if (Team_Num == 1)
        {
            GameObject BuffedGrid = Instantiate(BuffedEffect, Team1_Map[Buffed_Grid_Num - 1].transform.position, Quaternion.identity);
            Destroy(BuffedGrid, BattleManager.Instance.bM_AttackTimegap);
        }
        else
        {
            GameObject BuffedGrid = Instantiate(BuffedEffect, Team2_Map[Buffed_Grid_Num - 1].transform.position, Quaternion.identity);
            Destroy(BuffedGrid, BattleManager.Instance.bM_AttackTimegap);
        }

        PlaySound.Instance.ChangeSoundAndPlay(Resources.Load("Sound/Sound/Sound/SFX/power up") as AudioClip); // 공격 사운드 재생
    }

    public void Create_Nerfed_Grid(int Team_Num,int Nerfed_Grid_Num)
    {
        if(Team_Num == 1)
        {
            GameObject NerfedGrid = Instantiate(NerfedEffect, Team1_Map[Nerfed_Grid_Num - 1].transform.position, Quaternion.identity);
            Destroy(NerfedGrid, BattleManager.Instance.bM_AttackTimegap);
        }

        else
        {
            GameObject NerfedGrid = Instantiate(NerfedEffect, Team2_Map[Nerfed_Grid_Num - 1].transform.position, Quaternion.identity);
            Destroy(NerfedGrid, BattleManager.Instance.bM_AttackTimegap);
        }

        PlaySound.Instance.ChangeSoundAndPlay(Resources.Load("Sound/Sound/Sound/SFX/power down") as AudioClip); // 공격 사운드 재생
    }
}
 