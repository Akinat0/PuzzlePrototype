using System;
using Data.Scripts.Tools.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;
using Input = UnityEngine.Input;


public class MobileSwipe
{

    Vector2 previousPressPos;
    Vector2 latestPressPos;
    Vector2 currentSwipe;

    public void Update()
    {
#if UNITY_EDITOR
        SwipeMouse();
#else
        SwipeMobile();
#endif
    }

    public event Action<Vector2> OnTouchStart;  //start position
    public event Action<Vector2> OnTouchMove;   //delta  
    public event Action<Vector2> OnTouchCancel; //end position

    bool inProgress;
    
    [Preserve]
    void SwipeMobile()
    {
        if (Input.touches.Length <= 0)
            return;

        Touch t = Input.GetTouch(0);

        if (EventSystem.current.IsPointerOverGameObject(t.fingerId) || !MobileInput.Condition)
        {
            if (inProgress)
                SwipeEnded(t.position);
            return;
        }

        switch (t.phase)
        {
            case TouchPhase.Began:
                SwipeBegan(t.position);
                break;
            case TouchPhase.Moved:
                SwipeMoved(t.position);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                SwipeEnded(t.position);
                break;
        }
    
    }

    void SwipeMouse()
    {
        
        if (EventSystem.current.IsPointerOverGameObject() || !MobileInput.Condition)
        {
            if (inProgress)
                SwipeEnded(Input.mousePosition);
            return;
        }
        
        if(!inProgress && Input.GetMouseButtonDown(0))
            SwipeBegan(Input.mousePosition);
        
        if(inProgress && Input.GetMouseButton(0))
            SwipeMoved(Input.mousePosition);
            
        if(inProgress && Input.GetMouseButtonUp(0))
            SwipeEnded(Input.mousePosition);
        
    }
    
    void SwipeBegan(Vector2 position)
    {
        inProgress = true;
        
        previousPressPos = position;
        latestPressPos = position;

        OnTouchStart?.Invoke(latestPressPos);
    }

    void SwipeMoved(Vector2 position)
    {
        previousPressPos = latestPressPos;
        latestPressPos = position;
        
        OnTouchMove?.Invoke(latestPressPos - previousPressPos);
    }

    void SwipeEnded(Vector2 position)
    {
        inProgress = false;
        
        previousPressPos = Vector2.zero;
        latestPressPos = Vector2.zero;
        OnTouchCancel?.Invoke(position);
    }
    
//    
//    private void SwipeMouse()
//    {
//        //Mouse on UI element
//        if (EventSystem.current.IsPointerOverGameObject() || !MobileInput.Condition)
//        {
//            firstPressPos = Vector2.zero;
//            return;
//        }
//        
//        if(Input.GetMouseButtonDown(0))
//        {
//            //save began touch 2d point
//            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
//        }
//        if(Input.GetMouseButtonUp(0))
//        {
//            //save ended touch 2d point
//            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
//   
//            //create vector from the two points
//            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
//
//            Vector2 cameraSize = ScreenScaler.CameraSize;
//
//            if (Mathf.Abs(currentSwipe.x) < cameraSize.x * horizontalSensitivity ||
//                Mathf.Abs(currentSwipe.y) < cameraSize.y * verticalSensitivity)
//                return;
//            
//            Vector2 normalizedSwipe = currentSwipe.normalized;
//
//            if(Mathf.Abs(normalizedSwipe.x) > Mathf.Abs(normalizedSwipe.y)) //Horizontal or verical swipe
//            {
//                //swipe left
//                if(currentSwipe.x < 0)
//                {
//                    OnSwipe?.Invoke(SwipeType.Left);
//                    Debug.Log("Swipe left");
//                }
//                else 
//                //swipe right
//                {
//                    OnSwipe?.Invoke(SwipeType.Right);
//                    Debug.Log("Swipe right");
//                }
//            }
//            else
//            {
//                //swipe upwards
//                if(currentSwipe.y > 0)
//                {
//                    OnSwipe?.Invoke(SwipeType.Up);
//                    Debug.Log("Swipe up");
//                }
//                else
//                //swipe down
//                if(currentSwipe.y < 0)
//                {
//                    OnSwipe?.Invoke(SwipeType.Down);
//                    Debug.Log("Swipe down");
//                }
//            }
//            
//        }
//    }

}