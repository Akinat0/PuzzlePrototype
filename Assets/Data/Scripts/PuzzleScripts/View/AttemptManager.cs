using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Puzzle;
using TMPro;

public class AttemptManager : ManagerView
{
    private int _attemptNumber;

    [SerializeField]
    private TextMeshProUGUI _textPlaceholder;

    [SerializeField]
    private string _textOfNotification = "Attempt #";

    void Awake()
    {
        _attemptNumber = 0;
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
        _attemptNumber++;
        _textPlaceholder.text = _textOfNotification + _attemptNumber;
        ShowInstant(_textPlaceholder);
        HideLong(_textPlaceholder);
    }

    protected override void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        _textPlaceholder.color = levelColorScheme.TextColor2;
    }
}
