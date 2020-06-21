using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public abstract class ManagerView : MonoBehaviour
    {
        protected const float TIME_TO_DISAPPEAR = 1.0f;
        protected const float APPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME_LONG = 3.0f;

        protected static event Action<float> ChangeSharedFontSize;

        private void Awake()
        {
            GameSceneManager.CutsceneStartedEvent += CutsceneStartedEvent_Handler;
            GameSceneManager.CutsceneEndedEvent += CutsceneEndedEvent_Handler;
        }

        private void OnDestroy()
        {
            GameSceneManager.CutsceneStartedEvent -= CutsceneStartedEvent_Handler;
            GameSceneManager.CutsceneEndedEvent -= CutsceneEndedEvent_Handler;
        }

        protected virtual void OnEnable()
        {
            GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        }

        protected virtual void OnDisable()
        {
            GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        }
        
        protected abstract void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme);

        protected IEnumerator CountdownRoutine(Text timerField, Action onFinish)
        {
            if(!timerField.gameObject.activeSelf)
                timerField.gameObject.SetActive(true);
            
            for (int i = 3; i > 0; i--)
            {
                timerField.text = i.ToString();
                timerField.DOKill();
                timerField.rectTransform.DOPunchScale(new Vector3(1.1f, 1.1f, 1), 0.7f, 3, 0.2f).SetUpdate(true);
                yield return new WaitForSecondsRealtime(1);
            }
            
            onFinish?.Invoke();
        }
        
        protected virtual void CutsceneStartedEvent_Handler(CutsceneEventArgs _args)
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void CutsceneEndedEvent_Handler(CutsceneEventArgs _args)
        {
            gameObject.SetActive(true);
        }

        protected void ShowShort(TextMeshProUGUI text)
        {
            text.DOKill();
            text.DOFade(1.0f, APPEAR_TIME).onComplete += () => text.DOFade(0.0f, DISAPPEAR_TIME);
        }
        
        protected void HideLong(TextMeshProUGUI text)
        {
            text.DOKill();
            text.Invoke(() => text.DOFade(0.0f, DISAPPEAR_TIME_LONG), TIME_TO_DISAPPEAR);
        }

        protected void ShowInstant(TextMeshProUGUI text)
        {
            text.DOKill();
            Color color = text.color;
            color.a = 1;
            text.color = color;
        }
        
        protected void HideInstant(TextMeshProUGUI text)
        {
            text.DOKill();
            Color color = text.color;
            color.a = 0;
            text.color = color;
        }

        protected void InvokeChangeSharedFontSize(float fontSize)
        {
            ChangeSharedFontSize?.Invoke(fontSize);
        }
    }
}