using System;
using System.Collections;
using Abu.Tools;
using Abu.Tools.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Puzzle
{
    public abstract class ManagerView : MonoBehaviour
    {
        protected const float TIME_TO_DISAPPEAR = 1.0f;
        protected const float APPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME_LONG = 3.0f;

        protected Action<float> AlphaSetter;
        protected Func<float> AlphaGetter;

        
        static TextGroupComponent textGroup;

        protected TextGroupComponent TextGroup
        {
            get
            {
                if (textGroup == null)
                    textGroup = TextGroupComponent.AttachTo(transform.parent.gameObject, 0);
            
                return textGroup;
            }
        }
        
        private void Awake()
        {
            GameSceneManager.CutsceneStartedEvent += CutsceneStartedEvent_Handler;
            GameSceneManager.CutsceneEndedEvent += CutsceneEndedEvent_Handler;
        }

        private void OnDestroy()
        {
            GameSceneManager.CutsceneStartedEvent -= CutsceneStartedEvent_Handler;
            GameSceneManager.CutsceneEndedEvent -= CutsceneEndedEvent_Handler;
            textGroup = null;
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

        #region timer
        
        protected IEnumerator CountdownRoutine(TextMeshProUGUI timerField, Action onFinish)
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
        
        #endregion
        
        protected virtual void CutsceneStartedEvent_Handler(CutsceneEventArgs _args)
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void CutsceneEndedEvent_Handler(CutsceneEventArgs _args)
        {
            gameObject.SetActive(true);
        }

        IEnumerator AlphaRoutine(float endValue, float duration, Action finished = null)
        {
            float startValue = AlphaGetter();
            endValue = Mathf.Clamp01(endValue);

            float time = 0;

            while (time < duration)
            {
                yield return null;

                time += Time.deltaTime;
                AlphaSetter(Mathf.Lerp(startValue, endValue, time / duration));
            }

            AlphaSetter(endValue);
            yield return null;
            
            finished?.Invoke();
        }

        protected void ShowShort()
        {
            StopAllCoroutines();
            AlphaSetter(1);
            this.Invoke(() => StartCoroutine(AlphaRoutine(0, DISAPPEAR_TIME)), APPEAR_TIME);
        }
        
        protected void HideLong()
        {
            StopAllCoroutines();
            this.Invoke(
                () => { StartCoroutine(AlphaRoutine(0, DISAPPEAR_TIME_LONG)); },
                TIME_TO_DISAPPEAR);
        }

        protected void ShowInstant()
        {
            StopAllCoroutines();
            AlphaSetter(1);
        }
        
        protected void HideInstant()
        {
            StopAllCoroutines();
            AlphaSetter(1);
        }
    }
}