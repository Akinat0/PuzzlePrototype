using Puzzle;
using UnityEngine;
using System;
using Abu.Tools;
using UnityEngine.EventSystems;

public interface ITouchProcessor
{
    void OnTouch();
}

public class MobileGameInput : MonoBehaviour
{
    public static event Action<Vector3> TouchOnTheScreen;
    public static event Action<Vector3> TouchRegistered; //Called each time your touch was registered

    public bool Condition { get; protected set; }
    float touchRadius; 

    void Start()
    {
        touchRadius = ScreenScaler.CameraSize.x / 10;
    }

    protected void Update()
    {
        if (!Condition)
            return;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    //Touch on UI element
                    return;
                
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D[] touchedColliders = Physics2D.OverlapCircleAll(touchPosition, touchRadius);
                
                if (touchedColliders.Length > 0)
                    ProcessColliders(touchedColliders, touchPosition);
                else
                    TouchOnScreen(touchPosition);
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                //Mouse on UI element
                return;
            
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] touchedColliders = Physics2D.OverlapCircleAll(clickPosition, touchRadius);

            if (touchedColliders.Length > 0)
                ProcessColliders(touchedColliders, clickPosition);
            else
                TouchOnScreen(clickPosition);
        }    
#endif
    }

    void ProcessColliders(Collider2D[] colliders, Vector3 position)
    {
        bool isTouchOnScreen = true;
        foreach (Collider2D collider in colliders)
        {
            ITouchProcessor touchProcessor = collider.GetComponent<ITouchProcessor>();

            if (touchProcessor == null) continue;
            
            touchProcessor.OnTouch();
            isTouchOnScreen = false;
        }
        
        if(isTouchOnScreen)
            TouchOnTheScreen?.Invoke(position);

        InvokeTouchRegistered(position);
    }

    protected void TouchOnScreen(Vector3 position)
    {
        TouchOnTheScreen?.Invoke(position);
        InvokeTouchRegistered(position);
    }
    
    protected void OnEnable()
    {
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
    }

    protected void OnDisable()
    {
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
    }

    void PauseLevelEvent_Handler(bool pause)
    {
        Condition = !pause;
    }

    void InvokeTouchRegistered(Vector3 position)
    {
        Debug.Log("Touch registered " + position);
        TouchRegistered?.Invoke(position);
    }
    
}
