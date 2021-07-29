using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audio_Mixer;


    public void SetLevel(float Slider_value) //볼륨의 높낮이를 slider로 조정하는 함수
    {
        audio_Mixer.SetFloat("BackGround_Music", Mathf.Log10(Slider_value) * 20);
    }

}
