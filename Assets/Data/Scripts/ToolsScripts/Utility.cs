using System;
using System.Collections;
using Abu.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Utility 
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
    
    
    //Colors
    public static void SetAlpha(this TextMeshProUGUI textMesh, float value)
    {
        Color color = textMesh.color;
        color.a = value;
        textMesh.color = color;
    }
    
    //Transform
    public static void SetX(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.x = value;
        transform.position = position;
    }
    
    public static void SetY(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.y = value;
        transform.position = position;
    }
    
    //RectTransform
    public static bool IsVisibleOnTheScreen(this RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners (corners);
 
        float maxY = Mathf.Max (corners [0].y, corners [1].y, corners [2].y, corners [3].y);
        float minY = Mathf.Min (corners [0].y, corners [1].y, corners [2].y, corners [3].y);

        float maxX = Mathf.Max (corners [0].x, corners [1].x, corners [2].x, corners [3].x);
        float minX = Mathf.Min (corners [0].x, corners [1].x, corners [2].x, corners [3].x);

        Vector2 screenSize = ScreenScaler.ScreenSize;
        
        return maxY > 0 && minY < screenSize.y && maxX > 0 && minX < screenSize.x;
        
    }
    
    
}
