using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] _directors;
    private int _curDir;

    private bool _isCanTap;
    private TutorialInput _gameInput;
    private bool _firstStage;

    private void Start()
    {
        _isCanTap = false;
        _curDir = 0;
        _gameInput = GetComponent<TutorialInput>();
       _gameInput.enabled = false;
       _firstStage = true;
    }

    public void StartFirstDirector()
    {
        _directors[_curDir].Play();
    }

    public void GoToNext()
    {
        _directors[_curDir].Pause();
        _curDir++;
        if(_curDir < _directors.Length)
        {
            _directors[_curDir].Play();
        }
    }

    public void GiveControl()
    {
        _isCanTap = true;
        _gameInput.enabled = true;
    }

    public void RemoveControl()
    {
        _isCanTap = false;
        _gameInput.enabled = false;
    }

    public void Restart()
    {
        _directors[_curDir].Stop();
        _directors[_curDir].Play();
    }

    private void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += TutorialTakeDamageAction_handler;
        MobileGameInput.TouchOnTheScreen += OnTap_handler;

    }

    private void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= TutorialTakeDamageAction_handler;
        MobileGameInput.TouchOnTheScreen -= OnTap_handler;
    }

    private void OnTap_handler(Touch touch)
    {
        if (_firstStage)
        {
            RemoveControl();
        }
    }

    private void TutorialTakeDamageAction_handler(int hp)
    {
        Debug.LogError("RESTART");
        Restart();
    }

    public void EndFirstStage()
    {
        _firstStage = false;
    }

    
}
