using System;
using Puzzle;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialManager : GameSceneManager
{
    public static event Action OnTutorialInputEnabled; 
    public static event Action OnTutorialInputDisabled;
    public static event Action OnTutorialRestart;
    public static event Action<ITutorialStopReason> OnEnemyIsClose;
    public static event Action<bool> OnStopTutorial;
    public static event Action OnTutorialNextStage;
    public static event Action<ITutorialStopReason> OnTutorialStopReasonSolved;

    [SerializeField] private PlayableDirector[] _directors;

    private bool _tutorialStopped = false;
    private ITutorialStopReason _stopReason = null;
    private int _stage = 0;
    
    public int Stage => _stage;
    public static bool TutorialStopped => ((TutorialManager) Instance)._tutorialStopped;
    public static ITutorialStopReason StopReason => ((TutorialManager) Instance)._stopReason;

    private void ProcessTouch()
    {
        switch (_stage)
        {
            case 0:
                //InvokeDisableInput();
                break;
        }
    }

    private void ProcessEnemyIsClose(EnemyBase enemy)
    {
        if (_stage == 0)
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
                
                _stopReason = (ITutorialStopReason)enemy;
                ((ITutorialStopReason)enemy).Solved += StopReasonSolved_Handler;

                InvokeOnStopTutorial(true);
                InvokeEnableInput();
            }
        }
        
    }
    
    private void ProcessTutorialStopReasonSolved()
    {
        if (_stage == 0)
        {
            InvokeOnStopTutorial(false);
            InvokeDisableInput();
        }
    }

    private void Restart()
    {
        _directors[_stage].Stop();
        _directors[_stage].Play();
    }
    
    private void OnEnable()
    {
        base.OnEnable();
        PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        MobileGameInput.TouchOnTheScreen += TouchOnTheScreen_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        MobileGameInput.TouchOnTheScreen -= TouchOnTheScreen_Handler;
    }
    
    private void PlayerLosedHpEvent_Handler(int hp)
    {
        InvokeTutorialRestart();
    }

    private void TouchOnTheScreen_Handler(Touch touch)
    {
        Debug.LogError("Touch registered");
        ProcessTouch();
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
        Debug.LogError("Restart stage " + _stage);
        OnTutorialRestart?.Invoke();
        Restart();
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
        _stage++;
        Debug.LogError("OnTutorialNextStage invoked. Tutorial goes stage " + _stage);
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

