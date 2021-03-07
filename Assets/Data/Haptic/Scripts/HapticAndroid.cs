#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

class HapticAndroid : Haptic
{
    #region constants

    const long DURATION_SHORT  = 20;
    const long DURATION_MEDIUM = 40;
    const long DURATION_LONG   = 80;
    const int  AMP_LOW         = 40;
    const int  AMP_MEDIUM      = 128;
    const int  AMP_HIGH        = 255;

    const int NO_REPEAT = -1;

    static readonly Dictionary<Type, Pattern> m_Patterns = new Dictionary<Type, Pattern>
    {
        {
            Type.SELECTION,
            new Pattern(DURATION_SHORT, AMP_LOW)
        },
        {
            Type.SUCCESS, new Pattern(
                    new List<long> { DURATION_SHORT, DURATION_SHORT, DURATION_LONG },
                    new List<int> { AMP_LOW, 0, AMP_HIGH }
                )
        },
        {
            Type.WARNING,
            new Pattern(
                    new List<long> { DURATION_LONG, DURATION_SHORT, DURATION_MEDIUM },
                    new List<int> { AMP_HIGH, 0, AMP_MEDIUM }
                )
        },
        {
            Type.FAILURE,
            new Pattern(
                    new List<long> { DURATION_MEDIUM, DURATION_SHORT, DURATION_MEDIUM, DURATION_SHORT, DURATION_LONG, DURATION_SHORT, DURATION_SHORT },
                    new List<int> { AMP_MEDIUM, 0, AMP_MEDIUM, 0, AMP_HIGH, 0, AMP_LOW }
                )
        },
        {
            Type.IMPACT_LIGHT,
            new Pattern(DURATION_SHORT, AMP_LOW)
        },
        {
            Type.IMPACT_MEDIUM,
            new Pattern(DURATION_MEDIUM, AMP_MEDIUM)
        },
        {
            Type.IMPACT_HEAVY,
            new Pattern(DURATION_LONG, AMP_HIGH)
        }
    };

    #endregion

    #region nested types

    class Pattern
    {
        public readonly long[] Duration;
        public readonly int[] Amplitude;

        public Pattern(List<long> _Duration, List<int> _Amplitude)
        {
            _Duration.Insert(0, 0);
            Duration = _Duration.ToArray();

            _Amplitude.Insert(0, 0);
            Amplitude = _Amplitude.ToArray();
        }

        public Pattern(long _Duration, int _Amplitude)
        {
            Duration  = new[] { 0, _Duration };
            Amplitude = new[] { 0, _Amplitude };
        }
    }

    #endregion
    
    #region service methods
    
    protected override void RunInternal(Haptic.Type _Type)
    {
        if (_Type == Type.DEFAULT)
        {
            Handheld.Vibrate();
            return;
        }

        Pattern pattern;

        if (!m_Patterns.TryGetValue(_Type, out pattern))
        {
            Debug.LogError("Unknown haptic type : " + _Type);
            return;
        }

        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
            {

                if (HWUtils.ApiVersion < 26)
                {
                    vibrator.Call("vibrate", pattern.Duration, NO_REPEAT);
                }
                else
                {
                    using (AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
                    using (AndroidJavaObject waveform = vibrationEffect.CallStatic<AndroidJavaObject>(
                            "createWaveform",
                            pattern.Duration,
                            pattern.Amplitude,
                            NO_REPEAT
                        ))
                    {
                        vibrator.Call("vibrate", waveform);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to run haptic : " + e.Message);
        }
    }
    
    #endregion
}

#endif