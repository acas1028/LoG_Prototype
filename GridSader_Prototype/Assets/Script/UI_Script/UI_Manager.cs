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
        character_placement , character_choose_grid_property
    }

   
    private orders ui_order; //각각의 순서를 명시하는 변수
    private GameObject arraymanager; //오브젝트 arraymanager
    private GameObject[] null_Character; //오브젝트 nullcharacter
    private GameObject popup;

    private int arraybutton_On = 0;
    private void Start()
    {
        popup = GameObject.FindGameObjectWithTag("Popup"); // tag를 이용한 popup창 가져오기
        popup.SetActive(false);
        ui_order = orders.character_placement;
        arraymanager = GameObject.FindWithTag("ArrayManager"); // find를 이용할 시 오류가 일어날 수가 있으니 버그가 발생했을 경우 이곳을 주시(unity communication  피셜) 
        arraymanager.SetActive(false);
        null_Character = GameObject.FindGameObjectsWithTag("Null_Character"); // tag를 이용한 nullcharacter 가져오기
    }

    private void Update()
    { 
        Character_placement();

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
     
        }

    }

    void Charactger_chooose_grid_property() // 칸 지정 특성을 지닌 캐릭터가 배치 될 시, 특성을 사용하는 때
    {

    }

    void Debug_Manager() //디버그용 함수
    {
        Debug.Log(ui_order);
        Debug.Log(arraybutton_On);
    }
}
