using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        character_placement , character_choose_grid_property
    }

   
    private orders ui_order; //������ ������ ����ϴ� ����
    private GameObject arraymanager; //������Ʈ arraymanager
    private GameObject[] null_Character; //������Ʈ nullcharacter
    private GameObject popup;

    private int arraybutton_On = 0;
    private void Start()
    {
        popup = GameObject.FindGameObjectWithTag("Popup"); // tag�� �̿��� popupâ ��������
        popup.SetActive(false);
        ui_order = orders.character_placement;
        arraymanager = GameObject.FindWithTag("ArrayManager"); // find�� �̿��� �� ������ �Ͼ ���� ������ ���װ� �߻����� ��� �̰��� �ֽ�(unity communication  �Ǽ�) 
        arraymanager.SetActive(false);
        null_Character = GameObject.FindGameObjectsWithTag("Null_Character"); // tag�� �̿��� nullcharacter ��������
    }

    private void Update()
    { 
        Character_placement();

    }



    void Character_placement()// ĳ���� ��ġ �ܰ��϶� �Լ� �۵�
    {
        if(ui_order==orders.character_placement)
        {
            
            arraymanager.SetActive(true);
            for(int i=0; i<null_Character.Length;i++) //��ġ�� ���������� �����ִ� nullCharacter�� �ٽ� on��Ų��.
            {
                null_Character[i].SetActive(true);
            }
     
        }

    }

    void Charactger_chooose_grid_property() // ĭ ���� Ư���� ���� ĳ���Ͱ� ��ġ �� ��, Ư���� ����ϴ� ��
    {

    }

    void Debug_Manager() //����׿� �Լ�
    {
        Debug.Log(ui_order);
        Debug.Log(arraybutton_On);
    }
}
