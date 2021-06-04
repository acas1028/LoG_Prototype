using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static BattleManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static BattleManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(BattleManager)) as BattleManager;

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


    public GameObject[] bM_Character_Team1;
    public GameObject[] bM_Character_Team2;
    public int bM_Phase { get; set; }
    public bool bM_Team1_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }
    
    public GameObject Damaged_Grid;
    private float Debug_Delay;

    // Start is called before the first frame update
    void Start()
    {
        Debug_Delay = 0.0f;

        bM_Phase = 0;
        bM_Team1_Is_Preemitive = true;
        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;

        GameObject map = null;
        map = GameObject.Find("Castle_Background");
        if(map != null)
        {
            Debug.Log(map.name + " 오브젝트를 불러오는데 성공");
            map.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            Debug.LogError(map.name + " 불러오는데 실패");
        }
        
        GameObject Damaged_Gird = Instantiate(Damaged_Grid);

        for (float x= -6.7f; x<=-2f;)
        {
            for(float y=-2f; y<=3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;

            }

            x +=2.25f;
        }
        // (grid-1)%3 %3->열 결정
        for (float x = 2.2f; x <= 6.7f;)
        {

            for (float y = -2f; y <= 3;)
            {
                Instantiate(Damaged_Grid, new Vector3(x, y, 0), Quaternion.identity);
                y += 2.25f;

            }

            x += 2.25f;
        }
        // Instantiate(Damaged_Grid, new Vector3(-4.5f, 2.5f, 0), Quaternion.identity);
        // Instantiate(Damaged_Grid, new Vector3(-2.2f, 2.5f, 0), Quaternion.identity);
        
    }


    // Update is called once per frame
    void Update()
    {
        Debug_Delay += Time.deltaTime;

        if (Debug_Delay > 2) // 디버깅용.
        {
            Running_Phase();
            Debug_Delay = 0;
        }
    }

    void Running_Phase()
    {
        switch(bM_Phase)
        {
            case 0:
                BM_Character_Setting(); // 디버깅용이니 차후 변경예정.
                bM_Phase++;
                break;
            case 1:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 2:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 3:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 4:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            case 5:
                Battle(bM_Phase);
                Battle(bM_Phase);
                bM_Phase++;
                break;
            default:
                break;      
        }
    }

    void BM_Character_Setting()
    {
        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team1[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Is_Preemptive = true; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1; // Debuging
            bM_Character_Team1[i].GetComponent<Character_Script>().Debuging_Character();

            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Attack_Order = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Is_Preemptive = false; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().character_Num_Of_Grid = i + 1; // Debuging
            bM_Character_Team2[i].GetComponent<Character_Script>().Debuging_Character();
        }
        
    }

    void Character_Attack(GameObject attacker,GameObject[] enemy_Characters,int attacked_Grid) //캐릭터 공격
    {
        // 공격 하는 캐릭터와, 적의 모든 캐릭터들, 공격 할 위치를 받아온다.
        // 적의 모든 캐릭터들을 탐색하여, 공격 할 위치에 존재하고, 살아있는 캐릭터를 공격한다.
        foreach(GameObject enemy_Character in enemy_Characters)
        {
            if (enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid == attacked_Grid
            && enemy_Character.GetComponent<Character_Script>().character_Is_Allive)
            {
                Debug.Log(attacker.GetComponent<Character_Script>().character_Num_Of_Grid + " attack " + enemy_Character.GetComponent<Character_Script>().character_Num_Of_Grid +
                    " by " + attacker.GetComponent<Character_Script>().character_Attack_Damage);
                attacker.GetComponent<Character_Script>().Character_Attack(enemy_Character);
            }
        }
    }
    void Battle(int phase) // 선공,후공에 따라 배틀을 진행한다.
    {
        if (bM_Team1_Is_Preemitive) // 선공 판별 (차후 변경 예정)
        {
            foreach(GameObject team1_Character in bM_Character_Team1)
            {
                if(team1_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                && team1_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀1의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (team1_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
                            Character_Attack(team1_Character,bM_Character_Team2, j + 1);
                    }
                }
                team1_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        else
        {
            foreach (GameObject team2_Character in bM_Character_Team2)
            {
                if (team2_Character.GetComponent<Character_Script>().character_Attack_Order == phase
                 && team2_Character.GetComponent<Character_Script>().character_Is_Allive) // 팀2의 캐릭터 중 공격순서가 페이즈와 똑같고, 살아있는 캐릭터가 공격을 실행한다.
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (team2_Character.GetComponent<Character_Script>().character_Attack_Range[j] == true) // 공격범위만큼 공격한다.
                            Character_Attack(team2_Character, bM_Character_Team1, j + 1);
                    }
                }
                team2_Character.GetComponent<Character_Script>().Debuging_Character();
            }
        }
        bM_Team1_Is_Preemitive = !bM_Team1_Is_Preemitive; // 선후공 변경!

        Calculate_Remain_HP();

        Debug.Log("Phase " + bM_Phase + " Team1 남은체력 = " + bM_Remain_HP_Team1);
        Debug.Log("Phase " + bM_Phase + " Team2 남은체력 = " + bM_Remain_HP_Team2);
    }

    void Calculate_Remain_HP() //남은 체력 계산
    {
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
        for (int i = 0; i < 5; i++)
        {
            bM_Remain_HP_Team1 += bM_Character_Team1[i].GetComponent<Character_Script>().character_HP;
            bM_Remain_HP_Team2 += bM_Character_Team2[i].GetComponent<Character_Script>().character_HP;
        }
    }
}

