using UnityEngine;

namespace Tutorial.GameActions
{
    public class TutorialDelayGameAction : TutorialGameAction
    {
        readonly float Delay;
        float StartTime;

        public TutorialDelayGameAction(float delay)
        {
            Delay = delay;
        }

        public override void Start()
        {
            StartTime = Time.time;
        }

        public override void Update()
        {
            if(Time.time - StartTime >= Delay)
                Pop();
        }
        
    }
}