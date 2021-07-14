using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    GameObject emoticonBlock;
    public float time;
    float animation_play_maxtime;
    float Not_use_Time;
    float max_Not_use_Time;
    int emoticon_open_Count; // count를 재어 present카운트와 다른 경우 애니메이션 작동
    int present_emoticon_open_Count; //현재의 카운트를 저장하는 용도
    bool is_emoticon_block_on;
    bool is_stop_emoticon;

    public List<float> repeat_limit_list = new List<float>();

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

    void Is_Animation_on()
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

    void Animation_Time_Flow()
    {
        if(is_emoticon_block_on==true)
        {
            time += Time.deltaTime;
        }
    }

    void Animation_Off()
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

    void List_Time_Flow()
    {
        for(int i=repeat_limit_list.Count-1; i>=0;i--) //remone에 관한 버그가 일어나지 않도록 for 내용을 이런식으로 변경
        {
            repeat_limit_list[i] += Time.deltaTime;
        }
    }

    void Vanish_List_Element()
    {
        for (int i = repeat_limit_list.Count - 1; i >= 0; i--)
        {
            if (repeat_limit_list[i] >= 6)
            {
                repeat_limit_list.Remove(repeat_limit_list[i]);
            }
        }
    }

    void Full_List()
    {
        if(repeat_limit_list.Count==5)
        {
            is_stop_emoticon = true;
        }
    }

    void Flow_not_use_emoticon_time()
    {
        if(is_stop_emoticon==true)
        {
            Not_use_Time += Time.deltaTime;
        }
    }

    void Release_restrictions_Emoticon()
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
