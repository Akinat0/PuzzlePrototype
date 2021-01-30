using Abu.Tools;
using Puzzle;
using UnityEngine;

public class RippleOnClickEffect : MonoBehaviour
{
    void OnEnable()
    {
        MobileGameInput.TouchRegistered += TouchRegistered_Handler;
    }
    
    void OnDisable()
    {
        MobileGameInput.TouchRegistered -= TouchRegistered_Handler;
    }

    void TouchRegistered_Handler(Touch touch)
    {
        Vector3 position = ScreenScaler.MainCamera.ScreenToWorldPoint(touch.position);
        VFXManager.Instance.CallTapRippleEffect(position);
    }
}
