using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Puzzle;

public class AttemptManager : MonoBehaviour
{

    private int _attemptNumber;

    [SerializeField]
    private Text _textPlaceholder;

    [SerializeField]
    private float _timeUntilStartFade = 1.0f;

    [SerializeField]
    private float _fadeTime = 1.0f;

    [SerializeField]
    private string _textOfNotification = "Attempt #";

    void Awake()
    {
        _attemptNumber = 0;
    }

    private void OnEnable()
    {
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;

    }

    void GameStartedEvent_Handler()
    {
        _attemptNumber++;
        _textPlaceholder.text = _textOfNotification + _attemptNumber;
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        _textPlaceholder.gameObject.SetActive(true);
        _textPlaceholder.color = new Color(_textPlaceholder.color.r, _textPlaceholder.color.g, _textPlaceholder.color.b, 1);
        yield return new WaitForSeconds(_timeUntilStartFade);
        while(_textPlaceholder.color.a > 0)
        {
            _textPlaceholder.color = new Color(_textPlaceholder.color.r, _textPlaceholder.color.g, _textPlaceholder.color.b, _textPlaceholder.color.a - (Time.deltaTime / _fadeTime));
            yield return null;
        }
        _textPlaceholder.gameObject.SetActive(false);

    }
}
