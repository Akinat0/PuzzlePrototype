using System;
using System.Collections;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class OverlayView : ButtonComponent
    {
        public static T Create<T>(Transform parent, int siblingIndex) where T : OverlayView
        {
            T prefab = Resources.Load<T>("UI/" + typeof(T).Name);

            T entity = Instantiate(prefab, parent);
            
            entity.RectTransform.SetSiblingIndex(siblingIndex);
            
            entity.RectTransform.anchorMin = Vector2.zero;
            entity.RectTransform.anchorMax = Vector2.one;
            
            entity.RectTransform.offsetMin = Vector2.zero;
            entity.RectTransform.offsetMax = Vector2.zero;

            return entity;
        }

        [SerializeField, Range(0, 1)] float phase;

        protected virtual bool RaycastTarget
        {
            get => Background.raycastTarget;
            set => Background.raycastTarget = value;
        }
        
        public float Phase
        {
            get => phase;
            set
            {
                phase = Mathf.Clamp01(value);
                ProcessPhase();
            }
        }

        public virtual void ChangePhase(float targetValue, float duration, Action finished = null)
        {
            StopAllCoroutines();
            StartCoroutine(ChangePhaseRoutine(Mathf.Clamp01(targetValue), duration, finished));
        }

        IEnumerator ChangePhaseRoutine(float targetPhase, float duration, Action finished)
        {
            float startPhase = Phase;
            float time = 0;

            while (time < duration)
            {
                yield return null;

                time += Time.unscaledDeltaTime;
                Phase = Mathf.Lerp(startPhase, targetPhase, time / duration);
            }

            Phase = targetPhase;
            yield return null;
            
            finished?.Invoke();
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected abstract void ProcessPhase();
    }
}