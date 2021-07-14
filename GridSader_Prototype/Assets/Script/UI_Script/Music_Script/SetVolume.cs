using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audio_Mixer;


    public void SetLevel(float Slider_value)
    {
        audio_Mixer.SetFloat("BackGround_Music", Mathf.Log10(Slider_value) * 20);
    }

}
