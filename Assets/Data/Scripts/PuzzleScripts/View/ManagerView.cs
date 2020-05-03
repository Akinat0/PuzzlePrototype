using System;
using DG.Tweening;
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

        protected virtual void CutsceneStartedEvent_Handler(string SceneID)
        {
            gameObject.SetActive(false);
        }
        
        protected virtual void CutsceneEndedEvent_Handler(string SceneID)
        {
            gameObject.SetActive(true);
        }

        protected void ShowShort(Text text)
        {
            text.DOKill();
            var fadeOutTween = text.DOFade(1.0f, APPEAR_TIME);
            fadeOutTween.onComplete += () => text.DOFade(0.0f, DISAPPEAR_TIME);
        }
        
        protected void HideLong(Text text)
        {
            text.DOKill();
            text.Invoke(() => text.DOFade(0.0f, DISAPPEAR_TIME_LONG), TIME_TO_DISAPPEAR);
        }

        protected void ShowInstant(Text text)
        {
            text.DOKill();
            Color color = text.color;
            color.a = 1;
            text.color = color;
        }
        
        protected void HideInstant(Text text)
        {
            text.DOKill();
            Color color = text.color;
            color.a = 0;
            text.color = color;
        }
    }
}