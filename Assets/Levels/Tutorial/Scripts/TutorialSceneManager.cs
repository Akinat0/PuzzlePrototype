using System;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class TutorialSceneManager : GameSceneManager
{
    public static event Action<bool> OnStopTutorial;
    
    int Stage { get; set; }
    public bool TutorialStopped { get; private set; }
    public ITutorialStopReason StopReason { get; private set; }

    void ProcessEnemyNotSolved(EnemyBase enemy)
    {
        if (Stage != 0)
            return;
        
        switch (enemy)
        {
            case TutorialEnemyPuzzle puzzleEnemy:
            {
                VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, Player.transform,
                    () => { VFXManager.Instance.CallTutorialTapEffect(Player.transform); }, null, 0.3f);
                
                StopReason = puzzleEnemy;
                StopReason.Solved += StopReasonSolved_Handler;
                
                InvokeOnStopTutorial(true);
                break;
            }
            case TutorialEnemyShit enemyShit:
                VignetteAnimator.FocusAndFollow(VFXManager.Instance.Vignette, enemyShit.transform,
                    () => { VFXManager.Instance.CallTutorialTapEffect(enemy.transform); }, null, 0.3f);

                StopReason = enemyShit;
                enemyShit.Solved += StopReasonSolved_Handler;

                InvokeOnStopTutorial(true);
                break;
        }
    }
    void ProcessTutorialStopReasonSolved(ITutorialStopReason tutorialStopReason)
    {
        if (Stage != 0) return;
        
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
        InvokeOnStopTutorial(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ResetLevelEvent += ResetLevelEvent_Handler;
        LevelClosedEvent += OnLevelClosedEvent_Handler;
        LevelCompletedEvent += LevelCompletedEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ResetLevelEvent -= ResetLevelEvent_Handler;
        LevelClosedEvent -= OnLevelClosedEvent_Handler;
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs _)
    {
        //In tutorial we obtain all three stars on complete
        LevelConfig.ObtainThirdStar();
    }
    
    void ResetLevelEvent_Handler()
    {
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
        TotalHearts = int.MaxValue;
        CurrentHearts = int.MaxValue;
        Stage = 0;
    }

    void StopReasonSolved_Handler()
    {
        ProcessTutorialStopReasonSolved(StopReason);
        StopReason.Solved -= StopReasonSolved_Handler; //We're unsubscribing on invoke
    }
    
    void OnLevelClosedEvent_Handler()
    {
        LevelConfig.ObtainThirdStar();
        VignetteAnimator.FadeOut(VFXManager.Instance.Vignette);
        Account.TutorialCompleted.Value = true;
    }
    
    public void InvokeEnemyNotSolved(EnemyBase enemy)
    {
        Debug.Log($"<color=green> Enemy is close invoked {enemy.name}</color>");
        ProcessEnemyNotSolved(enemy);
    }
    public void InvokeTutorialNextStage()
    {
        Stage++;
        Debug.Log($"<color=green> OnTutorialNextStage invoked. Tutorial goes stage {Stage}</color>");
    }

    public void InvokeOnStopTutorial(bool pause)
    {
        TutorialStopped = pause;
        Debug.Log("<color=green> OnStopTutorial invoked </color> ");
        OnStopTutorial?.Invoke(TutorialStopped);
    }
}

