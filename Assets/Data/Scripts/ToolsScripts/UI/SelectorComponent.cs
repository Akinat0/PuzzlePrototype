using System;
using System.Collections;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class SelectorComponent<T> : ListBaseComponent<T>
    {
        #region serialized
        
        [SerializeField] protected ButtonComponent RightBtn;
        [SerializeField] protected ButtonComponent LeftBtn;

        #endregion
        
        #region Index
        
        int index;

        protected virtual int Index
        {
            get => index;
            set
            {
                index = value;
                ProcessIndex();
            }
        }

        #endregion

        #region Offset
        float offset;
        protected float Offset
        {
            get => offset;
            set
            {
                if (Mathf.Approximately(offset, value))
                    return;

                offset = value;
                
                ProcessOffset();
            } 
        }

        #endregion

        #region properties
        
        protected virtual float TouchSensitivity => 2.2f;
        protected virtual T Current => Selection[Index];
        protected virtual bool IsFocused { get; set; }
        protected MobileSwipe MobileSwipe => mobileSwipe;

        readonly MobileSwipe mobileSwipe = new MobileSwipe();

        #endregion
        
        #region methods
        
        protected abstract void MoveLeft();

        protected abstract void MoveRight();

        protected virtual void Update()
        {
            mobileSwipe.Update();
        }

        protected abstract void ProcessOffset();
        
        protected abstract void ProcessIndex();
        
        protected virtual void OnEnable()
        {
            mobileSwipe.OnTouchStart += OnTouchDown_Handler;
            mobileSwipe.OnTouchMove += OnTouchMove_Handler;
            mobileSwipe.OnTouchCancel += OnTouchCancel_Handler;
            RightBtn.OnClick += MoveRight;
            LeftBtn.OnClick += MoveLeft;
        }

        protected virtual void OnDisable()
        {
            mobileSwipe.OnTouchStart -= OnTouchDown_Handler;
            mobileSwipe.OnTouchMove -= OnTouchMove_Handler;
            mobileSwipe.OnTouchCancel -= OnTouchCancel_Handler;
            RightBtn.OnClick -= MoveRight;
            LeftBtn.OnClick -= MoveLeft;
        }

        protected virtual void OnTouchDown_Handler(Vector2 position)
        { }
        
        protected virtual void OnTouchMove_Handler(Vector2 delta)
        { }
        
        protected virtual void OnTouchCancel_Handler(Vector2 position)
        { }
        
        #endregion
        
        #region coroutines
        protected IEnumerator TimedAfterTouchRoutine(float duration, Action finished = null)
        {
            float startOffset = Offset;
            int targetIndex = Mathf.RoundToInt(startOffset);

            float time = 0;
        
            while (time <= duration)
            {
                Offset = Mathf.Lerp(startOffset, targetIndex, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            Index = targetIndex;
            finished?.Invoke();
        }

        protected IEnumerator MoveToIndexRoutine(int targetIndex, float duration, Action finished = null)
        {
            float time = 0;
            float startOffset = Offset;
            while (time < duration)
            {
                Offset = Mathf.Lerp(startOffset, targetIndex, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            Index = targetIndex;
            finished?.Invoke();
        } 
        
        #endregion
    }
}