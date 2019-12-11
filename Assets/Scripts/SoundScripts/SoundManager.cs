using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Puzzle;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    
    [SerializeField] private AudioClip mainThemeClip;

    private AudioSource mainThemeSource;
    private AudioSource oneShotPlayer;
    private void Start()
    {
        Instance = this;
        PlayMainTheme();
        oneShotPlayer = new GameObject("OneShotPlayer").AddComponent<AudioSource>();
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
        if(mainThemeSource != null) 
            Destroy(mainThemeSource.gameObject);
        
        mainThemeSource = new GameObject("MainTheme " + mainThemeClip.name).AddComponent<AudioSource>();
        mainThemeSource.clip = mainThemeClip;
        mainThemeSource.Play();
    }

    public void PauseSounds(bool pause)
    {
        AudioListener.pause = pause;
    }
    public void PauseMainTheme()
    {
        mainThemeSource.Pause();
    }

    private void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;

    }

    void ResetLevelEvent_Handler()
    {
        PlayMainTheme();
    }

    void PauseLevelEvent_Handler(bool pause)
    {
        PauseSounds(pause);
    }
}
