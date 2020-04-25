using UnityEngine;

public class LevelPlayAudioEventArgs
{
    private AudioClip m_AudioClip;
    private bool m_Looped;
    private AnimationCurve m_SoundCurve;
    public AudioClip AudioClip => m_AudioClip;
    public AnimationCurve SoundCurve => m_SoundCurve;
    public bool Looped => m_Looped;

    public LevelPlayAudioEventArgs(AudioClip _AudioClip, bool _Looped, AnimationCurve _SoundCurve)
    {
        m_AudioClip = _AudioClip;
        m_Looped = _Looped;
        m_SoundCurve = _SoundCurve;
    }
}
