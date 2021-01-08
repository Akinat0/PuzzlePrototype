using System.Collections.Generic;
using System.Linq;
using AudioVisualization;
using Puzzle;
using UnityEngine;


public class LevelSoundPlayer : MonoBehaviour
{
    protected Dictionary<AudioSource, AnimationCurve> audioSources = new Dictionary<AudioSource, AnimationCurve>();

    AudioDataSource audioDataSource;

    void Awake()
    {
        audioDataSource = GetComponent<AudioDataSource>();
    }

    protected virtual void Update()
    {
        if(audioSources.Count == 0)
            return;
        
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

    private void OnDestroy()
    {
        ClearAudio();
    }

    protected void ClearAudio()
    {
        foreach (AudioSource audioSource in audioSources.Keys)
            Destroy(audioSource);
        
        audioSources.Clear();
    }

    protected virtual void Pause(bool pause)
    {
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
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        TimeManager.DefaultTimeScaleValueChanged += DefaultTimeScaleValueChanged_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.PlayAudioEvent -= PlayAudioEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
        TimeManager.DefaultTimeScaleValueChanged -= DefaultTimeScaleValueChanged_Handler;
    }

    void PlayAudioEvent_Handler(LevelPlayAudioEventArgs args)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(audioSource, args.SoundCurve);
        audioSource.clip = args.AudioClip;
        audioSource.loop = args.Looped;
        audioSource.time = (float)args.Time;
        if (!args.Looped)
            this.Invoke(() =>
                {
                    audioSources.Remove(audioSource);
                    Destroy(audioSource);
                },
                audioSource.clip.length);
        
        audioSource.pitch = TimeManager.TimeScale;

        audioSource.Play();
        
        if(audioDataSource != null)
            audioDataSource.AttachAudioSource(audioSource);
    }
    
    void PauseLevelEvent_Handler(bool paused)
    {
        Pause(paused);
    }
    
    void ResetLevelEvent_Handler()
    {
        ClearAudio();
    }

    void LevelClosedEvent_Handler()
    {
        ClearAudio();
    }

    void DefaultTimeScaleValueChanged_Handler(float timeScale)
    {
        foreach (AudioSource audioSource in audioSources.Keys)
            audioSource.pitch = timeScale;
    }

}
