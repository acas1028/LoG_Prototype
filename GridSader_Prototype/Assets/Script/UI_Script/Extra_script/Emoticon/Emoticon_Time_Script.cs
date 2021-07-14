using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    GameObject emoticonBlock; //이모티콘이 나타나는 위치
    float time; //이모티콘이 화면에 노출되는 시간
    float animation_play_maxtime; // 이모티콘이 화면에 노출될수 있는 최대 시간을 가리키는 변수
    float Not_use_Time; //이모티콘을 사용할수 없는 시간
    float max_Not_use_Time; //이모티콘을 사용할수 없는 최대 시간을 가리키는 변수
    int emoticon_open_Count; // count를 재어 present카운트와 다른 경우 애니메이션 작동
    int present_emoticon_open_Count; //현재의 카운트를 저장하는 용도
    bool is_emoticon_block_on; //emoticonblock이 켜져있는가를 구별하는 bool 변수
    bool is_stop_emoticon; // 이모티콘 사용을 중단시킬 것인지를 구별하는 bool 변수

    List<float> repeat_limit_list = new List<float>(); //6초내 이모티콘 다수 사용을 방지하기 위한 list

    private void Awake()
    {
        emoticonBlock = GameObject.FindGameObjectWithTag("Emoticon_Block");
        emoticonBlock.SetActive(false);
    }

    private void Start()
    {
        time = 0;
        animation_play_maxtime = 3;
        Not_use_Time = 0;
        max_Not_use_Time = 10;
        emoticon_open_Count = 0;
        present_emoticon_open_Count = 0;
        is_emoticon_block_on = false;
        is_stop_emoticon = false;
    }

    private void Update()
    {
        if (is_stop_emoticon == false)
        {
            Is_Animation_on();
            List_Time_Flow();
            Vanish_List_Element();
            Full_List();
        }
        else
        {
            Flow_not_use_emoticon_time();
            Release_restrictions_Emoticon();
        }
        Animation_Off();
        Animation_Time_Flow();

    }

    void Is_Animation_on() //이모티콘 on off 애니메이션 함수
    {
        if(present_emoticon_open_Count != emoticon_open_Count)
        {
            if (is_emoticon_block_on == false)
            {
                emoticonBlock.SetActive(true);
                is_emoticon_block_on = true;
                present_emoticon_open_Count = emoticon_open_Count;
            }

            else
            {
                emoticonBlock.GetComponent<Animator>().Play("Emoticon_Click", -1, 0f);
                present_emoticon_open_Count = emoticon_open_Count;
            }
            repeat_limit_list.Add(0);

        }

    }

    void Animation_Time_Flow() //애니메이션 time 변수를 현재 시간에 알맞춰 흐르게 만드는 함수
    {
        if(is_emoticon_block_on==true)
        {
            time += Time.deltaTime;
        }
    }

    void Animation_Off() //이모티콘이 3초 화면이상 비춰지자 이를 꺼트리는 함수
    {
        if(time>=animation_play_maxtime)
        {
            emoticonBlock.GetComponent<Animator>().Play("Emoticon_Click_Disappear");
            if(emoticonBlock.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Emoticon_Click_Disappear"))
            {
                if(emoticonBlock.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
                {
                    emoticonBlock.SetActive(false);
                    is_emoticon_block_on = false;
                    time = 0; //이 부분에서 버그가 일어날수 있으니 유의
                }
            }
        }
    }

    void List_Time_Flow() //list 내부의 time flow 함수
    {
        for(int i=repeat_limit_list.Count-1; i>=0;i--) //remone에 관한 버그가 일어나지 않도록 for 내용을 이런식으로 변경
        {
            repeat_limit_list[i] += Time.deltaTime;
        }
    }

    void Vanish_List_Element() //list 내의 time의 한 요소가 6초 지났을 때 사라지게 만드는 함수
    {
        for (int i = repeat_limit_list.Count - 1; i >= 0; i--)
        {
            if (repeat_limit_list[i] >= 6)
            {
                repeat_limit_list.Remove(repeat_limit_list[i]);
            }
        }
    }

    void Full_List() //리스트가 5개 꽉차있는지를 확인하는 함수
    {
        if(repeat_limit_list.Count==5)
        {
            is_stop_emoticon = true;
        }
    }

    void Flow_not_use_emoticon_time() //이모티콘 사용하지 못하는 시간 flow 함수
    {
        if(is_stop_emoticon==true)
        {
            Not_use_Time += Time.deltaTime;
        }
    }

    void Release_restrictions_Emoticon() //이모티콘 제한을 푸는 함수
    {
        if(is_stop_emoticon==true && Not_use_Time>=max_Not_use_Time)
        {
            is_stop_emoticon = false;
            Not_use_Time = 0;
            repeat_limit_list.Clear();
        }
    }






    //get set plus 함수

    public int Get_Emoticon_open_Count()
    {
        return emoticon_open_Count;
    }
    public GameObject get_Emoticon_Block()
    {
        return emoticonBlock;
    }

    public bool Get_Is_Stop_Emoticon()
    {
        return is_stop_emoticon;
    }

    public void Set_Time(int Time)
    {
        time = Time;
    }

    public void Plus_Emoticon_open_Count(int count)
    {
        emoticon_open_Count += count;
    }

}
