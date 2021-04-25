using System;
using System.Collections;
using Abu.Tools.UI;
using DG.Tweening;
using Puzzle;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextComponent timerField;
    [SerializeField] AudioClip sound;
 
    IEnumerator currentCountdownRoutine;
    
    public void StartTimer(Action finished)
    {
        if(currentCountdownRoutine != null)
            StopCoroutine(currentCountdownRoutine);

        StartCoroutine(currentCountdownRoutine = CountdownRoutine(finished));
    } 
    
    IEnumerator CountdownRoutine(Action onFinish)
    {
        timerField.gameObject.SetActive(true);
            
        for (int i = 3; i > 0; i--)
        {
            timerField.Text = i.ToString();
            timerField.DOKill();
            timerField.RectTransform.DOPunchScale(new Vector3(1.1f, 1.1f, 1), 0.7f, 3, 0.2f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(1);
            
            if(sound != null && SoundManager.Instance != null)
                SoundManager.Instance.PlayOneShot(sound);
        }
        
        currentCountdownRoutine = null;
        timerField.SetActive(false);
        onFinish?.Invoke();
    }

    void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
    }

    void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme)
    {
        levelColorScheme.SetTextColor(timerField, true);
    }

}
