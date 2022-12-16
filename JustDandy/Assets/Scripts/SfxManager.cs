using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    public AudioMixer Sound;


    public void ChangeSfxVolume(float slidervalue)
    {
        Sound.SetFloat("SfxVol", Mathf.Log10(slidervalue) * 20);
    }
}
