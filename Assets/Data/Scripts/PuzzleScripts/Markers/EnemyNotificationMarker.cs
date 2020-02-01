using System;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//Kind of crutch
[Serializable]
public class EnemyNotificationMarker : Marker, INotification
{
    [SerializeField] public EnemyParams enemyParams;

    public PropertyName id { get; }
}