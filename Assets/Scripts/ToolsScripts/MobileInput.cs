using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchProcessor
{
    void OnTouch(Touch touch);

}
public class MobileInput : MonoBehaviour
{
    public static System.Action<Touch> TouchOnTheScreen;
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (touchedCollider != null)
                {
                    ProcessCollider(touchedCollider, touch);
                }
                else
                {
                    TouchOnTheScreen?.Invoke(touch);
                }
            }
        }       
    }

    void ProcessCollider(Collider2D _collider, Touch _touch)
    {
        ITouchProcessor touchProcessor = _collider.GetComponent<ITouchProcessor>();
        touchProcessor?.OnTouch(_touch);
        if (touchProcessor != null)
        {
            touchProcessor.OnTouch(_touch);
        }
        else
        {
            TouchOnTheScreen?.Invoke(_touch);
        }
    }
}
