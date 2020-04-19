using System;
using PuzzleScripts;
using Puzzle;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteSpawner : SpawnerBase
{
    private float _difficulty;
    private float _enemySpeed;
    private float _patternTimeLineSpeed;

    private void Start()
    {
        _enemySpeed = 4;
        _patternTimeLineSpeed = 1;
        _difficulty = 5;
    }

    private void ChangeEnemySpeed(float diif)
    {
        _enemySpeed += diif / (_difficulty / 2);
        Debug.Log("<color=#00FFCC>" + "Change speed: " + _enemySpeed + "</color>");
    }

    private void ChangePatternTimeLineSpeed(float diff)
    {
        _patternTimeLineSpeed += diff / _difficulty / 5;
        Debug.Log("<color=#00FFCC>" + "Change spawnTimestep: " + _patternTimeLineSpeed + "</color>");
    }

    protected override void OnEnable()
    {
        GameSceneManager.CreateEnemyEvent += CreateEnemyEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawnerEvent += ChangeDifficultyInfinitySpawner_Handler;
        InfinityGameSceneManager.ChangeSoundEvent += ChangeSound_Handler;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        GameSceneManager.CreateEnemyEvent -= CreateEnemyEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawnerEvent -= ChangeDifficultyInfinitySpawner_Handler;
        InfinityGameSceneManager.ChangeSoundEvent -= ChangeSound_Handler;
        base.OnDisable();
    }

    void CreateEnemyEvent_Handler(EnemyParams @params)
    {
        CreateEnemy(@params);
    }

    void ResetLevelEvent_Handler()
    {
    }

    void ChangeDifficultyInfinitySpawner_Handler(float diff)
    {
        _difficulty += diff;
        if (GameSceneManager.Instance is InfinityGameSceneManager instance)
        {
            if (Random.value >= 0.5)
            {
                ChangeEnemySpeed(diff);
                instance.InvokeChangeEnemySpeed(_enemySpeed);
            }
            else
            {
                ChangePatternTimeLineSpeed(diff);
                instance.InvokeChangePatternTimeLineSpeed(_patternTimeLineSpeed);
            }
        }

    }

    void ChangeSound_Handler()
    {
    }
}