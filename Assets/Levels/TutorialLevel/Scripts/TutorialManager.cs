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
    public static event Action<EnemyBase> OnEnemyIsClose;
    public static event Action<bool> OnStopTutorial;

    [SerializeField] private PlayableDirector[] _directors;
    
    private int _stage = 0;
    public int Stage => _stage;

    public void StartFirstDirector()
    {
        _directors[_stage].Play();
    }
    
    public void GoToNext()
    {
        _directors[_stage].Pause();
        _stage++;
        if(_stage < _directors.Length)
        {
            _directors[_stage].Play();
        }
    }

    private void ProcessTouch()
    {
        switch (_stage)
        {
            case 0:
                InvokeDisableInput();
                break;
        }
    }

    private void ProcessEnemyIsClose(EnemyBase enemy)
    {
        if (_stage == 0)
        {
            if (enemy.Type == EnemyType.Puzzle && enemy is PuzzleEnemy puzzleEnemy)
            {
                if(Player.sides[puzzleEnemy.Type.GetHashCode()] != puzzleEnemy.stickOut)
                {
                    OnStopTutorial?.Invoke(enemy);
                }      
            }
            else
            {
                OnStopTutorial?.Invoke(enemy);
            }
            
            OnStopTutorial?.Invoke(true);
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
        OnEnemyIsClose?.Invoke(enemy);
        ProcessEnemyIsClose(enemy);
    }
}

