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
    public static Action<Touch> TouchOnTheScreen;

    public bool Condition { get; private set; }

    void Update()
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
                    TouchOnTheScreen?.Invoke(touch);
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
                TouchOnTheScreen?.Invoke(new Touch {position = Input.mousePosition});
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
    }
    
    private void OnEnable()
    {
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
    }

    void PauseLevelEvent_Handler(bool _Pause)
    {
        Condition = !_Pause;
    }
}
