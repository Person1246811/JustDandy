using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Slider MusicVolume;


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = MusicVolume.value;
    }

    private void Load()
    {
        MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
    }
}
