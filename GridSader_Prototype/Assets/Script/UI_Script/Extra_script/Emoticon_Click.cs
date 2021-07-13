using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Emoticon_Click : MonoBehaviour
{
    GameObject emoticon_Block;

    public float time;
    bool is_appear;

    private void Start()
    {
        time = 0;
        is_appear = false;
        emoticon_Block = GameObject.FindGameObjectWithTag("Emoticon_Block");
        emoticon_Block.SetActive(false);
    }

    private void Update()
    {
        Time_Flow();
        Emoticon_off();
    }

    public void Emoticon_Button_Click()
    {
        if (is_appear == false)
        {
            emoticon_Block.SetActive(true);
            emoticon_Block.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            is_appear = true;
        }
    }

    void Time_Flow()
    {
        if(is_appear==true)
        {
            time += Time.deltaTime;
        }
    }

    void Emoticon_off()
    {
        if(time>=3)
        {
            emoticon_Block.GetComponent<Animator>().SetInteger("Is_appear", 1);
            if(emoticon_Block.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Emoticon_Click_Disappear"))
            {
                if (emoticon_Block.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
                {
                    is_appear = false;
                    emoticon_Block.SetActive(false);
                    time = 0;
                }
            }
        }
    }
}
