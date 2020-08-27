using System;
using Puzzle;
using UnityEngine;

public abstract class HealthManagerBase : MonoBehaviour
{
    
    protected int Hp { get; set; }
    protected abstract void LoseHeart(int _Hp);
    protected abstract void ResetHealth();
    
    protected virtual void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PlayerReviveEvent += PlayerReviveEventHandler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PlayerReviveEvent -= PlayerReviveEventHandler;
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }

    protected virtual void ResetLevelEvent_Handler()
    {
        ResetHealth();
    }

    protected virtual void PlayerReviveEventHandler()
    {
        ResetHealth();
    }
    
    protected virtual void PlayerLosedHpEvent_Handler(int _Hp)
    {
        LoseHeart(_Hp);
    }

    protected virtual void LevelCompletedEvent_Handler(LevelCompletedEventArgs _)
    {
    }

    protected virtual void GameStartedEvent_Handler()
    {
    }

    protected virtual void LevelClosedEvent_Handler()
    {
        
    }
    
}
