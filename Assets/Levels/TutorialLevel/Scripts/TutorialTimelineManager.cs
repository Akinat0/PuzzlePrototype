using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] _directors;
    private int _curDir;

    private void Start()
    {
        _curDir = 0;
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

    public void Restart()
    {
        _directors[_curDir].Stop();
        _directors[_curDir].Play();
    }

    private void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += TutorialTakeDamageAction_handler;
    }

    private void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= TutorialTakeDamageAction_handler;
    }

    private void TutorialTakeDamageAction_handler(int hp)
    {
        Debug.LogError("RESTART");
        Restart();
    }
}
