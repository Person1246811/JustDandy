using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxManager : MonoBehaviour
{
    [SerializeField] Slider sfxVolume;


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("SfxVolume"))
        {
            PlayerPrefs.SetFloat("SfxVolume", 1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = sfxVolume.value;
    }

    private void Load()
    {
        sfxVolume.value = PlayerPrefs.GetFloat("SfxVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume.value);
    }
}
