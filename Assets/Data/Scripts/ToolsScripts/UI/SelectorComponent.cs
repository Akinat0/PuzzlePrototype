using System;
using Data.Scripts.Tools.Input;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class SelectorComponent<T> : ListBaseComponent<T>
    {
        [SerializeField] protected ButtonComponent RightBtn;
        [SerializeField] protected ButtonComponent LeftBtn;

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

        protected virtual bool IsFocused { get; set; }

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

        protected virtual T Current => Selection[Index];

        protected MobileSwipe MobileSwipe => mobileSwipe;

        readonly MobileSwipe mobileSwipe = new MobileSwipe();
        
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
    }
}