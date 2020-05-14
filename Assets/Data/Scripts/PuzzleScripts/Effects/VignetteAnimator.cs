using System;
using System.Collections;
using Abu.Tools;
using Puzzle;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class VignetteAnimator
{
    private const float FocusTime = 0.8f;
    private const float FadeOutTime = 0.2f;
    private const float Sampling = 60f;

    private static MonoHelper coroutineHolder;

    private static GameObject holderObject;
    private static GameObject HolderObject
    {
        get
        {
            if (holderObject == null)
                holderObject = new GameObject("Vignette Animator Coroutine Holder");
            
            return holderObject;
        }
    }

    static void Clear()
    {
        if(coroutineHolder != null && coroutineHolder.gameObject != null)
            Object.DestroyImmediate(coroutineHolder);

        coroutineHolder = HolderObject.AddComponent<MonoHelper>();
    }
    
    public static void FocusAndFollow(Vignette vignette, Transform target, Action onSuccess = null, Action onFail = null, float? focusTime = null)
    {
        Clear();
        
        Action successAction = () => { Object.DestroyImmediate(coroutineHolder); };
        successAction += onSuccess;
        
        Action failAction = () => { Object.DestroyImmediate(coroutineHolder); };
        failAction += onFail;
        
        vignette.active = true;
        vignette.rounded.value = true;
        vignette.smoothness.value = 1;
        vignette.color.value = new Color(0.2f, 0.2f, 0.2f);
        
        coroutineHolder.StartCoroutine(FollowRoutine(target, successAction, failAction, focusTime ?? FocusTime, vignette));
    }

    
    static IEnumerator FollowRoutine(Transform target, Action success, Action fail, float focusTime, Vignette vignette)
    {
        bool isFail = false;
        float animationStep = focusTime / Sampling;
        float animationTime = 0;

        while (animationTime < focusTime)
        {
            if (target == null)
            {
                isFail = true;
                break;
            }

            vignette.center.value = ScreenScaler.WorldToScreenNormalized(target.position);
            vignette.intensity.value = animationTime / focusTime;
            animationTime += animationStep;
            yield return new WaitForSecondsRealtime(animationStep);
        }

        if(isFail)
            fail?.Invoke();    
        else
            success?.Invoke();
    }

    public static void FadeOut(Vignette vignette, Action onSuccess = null, float? fadeTime = null)
    {
        Clear();
        
        Action successAction = () => { Object.DestroyImmediate(coroutineHolder); };
        successAction += onSuccess;

        coroutineHolder.StartCoroutine(FadeOutRoutine(successAction, fadeTime ?? FadeOutTime, vignette));
    }

    
    static IEnumerator FadeOutRoutine(Action success, float fadeTime, Vignette vignette)
    {
        float animationStep = fadeTime / Sampling;
        float animationTime = 0;
        Color startVignetteColor = vignette.color.value;

        while (animationTime < fadeTime)
        {
            vignette.color.value = Color.Lerp(startVignetteColor, Color.white, animationTime / fadeTime); 
            vignette.intensity.value = animationTime / fadeTime;
            animationTime += animationStep;
            yield return new WaitForSecondsRealtime(animationStep);
        }

        success?.Invoke();
    }
    
}
