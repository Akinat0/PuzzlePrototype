using System;
using Puzzle;
using UnityEngine;

public class InfinityGameSceneManager : GameSceneManager
{
    [SerializeField] public float startEnemySpeed;
    [SerializeField] public float startPatternTimeLineSpeed;
    public static event Action<float> ChangeDifficultyInfinitySpawnerEvent;
    public static event Action ChangeSoundEvent;
    public static event Action ChangePatternEvent;
    public static event Action<float> ChangeEnemySpeedEvent;
    public static event Action<float> ChangePatternTimeLineSpeedEvent;
        
    //////////////////
    //Event Invokers//
    //////////////////

    public void InvokeChangeDifficultyInfinitySpawner(float speed)
    {
        ChangeDifficultyInfinitySpawnerEvent?.Invoke(speed);
    }

    public void InvokeChangeTimeLine()
    {
        ChangeSoundEvent?.Invoke();
    }

    public void InvokeChangePattern()
    {
        ChangePatternEvent?.Invoke();
    }

    public void InvokeChangeEnemySpeed(float enemySpeed)
    {
        ChangeEnemySpeedEvent?.Invoke(enemySpeed);
    }

    public void InvokeChangePatternTimeLineSpeed(float timeLineSpeed)
    {
        ChangePatternTimeLineSpeedEvent?.Invoke(timeLineSpeed);
    }
}
