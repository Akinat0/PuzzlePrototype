using System;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director1;
    [SerializeField] private PlayableDirector _director2;


    private void Start()
    {
        Invoke(nameof(GoTo), 5);
    }

    private void GoTo()
    {
        _director1.Pause();
        _director2.Play();
    }
}
