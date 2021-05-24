using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    private static UI_Manager _instance;

    //������ ������ ����ϴ� ����
    private int ui_order;

    //���� ������ ��Ʈȭ���� ���� ��ȭ�� ����
    enum orders
    {
        use_of_job_skills, character_stat_distribution, character_placement
    }

    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    private UI_Manager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UI_Manager)) as UI_Manager;

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
}
