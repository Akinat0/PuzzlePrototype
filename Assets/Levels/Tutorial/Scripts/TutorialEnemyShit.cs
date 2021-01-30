using System;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class TutorialEnemyShit : ShitEnemy, ITutorialStopReason
{

    TutorialSceneManager SceneManager => GameSceneManager.Instance as TutorialSceneManager;
    
    public event Action Solved;

    bool IsHalfway
    {
        get
        {
            if (_enemyParams.side == Side.Down || _enemyParams.side == Side.Up)
                return (GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                       ScreenScaler.CameraSize.y * 3.0f / 8.0f;
            else
                return (GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                       ScreenScaler.CameraSize.x * 3.0f / 8.0f;
        }
    }
    bool isNeedToBeSolved;
   
    protected override void Update()
    {
        base.Update();

        if (!IsHalfway || isNeedToBeSolved) return;

        isNeedToBeSolved = true;
        SceneManager.InvokeEnemyNotSolved(this);
    }

    public override Transform Die()
    {
        if (SceneManager.TutorialStopped
            && SceneManager.StopReason as TutorialEnemyShit == this)
        {
            Solved?.Invoke();
        }

        return base.Die();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        TutorialSceneManager.OnStopTutorial += TutorialStopEvent_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        TutorialSceneManager.OnStopTutorial -= TutorialStopEvent_Handler;
    }

    void PlayerLosedHpEvent_Handler()
    {
        Die();
    }
    
    void TutorialStopEvent_Handler(bool pause)
    {
        Motion = !pause;
    }

    protected override void PauseLevelEvent_Handler(bool paused)
    {
        if (!paused)
        {
            if (!SceneManager.TutorialStopped)
                base.PauseLevelEvent_Handler(false);
        }
        else
        {
            base.PauseLevelEvent_Handler(true);
        }
    }

}