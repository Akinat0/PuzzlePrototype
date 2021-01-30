using System;
using Abu.Tools;
using Puzzle;

public class TutorialEnemyPuzzle : NeonPuzzle, ITutorialStopReason
{
    public event Action Solved;
    
    public bool IsSolved => GameSceneManager.Instance.Player.sides[(int)side] != stickOut;
    bool IsHalfway
    {
        get
        {
            if (_enemyParams.side == Side.Down || _enemyParams.side == Side.Up)
                return (GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                       ScreenScaler.CameraSize.y / 4;
            else
                return (GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                       ScreenScaler.CameraSize.x / 4;
        }
    }
    bool isNeedToBeSolved;
    TutorialSceneManager SceneManager => GameSceneManager.Instance as TutorialSceneManager;
    
    protected override void Update()
    {
        base.Update();

        if (SceneManager.TutorialStopped
            && SceneManager.StopReason as TutorialEnemyPuzzle == this
            && IsSolved)
        {
            isNeedToBeSolved = false;
            Solved?.Invoke();
        }

        if (!isNeedToBeSolved && IsHalfway && !IsSolved)
        {
            isNeedToBeSolved = true;
            SceneManager.InvokeEnemyNotSolved(this);
        }
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        TutorialSceneManager.OnStopTutorial += TutorialStopEvent_Handler;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        TutorialSceneManager.OnStopTutorial -= TutorialStopEvent_Handler;
    }

    void PlayerLosedHpEvent_Handler()
    {
        Die();
    }
    void TutorialStopEvent_Handler(bool pause)
    {
        Motion = !pause;
    }

    protected override void PauseLevelEvent_Handler(bool paused)
    {
        if (!paused)
        {
            if (!SceneManager.TutorialStopped)
                base.PauseLevelEvent_Handler(false);
        }
        else
        {
            base.PauseLevelEvent_Handler(true);
        }
    }

}