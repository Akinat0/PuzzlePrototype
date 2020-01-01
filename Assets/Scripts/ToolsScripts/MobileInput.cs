﻿using System.Collections;
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
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
         
            if (hit.collider != null)
            {
                ProcessCollider(hit.collider, new Touch {position = Input.mousePosition});
            }
            else
            {
                TouchOnTheScreen?.Invoke(new Touch {position = Input.mousePosition});
            }
                
        }    
#endif
    
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
