using Data.Scripts.Tools.Input;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class SelectorComponent<T> : ListBaseComponent<T>
    {
        [SerializeField] protected ButtonComponent RightBtn;
        [SerializeField] protected ButtonComponent LeftBtn;

        protected virtual int Index { get; set; }
        
        protected abstract void MoveLeft();

        protected abstract void MoveRight();
        
        
        protected virtual void OnEnable()
        {
            RightBtn.OnClick += MoveRight;
            LeftBtn.OnClick += MoveLeft;
            MobileInput.OnSwipe += OnSwipeEvent_Handler;
        }
        
        protected virtual void OnDisable()
        {
            RightBtn.OnClick -= MoveRight;
            LeftBtn.OnClick -= MoveLeft;
            MobileInput.OnSwipe -= OnSwipeEvent_Handler;
        }

        protected void OnSwipeEvent_Handler()
        {
            //TODO
        }
    }
}