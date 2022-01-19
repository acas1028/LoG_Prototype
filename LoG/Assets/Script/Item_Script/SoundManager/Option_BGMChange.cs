using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option_BGMChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = BGMManager.Instance.BGMValue;
    }

    public void BGMvolumeChange()
    {
        BGMManager.Instance.BGMValue = GetComponent<Slider>().value;
    }
}
