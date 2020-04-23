using System;
using PuzzleScripts;
using Puzzle;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteSpawner : SpawnerBase
{
    [SerializeField] private float enemySpeedDenominator;
    [SerializeField] private float patternTimeLineSpeedDenominator;

    private float _difficulty;
    private float _enemySpeed;
    private float _patternTimeLineSpeed;

    private void Start()
    {
        if (GameSceneManager.Instance is InfinityGameSceneManager instance)
        {
            _enemySpeed = instance.startEnemySpeed;
            _patternTimeLineSpeed = instance.startPatternTimeLineSpeed;
        }

        _difficulty = _enemySpeed + _patternTimeLineSpeed;
    }

    private void ChangeEnemySpeed(float diff)
    {
        _enemySpeed += diff / (_difficulty / enemySpeedDenominator);
        Debug.Log("<color=#00FFCC>" + "Change speed: " + _enemySpeed + "</color>");
    }

    private void ChangePatternTimeLineSpeed(float diff)
    {
        _patternTimeLineSpeed += diff / _difficulty / patternTimeLineSpeedDenominator;
        Debug.Log("<color=#00FFCC>" + "Change spawnTimestep: " + _patternTimeLineSpeed + "</color>");
    }

    protected override void OnEnable()
    {
        GameSceneManager.CreateEnemyEvent += CreateEnemyEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawnerEvent += ChangeDifficultyInfinitySpawner_Handler;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        GameSceneManager.CreateEnemyEvent -= CreateEnemyEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawnerEvent -= ChangeDifficultyInfinitySpawner_Handler;
        base.OnDisable();
    }

    void CreateEnemyEvent_Handler(EnemyParams @params)
    {
        CreateEnemy(@params);
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
}
