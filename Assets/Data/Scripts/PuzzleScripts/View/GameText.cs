using System;
using System.Collections;
using Abu.Tools.UI;
using DG.Tweening;
using UnityEngine;

namespace Puzzle
{
    public abstract class GameText : MonoBehaviour
    {
        protected const float TIME_TO_DISAPPEAR = 1.0f;
        protected const float APPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME = 1.0f;
        protected const float DISAPPEAR_TIME_LONG = 3.0f;

        protected Action<float> AlphaSetter;
        protected Func<float> AlphaGetter;

        //TODO
        static TextGroupComponent textGroup;

        protected TextGroupComponent TextGroup
        {
            get
            {
                if (textGroup == null)
                    textGroup = TextGroupComponent.AttachTo(Canvas.gameObject, 0);
            
                return textGroup;
            }
        }

        Canvas canvas;
        Canvas Canvas
        {
            get
            {
                if (canvas == null)
                    canvas = transform.GetComponentInParent<Canvas>();
                
                if (canvas == null)
                    canvas = transform.GetComponent<Canvas>();
                
                return canvas;
            }
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