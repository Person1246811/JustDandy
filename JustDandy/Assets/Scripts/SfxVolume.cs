using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxVolume : MonoBehaviour
{
    public AudioMixer sound;


    public void ChangeSfxVolume(float slidervalue)
    {
        sound.SetFloat("SfxVol", Mathf.Log10(slidervalue) * 20);
    }
}
