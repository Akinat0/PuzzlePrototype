
using System;
using UnityEngine;

public static class TimeManager
{
    public static event Action<float> DefaultTimeScaleValueChanged; 
    
    static float defaultTimeScale = 1;
    
    public static float DefaultTimeScale
    {
        get => defaultTimeScale;

        set
        {
            defaultTimeScale = value;

            if (!IsPaused)
                Unpause();

            DefaultTimeScaleValueChanged?.Invoke(DefaultTimeScale);
        }
    }

    public static float TimeScale => Time.timeScale;

    public static bool IsPaused { get; private set; }
    
    public static void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        IsPaused = false;
        Time.timeScale = DefaultTimeScale;
    }
}
