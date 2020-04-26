using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Puzzle;
using UnityEngine;

public class LevelSoundPlayer : MonoBehaviour
{
    protected Dictionary<AudioSource, AnimationCurve> audioSources = new Dictionary<AudioSource, AnimationCurve>();
    
    protected virtual void Update()
    {
        foreach (AudioSource audioSource in audioSources.Keys)
        {
            if(audioSource == null)
                continue;
            
            AnimationCurve soundCurve = audioSources[audioSource];
            float soundCurveLength = soundCurve.keys.Last().time;
            float audioLength = audioSource.clip.length;
            float timeOnCurve = audioSource.time.Remap(0, audioLength, 0, soundCurveLength);
            audioSource.volume = Mathf.Clamp01(soundCurve.Evaluate(timeOnCurve));
        }
    }

    protected void ClearAudio()
    {
        foreach (AudioSource audioSource in audioSources.Keys)
            Destroy(audioSource);
        
        audioSources.Clear();
    }

    protected void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        
        foreach (var audioSource in audioSources.Keys)
        {
            if(audioSource == null)
                continue;

            if(pause)
                audioSource.Pause();
            else
                audioSource.UnPause();
        }
    }
    
    protected virtual void OnEnable()
    {
        GameSceneManager.PlayAudioEvent += PlayAudioEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.PlayAudioEvent -= PlayAudioEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }



    void PlayAudioEvent_Handler(LevelPlayAudioEventArgs args)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(audioSource, args.SoundCurve);
        audioSource.clip = args.AudioClip;
        audioSource.loop = args.Looped;
        if (!args.Looped)
            this.Invoke(() =>
                {
                    audioSources.Remove(audioSource);
                    Destroy(audioSource);
                },
                audioSource.clip.length);

        audioSource.Play();
    }
    
    void PauseLevelEvent_Handler(bool paused)
    {
        Pause(paused);
    }
    
    void ResetLevelEvent_Handler()
    {
        ClearAudio();
    }

}
