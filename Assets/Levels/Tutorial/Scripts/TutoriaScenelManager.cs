using System;
using Puzzle;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Playables;

public class TutoriaScenelManager : GameSceneManager
{
    public static event Action OnTutorialInputEnabled; 
    public static event Action OnTutorialInputDisabled;
    public static event Action OnTutorialRestart;
    public static event Action<ITutorialStopReason> OnEnemyIsClose;
    public static event Action<bool> OnStopTutorial;
    public static event Action OnTutorialNextStage;
    public static event Action<ITutorialStopReason> OnTutorialStopReasonSolved;
    
    private bool _tutorialStopped = false;
    private ITutorialStopReason _stopReason = null;
    private int _stage = 0;

    public int Stage
    {
        get => _stage;
      
        private set
        {
            _stage = value;
            if (_stage > 0)
                InvokeEnableInput();
            else
                InvokeDisableInput();
        }
    }
    
    public static bool TutorialStopped => ((TutoriaScenelManager) Instance)._tutorialStopped;
    public static ITutorialStopReason StopReason => ((TutoriaScenelManager) Instance)._stopReason;

    private void ProcessEnemyIsClose(EnemyBase enemy)
    {
        switch (Stage)
        {
            case 0:
            {
                if (enemy.Type == EnemyType.Puzzle && enemy is TutorialEnemyPuzzle puzzleEnemy)
                {
                    if (Player.sides[puzzleEnemy.side.GetHashCode()] == puzzleEnemy.stickOut) //Sides shouldn't be equal
                    {
                        VFXManager.Instance.CallTapEffect(Player.transform);

                        if (puzzleEnemy is ITutorialStopReason stopReason)
                        {
                            _stopReason = stopReason;
                            stopReason.Solved += StopReasonSolved_Handler;
                        }

                        InvokeOnStopTutorial(true);
                        InvokeEnableInput();
                    }
                }

                if (enemy.Type == EnemyType.Shit && enemy is TutorialEnemyShit)
                {
                    VFXManager.Instance.CallTapEffect(enemy.transform);

                    _stopReason = (ITutorialStopReason) enemy;
                    ((ITutorialStopReason) enemy).Solved += StopReasonSolved_Handler;

                    InvokeOnStopTutorial(true);
                    InvokeEnableInput();
                }
                break;
            }

            case 1:
            {
                if (enemy.Type == EnemyType.Puzzle && enemy is TutorialEnemyPuzzle puzzleEnemy)
                    if (Player.sides[puzzleEnemy.side.GetHashCode()] == puzzleEnemy.stickOut) //Sides shouldn't be equal
                        VFXManager.Instance.CallTapEffect(Player.transform);
                
                if (enemy.Type == EnemyType.Shit && enemy is TutorialEnemyShit)
                    VFXManager.Instance.CallTapEffect(enemy.transform);
                break;
            }
        }
    }
            
        
    
    
    private void ProcessTutorialStopReasonSolved()
    {
        switch (Stage)
        {
            case 0:
                InvokeOnStopTutorial(false);
                InvokeDisableInput();
                break;
        }
    }

    private void OnEnable()
    {
        base.OnEnable();
        PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        ResetLevelEvent += ResetLevelEvent_Handler;
        MobileGameInput.TouchOnTheScreen += TouchOnTheScreen_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        ResetLevelEvent -= ResetLevelEvent_Handler;
        MobileGameInput.TouchOnTheScreen -= TouchOnTheScreen_Handler;
    }
    
    private void PlayerLosedHpEvent_Handler(int hp)
    {
        InvokeTutorialRestart();
    }

    private void ResetLevelEvent_Handler()
    {
        Stage = 0;
    }
    
    private void TouchOnTheScreen_Handler(Touch touch)
    {
        Debug.LogError("Touch registered");
    }

    private void StopReasonSolved_Handler()
    {
        InvokeOnTutorialStopReasonSolved(_stopReason);
        _stopReason.Solved -= StopReasonSolved_Handler; //We're unsubscribing on invoke
    }
    
    public void InvokeEnableInput()
    {
        Debug.LogError("Tutorial input enabled");
        OnTutorialInputEnabled?.Invoke();
    }
    
    public void InvokeDisableInput()
    {
        Debug.LogError("Tutorial input disabled");
        OnTutorialInputDisabled?.Invoke();
    }

    public void InvokeTutorialRestart()
    {
        Debug.LogError("Restart stage " + Stage);
        OnTutorialRestart?.Invoke();
    }
    
    public void InvokeEnemyIsClose(EnemyBase enemy)
    {
        Debug.LogError("Enemy is close invoked " + enemy.name);
        if(enemy is ITutorialStopReason tutorialStopReason)
            OnEnemyIsClose?.Invoke(tutorialStopReason);
        ProcessEnemyIsClose(enemy);
    }
    public void InvokeTutorialNextStage()
    {
        Stage++;
        Debug.LogError("OnTutorialNextStage invoked. Tutorial goes stage " + Stage);
        OnTutorialNextStage?.Invoke();
    }

    public void InvokeOnStopTutorial(bool pause)
    {
        _tutorialStopped = pause;
        Debug.LogError("OnStopTutorial invoked");
        OnStopTutorial?.Invoke(_tutorialStopped);
    }
    
    public void InvokeOnTutorialStopReasonSolved(ITutorialStopReason reason)
    {
        Debug.LogError("OnTutorialStopReasonSolved invoked");
        OnTutorialStopReasonSolved?.Invoke(reason);
        ProcessTutorialStopReasonSolved();
    }
}

