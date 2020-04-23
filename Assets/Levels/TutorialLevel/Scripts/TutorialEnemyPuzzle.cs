﻿using System;
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

        if (TutorialManager.TutorialStopped
            && TutorialManager.StopReason as TutorialEnemyPuzzle == this
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
                if (GameSceneManager.Instance is TutorialManager sceneManager)
                    sceneManager.InvokeEnemyIsClose(this);
            }
        }
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