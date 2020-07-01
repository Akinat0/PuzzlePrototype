using Data.Scripts.Tools.Input;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class SelectorComponent<T> : ListBaseComponent<T>
    {
        [SerializeField] protected ButtonComponent RightBtn;
        [SerializeField] protected ButtonComponent LeftBtn;
        
        protected virtual int Index { get; set; }

        protected virtual bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                if (swipeInput)
                    swipeInput.enabled = value;
            }
        }

        protected virtual T Current => Selection[Index];
        
        MobileSwipeInputComponent swipeInput;
        bool isFocused;
        
        protected abstract void MoveLeft();

        protected abstract void MoveRight();

        protected virtual void Awake()
        {
            swipeInput = GetComponent<MobileSwipeInputComponent>();
        }
        
        protected virtual void OnEnable()
        {
            RightBtn.OnClick += MoveRight;
            LeftBtn.OnClick += MoveLeft;
        
            if(swipeInput)
                swipeInput.OnSwipe += OnSwipeEvent_Handler;
        }

        protected virtual void OnDisable()
        {
            RightBtn.OnClick -= MoveRight;
            LeftBtn.OnClick -= MoveLeft;
            
            if(swipeInput)
                swipeInput.OnSwipe -= OnSwipeEvent_Handler;
        }

        void OnSwipeEvent_Handler(SwipeType swipeType)
        {
            if (!IsFocused)
                return;
            
            switch (swipeType)
            {
                case SwipeType.Right:
                    MoveLeft();
                    break;
                case SwipeType.Left:
                    MoveRight();
                    break;
            }
        }
    }
}