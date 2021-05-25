using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //각각의 순서를 명시하는 변수
    private orders ui_order;

    

    private void Start()
    {
        ui_order = orders.use_of_job_skills;// 일단 job skill 사용을 처음으로 설정
    }

    private void Update()
    {
        
    }

    void GetMOuseclick_Vector() //레이 케스트를 이용한 방법으로 마우스 클릭 좌표 흭득
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 Mouse_Position = Input.mousePosition;
            

        }
    }


    void Character_stat_distribution()// 캐릭터 스탯 분배 차례일때 함수 작동
    {
        if(ui_order== orders.character_stat_distribution)
        {
            //일단 밑에 캐릭터 인벤토리 등장은 이 스크립트가 아닌 오브젝트 내에 스크립트를 추가시켜 지금 순서가 캐릭터 스텟분배이면 곧바로 나타나도록 설정.

            //인벤토리내 캐릭터 배치는 인벤토리의 크기를 설정하고 제작해야하므로 일단 잠시 보류.

            //레이캐스트 제작이 끝났을시 캐릭터 클릭-> 팝업창 정보 등장하도록 설정
            
        }

    }

    void Character_placement()// 캐릭터 배치 단계일때 함수 작동
    {
        if(ui_order==orders.character_placement)
        {

        }

    }
}
