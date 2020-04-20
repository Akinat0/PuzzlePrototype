using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineManager : MonoBehaviour
{
    public static event Action OnTutorialInputEnabled; 
    public static event Action OnTutorialInputDisabled;
    public static event Action OnTutorialRestart;

    [SerializeField] private PlayableDirector[] _directors;
    
    private int _stage = 0;

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
    
    private void Restart()
    {
        _directors[_stage].Stop();
        _directors[_stage].Play();
    }
    
    private void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        MobileGameInput.TouchOnTheScreen += TouchOnTheScreen_Handler;
    }
    
    private void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
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
}

