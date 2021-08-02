using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    AudioSource themePlayer;
    AudioSource oneShotPlayer;

    IEnumerator changeThemeVolumeRoutine;

    AudioSource OneShotPlayer => oneShotPlayer ? oneShotPlayer 
        : oneShotPlayer = new GameObject("OneShotPlayer").AddComponent<AudioSource>();
    
    AudioSource ThemePlayer
    {
        get
        {
            if (themePlayer == null)
            {
                themePlayer = new GameObject("ThemePlayer").AddComponent<AudioSource>();
                themePlayer.loop = true;
            }

            return themePlayer;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void PlayOneShot(AudioClip audioClip, float volume = 1)
    {
        OneShotPlayer.PlayOneShot(audioClip, volume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void PlayTheme(AudioClip theme, float delay = 0)
    {
        if(ThemePlayer.clip != null && ThemePlayer.clip == theme)
            return;
        
        ThemePlayer.Stop();
        
        ThemePlayer.gameObject.name = $"Theme_{theme.name}";
        ThemePlayer.clip = theme;
        
        if(Mathf.Approximately(delay, 0))
            ThemePlayer.Play();
        else
            ThemePlayer.PlayDelayed(delay);
    }

    public void StopTheme()
    {
        ThemePlayer.clip = null;
        ThemePlayer.Stop();
    }

    public void SetThemeVolume(float volume)
    {
        ThemePlayer.volume = volume;
    }
    
    public void ChangeThemeVolume(float targetVolume, float duration, Action finished = null)
    {
        if(changeThemeVolumeRoutine != null)
            StopCoroutine(changeThemeVolumeRoutine);

        StartCoroutine(changeThemeVolumeRoutine = ChangeThemeVolumeRoutine(targetVolume, duration, finished));
    }

    IEnumerator ChangeThemeVolumeRoutine(float targetVolume, float duration, Action finished)
    {
        float sourceVolume = ThemePlayer.volume;
        float time = 0;

        while (time < duration)
        {
            yield return null;
            time += Time.deltaTime;

            ThemePlayer.volume = Mathf.Lerp(sourceVolume, targetVolume, time / duration);
        }
        
        ThemePlayer.volume = targetVolume;
        finished?.Invoke();
    }
}
