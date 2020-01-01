using System.Collections;
using System.Collections.Generic;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class CommonSpawner : SpawnerBase
{
    protected override void OnEnable()
    {
        GameSceneManager.CreateEnemyEvent += CreateEnemyEvent_Handler;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        GameSceneManager.CreateEnemyEvent -= CreateEnemyEvent_Handler;
        base.OnDisable();
    }

    void CreateEnemyEvent_Handler(EnemyParams @params)
    {
        CreateEnemy(@params);
    }
}
