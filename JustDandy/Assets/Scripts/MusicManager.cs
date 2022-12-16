using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer music;


    public void ChangeMusicVolume(float slidervalue)
    {
        music.SetFloat("MusicVol", Mathf.Log10(slidervalue) * 20);
    }

}
