using System;
using Abu.Tools;
using DG.Tweening;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

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
    

    private bool _firstPuzzleTip = true;
    private bool _firstShitTip = true;
    
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
                        VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, Player.transform,
                            () => { VFXManager.Instance.CallTutorialTapEffect(Player.transform); }, null, 0.8f);

                        if (puzzleEnemy is ITutorialStopReason stopReason)
                        {
                            _stopReason = stopReason;
                            stopReason.Solved += StopReasonSolved_Handler;
                        }

                        InvokeOnStopTutorial(true);
                        InvokeEnableInput();

                        if (_firstPuzzleTip)
                            ShowDialog("Tap anywhere now! Otherwise I will take a damage =(");
                        
                        
                    }
                }

                if (enemy.Type == EnemyType.Shit && enemy is TutorialEnemyShit)
                {
                    VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, enemy.transform,
                        () => { VFXManager.Instance.CallTutorialTapEffect(enemy.transform); }, null, 0.8f);
                    
                    _stopReason = (ITutorialStopReason) enemy;
                    ((ITutorialStopReason) enemy).Solved += StopReasonSolved_Handler;

                    InvokeOnStopTutorial(true);
                    InvokeEnableInput();

                    if (_firstShitTip)
                        ShowDialog("Tap on this shit!");
                    
                }
                break;
            }

//            case 1:
//            {
//                if (enemy.Type == EnemyType.Puzzle && enemy is TutorialEnemyPuzzle puzzleEnemy)
//                    if (Player.sides[puzzleEnemy.side.GetHashCode()] == puzzleEnemy.stickOut) //Sides shouldn't be equal
//                        VFXManager.Instance.CallTutorialTapEffect(Player.transform);
//                
//                if (enemy.Type == EnemyType.Shit && enemy is TutorialEnemyShit)
//                    VFXManager.Instance.CallTutorialTapEffect(enemy.transform);
//                break;
//            }
        }
    }
    private void ProcessTutorialStopReasonSolved(ITutorialStopReason tutorialStopReason)
    {
        switch (Stage)
        {
            case 0:
                VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
                InvokeOnStopTutorial(false);
                InvokeDisableInput();

                if (tutorialStopReason is TutorialEnemyPuzzle && _firstPuzzleTip)
                {
                    _firstPuzzleTip = false;
                    ShowDialog("Great! Thank you!", 3);
                }
                
                if (tutorialStopReason is TutorialEnemyShit && _firstShitTip)
                {
                    _firstShitTip = false;
                    ShowDialog("Good job!", 3);
                }

                break;
        }
    }
    
    private void ProcessTutorialNextStage()
    {
        switch (Stage)
        {
            case 1:
                ShowDialog("Great!", 2);
                this.Invoke(() => ShowDialog("Try it yourself now =)", 5), 2);
                break;
        }
    }

    
    
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        ResetLevelEvent += ResetLevelEvent_Handler;
        LevelClosedEvent += OnLevelClosedEvent_Handler;
        MobileGameInput.TouchOnTheScreen += TouchOnTheScreen_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        ResetLevelEvent -= ResetLevelEvent_Handler;
        LevelClosedEvent -= OnLevelClosedEvent_Handler;
        MobileGameInput.TouchOnTheScreen -= TouchOnTheScreen_Handler;
    }
    
    private void PlayerLosedHpEvent_Handler(int hp)
    {
        ShowDialog("Hey! It was painful!", 2.5f);
        this.Invoke(() => ShowDialog("Let's try again =)", 2), 2.5f);
        InvokeTutorialRestart();
    }

    private void ResetLevelEvent_Handler()
    {
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
        Stage = 0;
    }
    
    private void TouchOnTheScreen_Handler(Touch touch)
    {
        Debug.Log("<color=green> Touch registered </color> ");
    }

    private void StopReasonSolved_Handler()
    {
        InvokeOnTutorialStopReasonSolved(_stopReason);
        _stopReason.Solved -= StopReasonSolved_Handler; //We're unsubscribing on invoke
    }
    
    private void OnLevelClosedEvent_Handler()
    {
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
    }
    
    public void InvokeEnableInput()
    {
        Debug.Log("<color=green> Tutorial input enabled </color> ");
        OnTutorialInputEnabled?.Invoke();
    }
    
    public void InvokeDisableInput()
    {
        Debug.Log("<color=green> Tutorial input disabled </color> ");
        OnTutorialInputDisabled?.Invoke();
    }

    public void InvokeTutorialRestart()
    {
        Debug.Log("<color=green> Restart stage " + Stage + "</color>");
        OnTutorialRestart?.Invoke();
    }
    
    public void InvokeEnemyIsClose(EnemyBase enemy)
    {
        Debug.Log("<color=green> Enemy is close invoked " + enemy.name + "</color>");
        if(enemy is ITutorialStopReason tutorialStopReason)
            OnEnemyIsClose?.Invoke(tutorialStopReason);
        ProcessEnemyIsClose(enemy);
    }
    public void InvokeTutorialNextStage()
    {
        Stage++;
        Debug.Log("<color=green> OnTutorialNextStage invoked. Tutorial goes stage " + Stage + "</color>");
        OnTutorialNextStage?.Invoke();
        ProcessTutorialNextStage();
    }

    public void InvokeOnStopTutorial(bool pause)
    {
        _tutorialStopped = pause;
        Debug.Log("<color=green> OnStopTutorial invoked </color> ");
        OnStopTutorial?.Invoke(_tutorialStopped);
    }
    
    public void InvokeOnTutorialStopReasonSolved(ITutorialStopReason reason)
    {
        Debug.Log("<color=green> OnTutorialStopReasonSolved invoked </color>");
        OnTutorialStopReasonSolved?.Invoke(reason);
        ProcessTutorialStopReasonSolved(reason);
    }
    
}

