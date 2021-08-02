using System.Linq;
using UnityEngine;

public class TutorialSoundPlayer : LevelSoundPlayer
{
    [SerializeField, Range(0, 1)] float volumeOnPause = 0.5f; 
    private bool quietSound;
    protected override void Update()
    {
        foreach (AudioSource audioSource in AudioSources.Keys)
        {
            if(audioSource == null)
                continue;
            
            AnimationCurve soundCurve = AudioSources[audioSource];
            float soundCurveLength = soundCurve.keys.Last().time;
            float audioLength = audioSource.clip.length;
            float timeOnCurve = audioSource.time.Remap(0, audioLength, 0, soundCurveLength);
            float valueOnCurve = Mathf.Clamp01(soundCurve.Evaluate(timeOnCurve));
            
            //Additional volume remap in case of quite sound is active
            audioSource.volume = quietSound ? valueOnCurve.Remap(0, 1, 0, volumeOnPause) : valueOnCurve;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TutorialSceneManager.OnStopTutorial += OnStopTutorial_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        TutorialSceneManager.OnStopTutorial -= OnStopTutorial_Handler;
    }

    void OnStopTutorial_Handler(bool paused)
    {
        Pause(paused);
    }

    protected override void Pause(bool pause)
    {
        //In tutorial we will change audio volume instead of pause
        quietSound = pause;
    }
}
