using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("PlayAudioMarker"), Serializable]
public class PlayAudioMarker : Marker, INotification
{
    [SerializeField] private bool looped;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AnimationCurve soundCurve = AnimationCurve.Constant(0, 1, 1);

    public AnimationCurve SoundCurve
    {
        get => soundCurve;
        set => soundCurve = value;
    }

    public AudioClip AudioClip
    {
        get => audioClip;
        set => audioClip = value;
    }

    public bool Looped
    {
        get => looped;
        set => looped = value;
    }

    public PropertyName id { get; }
}
