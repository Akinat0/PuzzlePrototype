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

    private FadeEffect _fadeEffect;

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

    protected override void Start()
    {
        base.Start();
        _fadeEffect = VFXManager.Instance.CallFadeEffect(GameSceneRoot, RenderLayer.Default, 110);
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
                        _fadeEffect.setActive(false);
                        VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, Player.transform,
                            () => { VFXManager.Instance.CallTutorialTapEffect(Player.transform); }, null, 0.8f);

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
                    _fadeEffect.setActive(false);
                    VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, enemy.transform,
                        () => { VFXManager.Instance.CallTutorialTapEffect(enemy.transform); }, null, 0.8f);
                    
                    _stopReason = (ITutorialStopReason) enemy;
                    ((ITutorialStopReason) enemy).Solved += StopReasonSolved_Handler;

                    InvokeOnStopTutorial(true);
                    InvokeEnableInput();
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
    private void ProcessTutorialStopReasonSolved()
    {
        switch (Stage)
        {
            case 0:
                _fadeEffect.setActive(true);
                VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
                InvokeOnStopTutorial(false);
                InvokeDisableInput();
                break;
        }
    }
    
    private void ProcessTutorialNextStage()
    {
        switch (Stage)
        {
            case 1:
                _fadeEffect.Sprite.DOFade(0, 1f); //Fadeout
                break;
        }
    }

    
    
    private void OnEnable()
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
        InvokeTutorialRestart();
    }

    private void ResetLevelEvent_Handler()
    {
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
        Stage = 0;
        _fadeEffect.setActive(true);
        _fadeEffect.Sprite.DOFade(FadeEffect.DefaultAlpha, 0);
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
        ProcessTutorialStopReasonSolved();
    }
    
}

