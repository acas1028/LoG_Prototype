using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    GameObject emoticonBlock; //�̸�Ƽ���� ��Ÿ���� ��ġ
    float time; //�̸�Ƽ���� ȭ�鿡 ����Ǵ� �ð�
    float animation_play_maxtime; // �̸�Ƽ���� ȭ�鿡 ����ɼ� �ִ� �ִ� �ð��� ����Ű�� ����
    float Not_use_Time; //�̸�Ƽ���� ����Ҽ� ���� �ð�
    float max_Not_use_Time; //�̸�Ƽ���� ����Ҽ� ���� �ִ� �ð��� ����Ű�� ����
    int emoticon_open_Count; // count�� ��� presentī��Ʈ�� �ٸ� ��� �ִϸ��̼� �۵�
    int present_emoticon_open_Count; //������ ī��Ʈ�� �����ϴ� �뵵
    bool is_emoticon_block_on; //emoticonblock�� �����ִ°��� �����ϴ� bool ����
    bool is_stop_emoticon; // �̸�Ƽ�� ����� �ߴܽ�ų �������� �����ϴ� bool ����

    List<float> repeat_limit_list = new List<float>(); //6�ʳ� �̸�Ƽ�� �ټ� ����� �����ϱ� ���� list

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

    void Is_Animation_on() //�̸�Ƽ�� on off �ִϸ��̼� �Լ�
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

    void Animation_Time_Flow() //�ִϸ��̼� time ������ ���� �ð��� �˸��� �帣�� ����� �Լ�
    {
        if(is_emoticon_block_on==true)
        {
            time += Time.deltaTime;
        }
    }

    void Animation_Off() //�̸�Ƽ���� 3�� ȭ���̻� �������� �̸� ��Ʈ���� �Լ�
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
                    time = 0; //�� �κп��� ���װ� �Ͼ�� ������ ����
                }
            }
        }
    }

    void List_Time_Flow() //list ������ time flow �Լ�
    {
        for(int i=repeat_limit_list.Count-1; i>=0;i--) //remone�� ���� ���װ� �Ͼ�� �ʵ��� for ������ �̷������� ����
        {
            repeat_limit_list[i] += Time.deltaTime;
        }
    }

    void Vanish_List_Element() //list ���� time�� �� ��Ұ� 6�� ������ �� ������� ����� �Լ�
    {
        for (int i = repeat_limit_list.Count - 1; i >= 0; i--)
        {
            if (repeat_limit_list[i] >= 6)
            {
                repeat_limit_list.Remove(repeat_limit_list[i]);
            }
        }
    }

    void Full_List() //����Ʈ�� 5�� �����ִ����� Ȯ���ϴ� �Լ�
    {
        if(repeat_limit_list.Count==5)
        {
            is_stop_emoticon = true;
        }
    }

    void Flow_not_use_emoticon_time() //�̸�Ƽ�� ������� ���ϴ� �ð� flow �Լ�
    {
        if(is_stop_emoticon==true)
        {
            Not_use_Time += Time.deltaTime;
        }
    }

    void Release_restrictions_Emoticon() //�̸�Ƽ�� ������ Ǫ�� �Լ�
    {
        if(is_stop_emoticon==true && Not_use_Time>=max_Not_use_Time)
        {
            is_stop_emoticon = false;
            Not_use_Time = 0;
            repeat_limit_list.Clear();
        }
    }






    //get set plus �Լ�

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
