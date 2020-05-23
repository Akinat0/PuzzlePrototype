using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Puzzle;
using ScreensScripts;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioClip launcherTheme;

    private AudioSource currentThemeSource;
    private AudioSource oneShotPlayer;
    private void Awake()
    {
        Instance = this;
        PlayTheme(launcherTheme);
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

    private void PlayTheme(AudioClip clip)
    {
        if(currentThemeSource != null) 
            Destroy(currentThemeSource.gameObject);
        
        currentThemeSource = new GameObject("Theme " + clip.name).AddComponent<AudioSource>();
        currentThemeSource.clip = clip;
        currentThemeSource.Play();
    }

    public void PauseTheme()
    {
        if(currentThemeSource != null)
            currentThemeSource.Pause();
    }

    private void OnEnable()
    {
        LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler; 
        LauncherUI.GameSceneUnloadedEvent += GameSceneUnloadedEvent_Handler; 
    }

    private void OnDisable()
    {
        LauncherUI.PlayLauncherEvent -= PlayLauncherEvent_Handler;
        LauncherUI.GameSceneUnloadedEvent -= GameSceneUnloadedEvent_Handler;
    }

    void PlayLauncherEvent_Handler(PlayLauncherEventArgs _Args)
    {
        PauseTheme();   
    }
    
    void GameSceneUnloadedEvent_Handler()
    {
        PlayTheme(launcherTheme);   
    }
}
