using System;

namespace Abu.Tools
{
    public class DelegateGroup
    {
        Action OnComplete { get; }

        int Count { get; }

        int InvokedCount { get; set; }

        public DelegateGroup(int count, Action onComplete)
        {
            OnComplete = onComplete;
            Count = count;
        }

        public void Invoke()
        {
            InvokedCount++;
            
            if (InvokedCount != Count) 
                return;
            
            OnComplete?.Invoke();    
        }

        public static implicit operator Action(DelegateGroup delegateGroup) => delegateGroup.Invoke;
    }
}