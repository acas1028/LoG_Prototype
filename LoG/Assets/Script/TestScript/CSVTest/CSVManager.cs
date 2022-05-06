using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSVManager : MonoBehaviour
{
    private static CSVManager _instance;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static CSVManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
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
