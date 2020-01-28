using System;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Timeline;

[Serializable]
public class EnemyMarker : Marker
{
    [SerializeField] public EnemyParams enemyParams;
}