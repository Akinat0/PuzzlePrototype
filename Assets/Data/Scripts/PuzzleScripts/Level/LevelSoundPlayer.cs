using System.Collections.Generic;
using System.Linq;
using Puzzle;
using UnityEngine;

public class LevelSoundPlayer : MonoBehaviour
{
    Dictionary<AudioSource, AnimationCurve> audioSources = new Dictionary<AudioSource, AnimationCurve>();
    private void OnEnable()
    {
        GameSceneManager.PlayAudioEvent += PlayAudioEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.PlayAudioEvent -= PlayAudioEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }
    
    private void Update()
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
        foreach (var audioSource in audioSources.Keys)
        {
            if(paused)
                audioSource.Pause();
            else
                audioSource.UnPause();
        }
    }
    
    void ResetLevelEvent_Handler()
    {
        audioSources.Clear();
    }
}
