using System;
using Puzzle;
using UnityEngine;

public abstract class HealthManagerBase : MonoBehaviour
{
    
    protected int Hp { get; set; }
    protected abstract void LoseHeart(int _Hp);
    protected abstract void Reset();
    
    protected virtual void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PlayerRiviveEvent += PlayerRiviveEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PlayerRiviveEvent -= PlayerRiviveEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
    }

    protected virtual void ResetLevelEvent_Handler()
    {
        Reset();
    }

    protected virtual void PlayerRiviveEvent_Handler()
    {
        Reset();
    }
    
    protected virtual void PlayerLosedHpEvent_Handler(int _Hp)
    {
        LoseHeart(_Hp);
    }

    protected virtual void LevelCompletedEvent_Handler()
    {
    }
}
