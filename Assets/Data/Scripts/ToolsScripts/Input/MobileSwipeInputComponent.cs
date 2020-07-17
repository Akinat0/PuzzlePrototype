using System;
using Data.Scripts.Tools.Input;
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
        [SerializeField, Range(0, 1)] float HorizontalSensitivity = 0.23f;
        [SerializeField, Range(0, 1)] float VerticalSensitivity = 0.23f; 
        
        public Action<SwipeType> OnSwipe;
        
        //inside class
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        private void Update()
        {
#if UNITY_EDITOR
            //SwipeMouse(); //toggle if you want to use mouse swipe
            SwipeButtons();
#else
            SwipeMobile();
#endif
        }

        private void SwipeMobile()
        {
            if(Input.touches.Length > 0)
            {
                
                Touch t = Input.GetTouch(0);
                
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId) || !MobileInput.Condition)
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
                    currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
               
                    Vector2 cameraSize = ScreenScaler.CameraSize;

                    if (Mathf.Abs(currentSwipe.x) < cameraSize.x * HorizontalSensitivity ||
                        Mathf.Abs(currentSwipe.y) < cameraSize.y * VerticalSensitivity)
                        return;
                
                    Vector2 normalizedSwipe = currentSwipe.normalized;

                    if(Mathf.Abs(normalizedSwipe.x) > Mathf.Abs(normalizedSwipe.y)) //Horizontal or verical swipe
                    {
                        //swipe left
                        if(currentSwipe.x < 0)
                        {
                            OnSwipe?.Invoke(SwipeType.Left);
                            Debug.Log("Swipe left");
                        }
                        else 
                            //swipe right
                        {
                            OnSwipe?.Invoke(SwipeType.Right);
                            Debug.Log("Swipe right");
                        }
                    }
                    else
                    {
                        //swipe upwards
                        if(currentSwipe.y > 0)
                        {
                            OnSwipe?.Invoke(SwipeType.Up);
                            Debug.Log("Swipe up");
                        }
                        else
                            //swipe down
                        if(currentSwipe.y < 0)
                        {
                            OnSwipe?.Invoke(SwipeType.Down);
                            Debug.Log("Swipe down");
                        }
                    }
                }
            }
        }
        
        private void SwipeMouse()
        {
            //Mouse on UI element
            if (EventSystem.current.IsPointerOverGameObject() || !MobileInput.Condition)
            {
                firstPressPos = Vector2.zero;
                return;
            }
                
            
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

                Vector2 cameraSize = ScreenScaler.CameraSize;

                if (Mathf.Abs(currentSwipe.x) < cameraSize.x * HorizontalSensitivity ||
                    Mathf.Abs(currentSwipe.y) < cameraSize.y * VerticalSensitivity)
                    return;
                
                Vector2 normalizedSwipe = currentSwipe.normalized;

                if(Mathf.Abs(normalizedSwipe.x) > Mathf.Abs(normalizedSwipe.y)) //Horizontal or verical swipe
                {
                    //swipe left
                    if(currentSwipe.x < 0)
                    {
                        OnSwipe?.Invoke(SwipeType.Left);
                        Debug.Log("Swipe left");
                    }
                    else 
                    //swipe right
                    {
                        OnSwipe?.Invoke(SwipeType.Right);
                        Debug.Log("Swipe right");
                    }
                }
                else
                {
                    //swipe upwards
                    if(currentSwipe.y > 0)
                    {
                        OnSwipe?.Invoke(SwipeType.Up);
                        Debug.Log("Swipe up");
                    }
                    else
                    //swipe down
                    if(currentSwipe.y < 0)
                    {
                        OnSwipe?.Invoke(SwipeType.Down);
                        Debug.Log("Swipe down");
                    }
                }
                
            }
        }

        private void SwipeButtons()
        {
            if (!MobileInput.Condition)
                return;

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSwipe?.Invoke(SwipeType.Down);
                Debug.Log("Swipe down");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnSwipe?.Invoke(SwipeType.Up);
                Debug.Log("Swipe up");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnSwipe?.Invoke(SwipeType.Right);
                Debug.Log("Swipe right");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnSwipe?.Invoke(SwipeType.Left);
                Debug.Log("Swipe left");
            }

        }
    }
}