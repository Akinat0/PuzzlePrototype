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

    void TouchRegistered_Handler(Vector3 position)
    {
        VFXManager.Instance.CallTapRippleEffect(position);
    }
}
