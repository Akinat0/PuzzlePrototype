using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class TutorialEnemyPuzzle : NeonPuzzle
{
    private bool _reachedHalfway;
    private EnemyBase _reasonOfStop = null;
    
    protected override void Update()
    {
        base.Update();

        if (TutorialManager.TutorialStopped
            && _reasonOfStop == this
            && GameSceneManager.Instance.Player.sides[side.GetHashCode()] != stickOut
            //Sides shouldn't be equal
            && GameSceneManager.Instance is TutorialManager manager)
        {
            manager.InvokeOnStopTutorial(false);
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
        TutorialManager.OnEnemyIsClose += EnmeyIsClose_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        TutorialManager.OnTutorialRestart -= TutorialRestartEvent_Handler;
        TutorialManager.OnStopTutorial -= TutorialStopEvent_Handler;
        TutorialManager.OnEnemyIsClose -= EnmeyIsClose_Handler;
    }

    void TutorialRestartEvent_Handler()
    {
        Destroy(gameObject);
    }
    void TutorialStopEvent_Handler(bool pause)
    {
        if (!pause)
            _reasonOfStop = null;
        
        Motion = !pause;
    }
    
    void EnmeyIsClose_Handler(EnemyBase enemy)
    {
        _reasonOfStop = enemy;
    }
}