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
        use_of_job_skills, character_stat_distribution, character_placement
    }

   
    private orders ui_order; //������ ������ ����ϴ� ����
    private GameObject arraymanager; //������Ʈ arraymanager
    private GameObject character_inventory; //������Ʈ panel
    private GameObject arrayButton; // ������Ʈ arraybutton
    private GameObject popup; // ������Ʈ popup
    private GameObject[] character_inventory_button; // ������Ʈ inventory(ĳ���� ���� ��ư)
    private GameObject[] null_Character; //������Ʈ nullcharacter

    private int arraybutton_On = 0;

    private void Start()
    {
        ui_order = orders.use_of_job_skills;// �ϴ� job skill ����� ó������ ����
        arraymanager = GameObject.FindWithTag("ArrayManager"); // find�� �̿��� �� ������ �Ͼ ���� ������ ���װ� �߻����� ��� �̰��� �ֽ�(unity communication  �Ǽ�) 
        arraymanager.SetActive(false);
        character_inventory_button = GameObject.FindGameObjectsWithTag("Character_inventory_Button");// tag�� �̿��� character inventory button ��������
        character_inventory = GameObject.FindWithTag("Character_inventory"); //tag�� �̿��� panel ��������
        character_inventory.SetActive(false);
        arrayButton = GameObject.FindWithTag("ArrayButton"); //tag�� �̿��� arrayButton ��������
        arrayButton.SetActive(false);
        null_Character = GameObject.FindGameObjectsWithTag("Null_Character"); // tag�� �̿��� nullcharacter ��������
        popup = GameObject.FindWithTag("Popup"); // tag�� �̿��� popup ��������
        popup.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) // �ӽ÷� ���� �ܰ�� �̵��ϱ� ���� ����
        {
            Debug_Manager();
            ui_order += 1;
        }

        Use_of_job_skills();

        Character_stat_distribution();

        Character_placement();
        
    }

    void Use_of_job_skills()// ���� �ɷ��� ����Ҷ� �Լ� �۵�
    {
        if(ui_order == orders.use_of_job_skills)
        {

        }

    }


    void Character_stat_distribution()// ĳ���� ���� �й� �����϶� �Լ� �۵�
    {
        if(ui_order== orders.character_stat_distribution)
        {
            if(arraybutton_On==0) //arraybutton�� �ѹ��� true�� �ǵ���(�̷��� ���� ���� ��� button�� �� �����϶� ������� ����)
            {
                arrayButton.SetActive(true);
                arraybutton_On++;
            }

            for (int i = 0; i < character_inventory_button.Length; i++) //�� �����Ͻ� ��ư�� ������� �ʵ���
            {
                character_inventory_button[i].SetActive(true);
            }

            for (int i = 0; i < null_Character.Length; i++) //��ġ�� ���������� �����ִ� nullcharacter�� �� ���ʶ��� off ��Ų��.
            {
                null_Character[i].SetActive(false);
            }



        }

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
            popup.SetActive(false);//�� ���ʽ� popbutton�� �������� �ʵ���
            
      
            
        }

    }

    void Debug_Manager() //����׿� �Լ�
    {
        Debug.Log(ui_order);
        Debug.Log(arraybutton_On);
        Debug.Log(character_inventory_button[0]);
    }
}
