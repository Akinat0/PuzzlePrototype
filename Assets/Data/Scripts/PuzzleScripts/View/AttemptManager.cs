using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Puzzle;

public class AttemptManager : MonoBehaviour
{

    private int _attemptNumper;

    [SerializeField]
    private Text _textPlaceholder;

    [SerializeField]
    private float _timeUntilStartFade = 1.0f;

    [SerializeField]
    private float _fadeTime = 1.0f;

    [SerializeField]
    private string _textOfNotification = "Attempt #";

    void Start()
    {
        _attemptNumper = 0;
    }

    private void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;

    }

    void ResetLevelEvent_Handler()
    {
        _attemptNumper++;
        _textPlaceholder.text = "Attempt #" + _attemptNumper;
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
