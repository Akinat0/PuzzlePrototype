using System;
using System.Collections;
using UnityEngine;

public static class Coroutines
{
    public static IEnumerator Delay(float delay, Action finished = null)
    {
        yield return new WaitForSeconds(delay);
        finished?.Invoke();
    }
    
    public static IEnumerator FramesDelay(int frames, Action finished = null)
    {
        for (int i = 0; i < frames; i++)
            yield return null;

        finished?.Invoke();
    }
}
