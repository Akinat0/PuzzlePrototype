using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class GameEnemiesCounter
{
    public int ExistingEnemies { get; private set; }
    public int EnemiesOnScreen { get; private set; }


    public GameEnemiesCounter()
    {
        GameSceneManager.CreateEnemyEvent += CreateEnemyEvent_Handler;
        GameSceneManager.EnemyAppearedOnScreenEvent += EnemyAppearedOnScreenEvent_Handler;
        GameSceneManager.EnemyDiedEvent += EnemyDiedEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    void CreateEnemyEvent_Handler(EnemyParams _)
    {
        ExistingEnemies++;
    }
    
    void EnemyAppearedOnScreenEvent_Handler(EnemyBase _)
    {
        EnemiesOnScreen++;
    }

    void EnemyDiedEvent_Handler(EnemyBase _)
    {
        EnemiesOnScreen--;
        ExistingEnemies--;
    }

    void PlayerLosedHpEvent_Handler()
    {
        //We suppose, that reason of hp losing is puzzle collision
        EnemiesOnScreen--;
        ExistingEnemies--;
    }

    void ResetLevelEvent_Handler()
    {
        EnemiesOnScreen = 0;
        ExistingEnemies = 0;
    }
    
    void LevelClosedEvent_Handler()
    {
        EnemiesOnScreen = 0;
        ExistingEnemies = 0;
    }
}
