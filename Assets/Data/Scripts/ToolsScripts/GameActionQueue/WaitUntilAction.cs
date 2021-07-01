using System;

namespace Abu.Tools.GameActionPool
{
    public class WaitUntilAction : GameAction
    {
        readonly Func<bool> Predicate;

        public WaitUntilAction(Func<bool> predicate)
        {
            Predicate = predicate;
        }

        public override void Start()
        {
            if(Predicate.Invoke())
                Pop();
        }

        public override void Update()
        {
            if(Predicate.Invoke())
                Pop();
        }

        public override void Abort() { }
        public override void Dispose() { }
    }
}