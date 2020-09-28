using Puzzle;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public interface ITouchProcessor
{
    void OnTouch(Touch touch);
}

public class MobileGameInput : MonoBehaviour
{
    public static event Action<Touch> TouchOnTheScreen;
    public static event Action<Touch> TouchRegistered; //Called each time your touch was registered

    public virtual bool Condition { get; protected set; }

    protected virtual void Update()
    {
        if (!Condition)
            return;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    //Touch on UI element
                    return;

                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (touchedCollider != null)
                {
                    ProcessCollider(touchedCollider, touch);
                }
                else
                {
                    TouchOnScreen(touch);
                }
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            
            if (EventSystem.current.IsPointerOverGameObject())
                //Mouse on UI element
                return;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                ProcessCollider(hit.collider, new Touch {position = Input.mousePosition});
            }
            else
            {
                
                TouchOnScreen(new Touch {position = Input.mousePosition});
            }
                
        }    
#endif
        
    }

    void ProcessCollider(Collider2D _collider, Touch _touch)
    {
        ITouchProcessor touchProcessor = _collider.GetComponent<ITouchProcessor>();

        if (touchProcessor != null)
        {
            touchProcessor.OnTouch(_touch);
        }
        else
        {
            TouchOnTheScreen?.Invoke(_touch);
        }
        
        InvokeTouchRegistered(_touch);
    }

    protected void TouchOnScreen(Touch touch)
    {
        TouchOnTheScreen?.Invoke(touch);
        InvokeTouchRegistered(touch);
    }
    
    protected virtual void OnEnable()
    {
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
    }

    void PauseLevelEvent_Handler(bool _Pause)
    {
        Condition = !_Pause;
    }

    private void InvokeTouchRegistered(Touch touch)
    {
        Debug.Log("Touch registered " + touch.position);
        TouchRegistered?.Invoke(touch);
    }
    
}
