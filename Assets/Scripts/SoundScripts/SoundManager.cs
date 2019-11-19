using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip mainThemeClip;

    private AudioSource mainThemeSource;
    private AudioSource oneShotPlayer;
    private void Start()
    {
        PlayMainTheme();

        oneShotPlayer = new GameObject("OneShotPlayer").AddComponent<AudioSource>();
    }

    public void PauseSounds(bool pause)
    {
        AudioListener.pause = pause;
    }
    
    public void PlayOneShot(AudioClip audioClip, float volume = 1)
    {
        oneShotPlayer.PlayOneShot(audioClip, volume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    
    private void PlayMainTheme()
    {
        mainThemeSource = new GameObject("MainTheme " + mainThemeClip.name).AddComponent<AudioSource>();
        mainThemeSource.clip = mainThemeClip;
        mainThemeSource.Play();
    }
}
