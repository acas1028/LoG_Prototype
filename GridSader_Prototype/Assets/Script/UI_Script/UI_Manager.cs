using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    private static UI_Manager _instance;

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

    //���� ������ ��Ʈȭ���� ���� ��ȭ�� ����
    enum orders
    {
        use_of_job_skills, character_stat_distribution, character_placement
    }

    //������ ������ ����ϴ� ����
    private orders ui_order;

    

    private void Start()
    {
        ui_order = orders.use_of_job_skills;// �ϴ� job skill ����� ó������ ����
    }

    private void Update()
    {
        
    }

    void GetMOuseclick_Vector() //���� �ɽ�Ʈ�� �̿��� ������� ���콺 Ŭ�� ��ǥ ŉ��
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 Mouse_Position = Input.mousePosition;
            

        }
    }


    void Character_stat_distribution()// ĳ���� ���� �й� �����϶� �Լ� �۵�
    {
        if(ui_order== orders.character_stat_distribution)
        {
            //�ϴ� �ؿ� ĳ���� �κ��丮 ������ �� ��ũ��Ʈ�� �ƴ� ������Ʈ ���� ��ũ��Ʈ�� �߰����� ���� ������ ĳ���� ���ݺй��̸� ��ٷ� ��Ÿ������ ����.

            //�κ��丮�� ĳ���� ��ġ�� �κ��丮�� ũ�⸦ �����ϰ� �����ؾ��ϹǷ� �ϴ� ��� ����.

            //����ĳ��Ʈ ������ �������� ĳ���� Ŭ��-> �˾�â ���� �����ϵ��� ����
            
        }

    }

    void Character_placement()// ĳ���� ��ġ �ܰ��϶� �Լ� �۵�
    {
        if(ui_order==orders.character_placement)
        {

        }

    }
}
