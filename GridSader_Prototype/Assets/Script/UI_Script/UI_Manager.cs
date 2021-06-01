using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static UI_Manager _instance;

    // 인스턴스에 접근하기 위한 프로퍼티
    private UI_Manager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }



    }

    //순서 변수를 인트화시켜 보기 편화게 설정
    enum orders
    {
        use_of_job_skills, character_stat_distribution, character_placement
    }

   
    private orders ui_order; //각각의 순서를 명시하는 변수
    private GameObject arraymanager; //오브젝트 arraymanager
    private GameObject character_inventory; //오브젝트 panel
    private GameObject arrayButton; // 오브젝트 arraybutton
    private GameObject popup; // 오브젝트 popup
    private GameObject[] character_inventory_button; // 오브젝트 inventory(캐릭터 관련 버튼)
    private GameObject[] null_Character; //오브젝트 nullcharacter

    private int arraybutton_On = 0;

    private void Start()
    {
        ui_order = orders.use_of_job_skills;// 일단 job skill 사용을 처음으로 설정
        arraymanager = GameObject.FindWithTag("ArrayManager"); // find를 이용할 시 오류가 일어날 수가 있으니 버그가 발생했을 경우 이곳을 주시(unity communication  피셜) 
        arraymanager.SetActive(false);
        character_inventory_button = GameObject.FindGameObjectsWithTag("Character_inventory_Button");// tag를 이용한 character inventory button 가져오기
        character_inventory = GameObject.FindWithTag("Character_inventory"); //tag를 이용한 panel 가져오기
        character_inventory.SetActive(false);
        arrayButton = GameObject.FindWithTag("ArrayButton"); //tag를 이용한 arrayButton 가져오기
        arrayButton.SetActive(false);
        null_Character = GameObject.FindGameObjectsWithTag("Null_Character"); // tag를 이용한 nullcharacter 가져오기
        popup = GameObject.FindWithTag("Popup"); // tag를 이용한 popup 가져오기
        popup.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) // 임시로 다음 단계로 이동하기 위한 수단
        {
            Debug_Manager();
            ui_order += 1;
        }

        Use_of_job_skills();

        Character_stat_distribution();

        Character_placement();
        
    }

    void Use_of_job_skills()// 직업 능력을 사용할때 함수 작동
    {
        if(ui_order == orders.use_of_job_skills)
        {

        }

    }


    void Character_stat_distribution()// 캐릭터 스탯 분배 차례일때 함수 작동
    {
        if(ui_order== orders.character_stat_distribution)
        {
            if(arraybutton_On==0) //arraybutton이 한번만 true가 되도록(이렇게 하지 않을 경우 button이 이 차례일때 사라지지 않음)
            {
                arrayButton.SetActive(true);
                arraybutton_On++;
            }

            for (int i = 0; i < character_inventory_button.Length; i++) //이 차례일시 버튼이 사라지지 않도록
            {
                character_inventory_button[i].SetActive(true);
            }

            for (int i = 0; i < null_Character.Length; i++) //배치에 직접적으로 관련있는 nullcharacter를 이 차례때는 off 시킨다.
            {
                null_Character[i].SetActive(false);
            }



        }

    }

    void Character_placement()// 캐릭터 배치 단계일때 함수 작동
    {
        if(ui_order==orders.character_placement)
        {
            
            arraymanager.SetActive(true);
            for(int i=0; i<null_Character.Length;i++) //배치에 직접적으로 관련있는 nullCharacter를 다시 on시킨다.
            {
                null_Character[i].SetActive(true);
            }
            popup.SetActive(false);//이 차례시 popbutton이 등장하지 않도록
            
      
            
        }

    }

    void Debug_Manager() //디버그용 함수
    {
        Debug.Log(ui_order);
        Debug.Log(arraybutton_On);
        Debug.Log(character_inventory_button[0]);
    }
}
