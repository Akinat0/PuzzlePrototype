using System;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class TutorialEnemyShit : ShitEnemy, ITutorialStopReason
{
    private bool _reachedHalfway;

    public event Action Solved;

    protected override void Update()
    {
        base.Update();

        if (!_reachedHalfway)
        {
            if ((GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                ScreenScaler.CameraSize.y * 3.0f / 8.0f)
            {
                _reachedHalfway = true;
                if (GameSceneManager.Instance is TutorialManager sceneManager)
                {
                    sceneManager.InvokeEnemyIsClose(this);
                }
            }
        }
    }

    public override Transform Die()
    {
        if (TutorialManager.TutorialStopped
            && TutorialManager.StopReason as TutorialEnemyShit == this)
        {
            Solved?.Invoke();
        }

        return base.Die();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TutorialManager.OnTutorialRestart += TutorialRestartEvent_Handler;
        TutorialManager.OnStopTutorial += TutorialStopEvent_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        TutorialManager.OnTutorialRestart -= TutorialRestartEvent_Handler;
        TutorialManager.OnStopTutorial -= TutorialStopEvent_Handler;
    }

    void TutorialRestartEvent_Handler()
    {
        Destroy(gameObject);
    }
    
    void TutorialStopEvent_Handler(bool pause)
    {
        Motion = !pause;
    }

    protected override void PauseLevelEvent_Handler(bool paused)
    {
        if (!paused)
        {
            if (!TutorialManager.TutorialStopped)
                base.PauseLevelEvent_Handler(paused);
        }
        else
        {
            base.PauseLevelEvent_Handler(paused);
        }
    }
    
}