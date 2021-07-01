using UnityEngine;

namespace Abu.Tools.GameActionPool
{
    public class WaitAction : GameAction
    {
        readonly float Delay;

        float endTime;
        
        public WaitAction(float delay)
        {
            Delay = delay;
        }
        
        public override void Start()
        {
            if(Delay > 0)
                endTime = Time.time + Delay;
            else
                Pop();
        }

        public override void Update()
        {
            if (Time.time >= endTime)
                Pop();
        }

        public override void Abort() { }
        public override void Dispose() { }
    }
}