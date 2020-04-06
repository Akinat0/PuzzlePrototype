using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//Kind of crutch
[Serializable]
public class ChangeDifficultyMarker : Marker, INotification
{
    [SerializeField] public DifficultyParams difficultyParams;
    public PropertyName id { get; }
}

[Serializable]
public struct DifficultyParams
{
    public float difficulty;
}
