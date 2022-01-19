using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option_SoundChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = SoundManager.Instance.SoundValue;
    }

    public void SoundvolumeControl()
    {
        SoundManager.Instance.SoundValue = GetComponent<Slider>().value;
    }
}
