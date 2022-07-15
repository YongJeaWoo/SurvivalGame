using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider backVolume;
    public AudioSource audio;

    float backVol = 0.8f;

    private void Start()
    {
        backVol = PlayerPrefs.GetFloat("backvol", 0.8f);
        backVolume.value = backVol;
        audio.volume = backVolume.value;
    }

    private void Update()
    {
        SoundSlider();
    }

    public void SoundSlider()
    {
        audio.volume = backVolume.value;

        backVol = backVolume.value;
        PlayerPrefs.SetFloat("backvol", backVol);
    }
}
