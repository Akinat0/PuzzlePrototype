using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Puzzle;
using TMPro;

public class AttemptManager : ManagerView
{
    private int attemptNumber;

    [SerializeField]
    private TextMeshProUGUI TextPlaceholder;
    
    const string TextOfNotification = "Attempt #";

    void Awake()
    {
        attemptNumber = 0;
        AlphaSetter = alpha => TextPlaceholder.alpha = alpha;
        AlphaGetter = () => TextPlaceholder.alpha;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
    }

    void GameStartedEvent_Handler()
    {
        attemptNumber++;
        TextPlaceholder.text = TextOfNotification + attemptNumber;
        ShowInstant();
        HideLong();
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        TextPlaceholder.color = levelColorScheme.TextColor2;
    }
}
