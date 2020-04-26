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

    public AnimationCurve SoundCurve => soundCurve;
    public AudioClip AudioClip => audioClip;
    public bool Looped => looped;

    public PropertyName id { get; }
}
