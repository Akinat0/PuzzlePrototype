using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Abu.Tools
{
    public enum SwipeType
    {
        Up,
        Down,
        Right,
        Left
    }
    
    public class MobileSwipeInputComponent : MonoBehaviour
    {
        public Action<SwipeType> OnSwipe;
        
        //inside class
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        private void Update()
        {
#if UNITY_EDITOR
            SwipeMouse();            
#else
            SwipeMobile();
#endif
        }

        private void SwipeMobile()
        {
            if(Input.touches.Length > 0)
            {
                
                Touch t = Input.GetTouch(0);
                
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                    //Touch on UI element
                    return;
                
                if(t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    firstPressPos = new Vector2(t.position.x,t.position.y);
                }
                if(t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    secondPressPos = new Vector2(t.position.x,t.position.y);
                           
                    //create vector from the two points
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
               
                    //normalize the 2d vector
                    currentSwipe.Normalize();
 
                    //swipe upwards
                    if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        OnSwipe?.Invoke(SwipeType.Up);
                        Debug.Log("Swipe up");
                    }
                    //swipe down
                    if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        OnSwipe?.Invoke(SwipeType.Down);
                        Debug.Log("Swipe down");
                    }
                    //swipe left
                    if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        OnSwipe?.Invoke(SwipeType.Left);
                        Debug.Log("Swipe left");
                    }
                    //swipe right
                    if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        OnSwipe?.Invoke(SwipeType.Right);
                        Debug.Log("Swipe right");
                    }
                }
            }
        }
        
        private void SwipeMouse()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                //Mouse on UI element
                return;
            
            if(Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            }
            if(Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
       
                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
           
                //normalize the 2d vector
                currentSwipe.Normalize();
                
                //swipe upwards
                if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    OnSwipe?.Invoke(SwipeType.Up);
                    Debug.Log("Swipe up");
                }
                //swipe down
                if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    OnSwipe?.Invoke(SwipeType.Down);
                    Debug.Log("Swipe down");
                }
                //swipe left
                if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    OnSwipe?.Invoke(SwipeType.Left);
                    Debug.Log("Swipe left");
                }
                //swipe right
                if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    OnSwipe?.Invoke(SwipeType.Right);
                    Debug.Log("Swipe right");
                }
            }
        }
    }
}