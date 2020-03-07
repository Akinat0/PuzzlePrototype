using System;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] _directors;
    private int _curDir;

    private void Start()
    {
        Debug.LogError("START");
        _curDir = 0;
    }

    public void Govno()
    {
        _directors[_curDir].Play();
    }

    public void GoToNext()
    {
        Debug.LogError("GOTO");
        _directors[_curDir].Pause();
        _curDir++;
        if(_curDir < _directors.Length)
        {
            _directors[_curDir].Play();
        }
    }
}
