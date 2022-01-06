using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSVManager : MonoBehaviour
{
    private static CSVManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CSVManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CSVManager)) as CSVManager;

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

    public int StageNumber;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StageNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingStage(int stageNum)
    {
        StageNumber = stageNum;
        SceneManager.LoadScene("PVE_CSVTestScene2");
    }
}
