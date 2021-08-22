
using System;
using System.Collections;
using Abu.Tools;
using UnityEngine;

public static class TimeManager
{
    public static event Action<float> TimeScaleValueChanged; 
    
    static float defaultTimeScale = 1;
    
    public static float DefaultTimeScale
    {
        get => defaultTimeScale;

        set
        {
            if(Mathf.Approximately(DefaultTimeScale, value))
                return;
            
            defaultTimeScale = value;

            TimeScale = Mathf.Clamp(TimeScale, 0, DefaultTimeScale);
        }
    }

    public static float TimeScale
    {
        get => Time.timeScale;
        private set
        {
            value = Mathf.Clamp(value, 0, DefaultTimeScale);
            
            if(Mathf.Approximately(Time.timeScale, value))
                return;

            Time.timeScale = value;
            
            TimeScaleValueChanged?.Invoke(Time.timeScale);
        }
    }

    static IEnumerator currentUnpauseRoutine;

    public static void Pause()
    {
        if(currentUnpauseRoutine != null)
            CoroutineHelper.StopRoutine(currentUnpauseRoutine);
        
        TimeScale = 0;
    }

    public static void Unpause(bool instant = true)
    {
        if(currentUnpauseRoutine != null)
            CoroutineHelper.StopRoutine(currentUnpauseRoutine);

        currentUnpauseRoutine = null; //clear this anyway
        
        if (instant)
            TimeScale = DefaultTimeScale;
        else
            CoroutineHelper.StartRoutine(currentUnpauseRoutine = UnpauseRoutine(1));
    }

    static IEnumerator UnpauseRoutine(float duration)
    {
        float time = 0;
        float startTimeScale = TimeScale;
        
        while (time <= duration)
        {
            float value = Mathf.Lerp(startTimeScale, DefaultTimeScale, time / duration);
            TimeScale = value * value; //apply quadratic function instead of linear
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        TimeScale = DefaultTimeScale;

        currentUnpauseRoutine = null;
    }
}
