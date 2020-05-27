using System;
using System.Collections;
using UnityEngine;

public static class Extensions 
{
    //Monobehaviour
    public static void Invoke(this MonoBehaviour _monoBehaviour, Action _action, float _time)
    {
        _monoBehaviour.StartCoroutine(CallInTime(_action, _time));
    }

    public static IEnumerator CallInTime(Action _action, float _time)
    {
        yield return new WaitForSeconds(_time);
        
        _action?.Invoke();
    }

    //Float
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
    public static int Remap (this int value, int from1, int to1, int from2, int to2)
    {
        float _value = value;
        return Mathf.RoundToInt(_value.Remap(from1, to1, from2, to2));
    }
    
    //Coroutines
    public static IEnumerator WaitUntil(Func<bool> predicate, Action finish)
    {
        yield return new WaitUntil(predicate);
        finish?.Invoke();
    }
}
