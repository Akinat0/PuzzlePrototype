using System;

namespace Abu.Tools.GameActionPool
{
    public class CallbackAction : GameAction
    {
        readonly Action Callback;
        
        public CallbackAction(Action callback)
        {
            Callback = callback;
        }
        
        public override void Start()
        {
            Callback?.Invoke();
            Pop();
        }

        public override void Update() { }

        public override void Abort()
        {
            Callback?.Invoke();
        }

        public override void Dispose()
        {
            Callback?.Invoke();
        }
    }
}