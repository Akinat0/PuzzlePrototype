using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public static class Coroutines
{
    public static IEnumerator Delay(float delay, Action finished = null)
    {
        yield return new WaitForSeconds(delay);
        finished?.Invoke();
    }

    public static IEnumerator DelayRealtime(float delay, Action finished = null)
    {
        yield return new WaitForSecondsRealtime(delay);
        finished?.Invoke();
    }

    public static IEnumerator LerpRoutine(Func<float> valueGetter, Action<float> valueSetter, float targetValue, float duration, Action finished = null)
    {
        float sourceValue = valueGetter();
        float time = 0;

        while (time < duration)
        {
            yield return null;
            time += Time.deltaTime;
            valueSetter(Mathf.Lerp(sourceValue, targetValue, time / duration));
        }
        
        valueSetter(targetValue);
        
        finished?.Invoke();
    }

    public static IEnumerator WaitUntil(Func<bool> predicate, Action finished)
    {
        yield return new WaitUntil(predicate);
        finished?.Invoke();
    }
    
    public static IEnumerator Repeat(float delay, [NotNull] Action repeatAction)
    {
        while (true)
        {
            repeatAction.Invoke();
            yield return new WaitForSeconds(delay);
        }
    }
    
    public static IEnumerator RepeatRealtime(float delay, [NotNull] Action repeatAction)
    {
        while (true)
        {
            repeatAction.Invoke();
            yield return new WaitForSecondsRealtime(delay);
        }
    }
    
    public static IEnumerator RepeatUntil(float delay, [NotNull] Action repeatAction, [NotNull] Func<bool> shouldRepeat, Action finished = null)
    {
        while (shouldRepeat.Invoke())
        {
            repeatAction.Invoke();
            yield return new WaitForSeconds(delay);
        }
        
        finished?.Invoke();
    }
    
    public static IEnumerator RepeatUntilRealtime(float delay, [NotNull] Action repeatAction, [NotNull] Func<bool> shouldRepeat, Action finished = null)
    {
        while (shouldRepeat.Invoke())
        {
            repeatAction.Invoke();
            yield return new WaitForSecondsRealtime(delay);
        }
        
        finished?.Invoke();
    }

    public static IEnumerator FramesDelay(int frames, Action finished = null)
    {
        for (int i = 0; i < frames; i++)
            yield return null;

        finished?.Invoke();
    }
}
