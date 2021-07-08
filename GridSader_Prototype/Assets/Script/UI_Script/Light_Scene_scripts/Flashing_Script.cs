using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing_Script : MonoBehaviour
{
    Text IbL;
    float BlinkFadeInTime;
    float BlinkStayTime;
    float BlinkFadeOutTime;
    private float _timeChecker;
    private Color _color;
    private void Start()
    {
        BlinkFadeInTime = 0.5f;
        BlinkStayTime = 0.8f;
        BlinkFadeOutTime = 0.7f;
        _timeChecker = 0;
        IbL = GetComponent<Text>();
        _color = IbL.color;
    }

    private void Update()
    {
        Flash_Text();
    }


    void Flash_Text()
    {
        _timeChecker += Time.deltaTime;
        if (_timeChecker < BlinkFadeInTime)
        {
            IbL.color = new Color(_color.r, _color.g, _color.b, _timeChecker / BlinkFadeInTime);
        }

        else if (_timeChecker < BlinkFadeInTime + BlinkStayTime)
        {
            IbL.color = new Color(_color.r, _color.g, _color.b, 1);
        }
        else if (_timeChecker < BlinkFadeInTime + BlinkStayTime + BlinkFadeOutTime)
        {
            IbL.color = new Color(_color.r, _color.g, _color.b, 1 - (_timeChecker - (BlinkFadeInTime + BlinkStayTime)) / BlinkFadeOutTime);
        }
        else
        {
            _timeChecker = 0;
        }
    }


}
