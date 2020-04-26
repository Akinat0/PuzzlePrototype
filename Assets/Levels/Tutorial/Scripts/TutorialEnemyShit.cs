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
                if (GameSceneManager.Instance is TutoriaScenelManager sceneManager)
                {
                    sceneManager.InvokeEnemyIsClose(this);
                }
            }
        }
    }

    public override Transform Die()
    {
        if (TutoriaScenelManager.TutorialStopped
            && TutoriaScenelManager.StopReason as TutorialEnemyShit == this)
        {
            Solved?.Invoke();
        }

        return base.Die();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TutoriaScenelManager.OnTutorialRestart += TutorialRestartEvent_Handler;
        TutoriaScenelManager.OnStopTutorial += TutorialStopEvent_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        TutoriaScenelManager.OnTutorialRestart -= TutorialRestartEvent_Handler;
        TutoriaScenelManager.OnStopTutorial -= TutorialStopEvent_Handler;
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
            if (!TutoriaScenelManager.TutorialStopped)
                base.PauseLevelEvent_Handler(paused);
        }
        else
        {
            base.PauseLevelEvent_Handler(paused);
        }
    }
    
}