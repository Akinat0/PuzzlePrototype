using System;
using PuzzleScripts;
using Puzzle;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteSpawner : SpawnerBase
{
    [SerializeField] private float spawnTimestep = 1;

    private float _enemySpeed;
    public bool _spawn = false;

    private readonly float SPEED_FACTOR = 2;
    private readonly float TIME_SPAWN_FACTOR = 7;

    private float _timeFromStart = 0;
    private float _spawnTimer = 0;

    private float _difficulty = 0;

    private void Start()
    {
        _enemySpeed = 0;
    }

    private void Update()
    {
        if (!_spawn)
            return;

        if (_enemySpeed <= 0)
            return;

        //Update Timers
        _spawnTimer += Time.deltaTime;
        _timeFromStart += Time.deltaTime;


        if (_spawnTimer >= spawnTimestep)
        {
            EnemyParams enemyParams = new EnemyParams { };
            
            enemyParams.enemyType = EnemyType.Puzzle;
            enemyParams.speed = _enemySpeed;
            enemyParams.stickOut = Random.value > 0.5f;
            enemyParams.side = chooseEnemySide();

            CreateEnemy(enemyParams);

            _spawnTimer = 0;
        }
    }

    private Side chooseEnemySide()
    {
        if (Random.value * 20 < _difficulty)
            return (Side) (Random.value > 0.5f ? 1 : 3);
        else
            return (Side) Random.Range(0, 4);
    }

    private void ChangeSpeed(float diif)
    {
        _enemySpeed += diif / SPEED_FACTOR;
        Debug.Log("<color=#00FFCC>" + "Change speed: " + _enemySpeed + "</color>");
    }

    private void ChangeTimeStemp(float diff)
    {
        spawnTimestep -= diff / TIME_SPAWN_FACTOR;
        Debug.Log("<color=#00FFCC>" + "Change spawnTimestep: " + spawnTimestep + "</color>");
    }

    protected override void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawner += ChangeDifficultyInfinitySpawner_Handler;
        InfinityGameSceneManager.ChangeLevel += ChangeChangeLevel_Handler;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        InfinityGameSceneManager.ChangeDifficultyInfinitySpawner -= ChangeDifficultyInfinitySpawner_Handler;
        InfinityGameSceneManager.ChangeLevel -= ChangeChangeLevel_Handler;
        base.OnDisable();
    }

    void ResetLevelEvent_Handler()
    {
        _enemySpeed = 0;
        _timeFromStart = 0;
        _spawnTimer = 0;
    }

    void PauseLevelEvent_Handler(bool pause)
    {
        _spawn = !pause;
    }

    void ChangeDifficultyInfinitySpawner_Handler(float diff)
    {
        if (_enemySpeed == 0)
        {
            ChangeSpeed(diff * SPEED_FACTOR);
        }
        else
        {
            //Condition ending spawn
            if (diff == -100)
            {
                //Set speed = 0
                ChangeTimeStemp(-_enemySpeed * SPEED_FACTOR);
                return;
            }
            
            if (Random.value > 0.6f)
            {
                if (_enemySpeed + (diff) < 3)
                {
                    _enemySpeed = 3;
                    ChangeTimeStemp(diff);
                }
                else if (_enemySpeed + (diff) > 8)
                {
                    _enemySpeed = 8;
                    ChangeTimeStemp(diff);
                }
                else
                    ChangeSpeed(diff);
            }
            else
            {
                if (spawnTimestep - (diff / TIME_SPAWN_FACTOR) < 0.5f)
                {
                    spawnTimestep = 0.5f;
                    ChangeSpeed(diff);
                }
                else if (spawnTimestep - (diff / TIME_SPAWN_FACTOR) > 2f)
                {
                    spawnTimestep = 2f;
                    ChangeSpeed(diff);
                }
                else
                    ChangeTimeStemp(diff);
            }
        }
        _difficulty += diff;
        Debug.Log("<color=#FFFF00>" + "Change difficulty: " + _difficulty + "</color>");
    }

    void ChangeChangeLevel_Handler()
    {
        _enemySpeed = 0;
        _timeFromStart = 0;
        _spawnTimer = 0;
        spawnTimestep = 1;
    }
}