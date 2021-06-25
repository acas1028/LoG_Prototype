using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadySeconds : MonoBehaviour
{
    public ReadySeconds instance;
    public Text Seconds;
    private float Count_Seconds = 0;
    private bool GetReady = false;
    public bool Notcancle = false;
    private void Start()
    {
        instance = this;
    }
    void Update()
    {
        if(GetReady==true)
        {
            if (Count_Seconds <= 2)
            {
                Count_Seconds += Time.deltaTime;
                Debug.Log(Count_Seconds);
                Seconds.text = Mathf.Ceil(Count_Seconds).ToString();
            }
            if(Count_Seconds>2)
            {
                Notcancle = true;
            }
        }
        if(GetReady==false)
        {
            Count_Seconds = 0;
            Notcancle = false;
        }
    }
    private void OnEnable()
    {
        GetReady = true;
    }
    private void OnDisable()
    {
        GetReady = false;
        Count_Seconds = 0;
    }
}
