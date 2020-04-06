using System;
using Puzzle;
using UnityEngine;

public class InfinityGameSceneManager : GameSceneManager
{
    public static event Action<float> ChangeDifficultyInfinitySpawner;
    public static event Action ChangeLevel;
        
    //////////////////
    //Event Invokers//
    //////////////////

    public void InvokeChangeDifficultyInfinitySpawner(float speed)
    {
        ChangeDifficultyInfinitySpawner?.Invoke(speed);
    }

    public void InvokeChangeLevel()
    {
        ChangeLevel?.Invoke();
    }
}