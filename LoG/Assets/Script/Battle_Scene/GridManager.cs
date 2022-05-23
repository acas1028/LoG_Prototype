using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Mine_Red_Grid;     // ���� �׸���(��)
    public GameObject Opponent_Red_Grid; // ���� �׸���(��)
    public GameObject[] Team1Character_Position;
    public GameObject[] Team2Character_Position;
    public GameObject[] Team1_Map;
    public GameObject[] Team2_Map;
    public GameObject BuffedEffect;
    public GameObject NerfedEffect;
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
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Mine_Red_Grid.transform.localScale = new Vector3(Mine_Red_Grid.transform.localScale.x * canvas.GetComponent<RectTransform>().sizeDelta.x / 2960f, Mine_Red_Grid.transform.localScale.y * canvas.GetComponent<RectTransform>().sizeDelta.y / 1440f);
        Opponent_Red_Grid.transform.localScale = new Vector3(Opponent_Red_Grid.transform.localScale.x * canvas.GetComponent<RectTransform>().sizeDelta.x / 2960f, Opponent_Red_Grid.transform.localScale.y * canvas.GetComponent<RectTransform>().sizeDelta.y / 1440f);
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

        PlaySound.Instance.ChangeSoundAndPlay(Resources.Load("Sound/Sound/Sound/SFX/power up") as AudioClip); // ���� ���� ���
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

        PlaySound.Instance.ChangeSoundAndPlay(Resources.Load("Sound/Sound/Sound/SFX/power down") as AudioClip); // ���� ���� ���
    }
}
 