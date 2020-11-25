using UnityEngine;

public class LevelPlayAudioEventArgs
{
    private AudioClip m_AudioClip;
    private bool m_Looped;
    private AnimationCurve m_SoundCurve;
    private double m_Time;
    public AudioClip AudioClip => m_AudioClip;
    public AnimationCurve SoundCurve => m_SoundCurve;
    public bool Looped => m_Looped;
    public double Time => m_Time;

    public LevelPlayAudioEventArgs(AudioClip _AudioClip, bool _Looped, AnimationCurve _SoundCurve, double _Time = 0)
    {
        m_AudioClip = _AudioClip;
        m_Looped = _Looped;
        m_SoundCurve = _SoundCurve;
        m_Time = _Time;
    }
}
