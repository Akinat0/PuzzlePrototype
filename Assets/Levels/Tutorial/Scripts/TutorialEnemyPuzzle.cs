using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class TutorialEnemyPuzzle : NeonPuzzle, ITutorialStopReason
{
    private bool _reachedHalfway;
    public event Action Solved;
    protected override void Update()
    {
        base.Update();

        if (TutoriaScenelManager.TutorialStopped
            && TutoriaScenelManager.StopReason as TutorialEnemyPuzzle == this
            && GameSceneManager.Instance.Player.sides[side.GetHashCode()] != stickOut)//Sides shouldn't be equal
        {
            Solved?.Invoke();
        }

        if (!_reachedHalfway)
        {
            if ((GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                ScreenScaler.CameraSize.y / 4)
            {
                _reachedHalfway = true;
                if (GameSceneManager.Instance is TutoriaScenelManager sceneManager)
                    sceneManager.InvokeEnemyIsClose(this);
            }
        }
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