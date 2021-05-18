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
    public List<GameObject> bM_Character_Team1;
    public List<GameObject> bM_Character_Team2;
    public int bM_Phase { get; set; }
    public bool bM_Is_Preemitive { get; set; }
    public int bM_Remain_Character_Team1 { get; set; }
    public int bM_Remain_Character_Team2 { get; set; }
    public int bM_Remain_HP_Team1 { get; set; }
    public int bM_Remain_HP_Team2 { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(bM_Character_Team1[i]);
        }
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bM_Character_Team2[i]);
        }
        bM_Phase = 0;
        bM_Is_Preemitive = false;
        bM_Remain_Character_Team1 = 0;
        bM_Remain_Character_Team2 = 0;
        bM_Remain_HP_Team1 = 0;
        bM_Remain_HP_Team2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Running_Phase();
    }

    void Running_Phase()
    {
        switch(bM_Phase)
        {
            case 0:
                BM_Character_Setting();
                break;
            case 1:
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
            Debug.Log(bM_Character_Team1[i].GetComponent<Character_Script>().character_HP);
        }
        for (int i = 0; i < 5; i++)
        {
            bM_Character_Team2[i].GetComponent<Character_Script>().Character_Setting(i + 1);
            Debug.Log(bM_Character_Team2[i].GetComponent<Character_Script>().character_HP);
        }
    }
}
