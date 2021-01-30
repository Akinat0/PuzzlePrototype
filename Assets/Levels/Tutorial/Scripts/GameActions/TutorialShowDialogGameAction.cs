using UnityEngine;

namespace Tutorial.GameActions
{
    public class TutorialShowDialogGameAction : TutorialGameAction
    {
        readonly BubbleDialog Dialog;
        readonly string Message;
        readonly float Delay;
        
        float StartTime;
        
        public TutorialShowDialogGameAction(BubbleDialog dialog, string message, float delay)
        {
            Dialog = dialog;
            Message = message;
            Delay = delay;
        }

        public override void Start()
        {
            StartTime = Time.time;
            MobileGameInput.TouchOnTheScreen += TouchOnTheScreenEvent_Handler;
            Dialog.Show(Message);
        }
        
        public override void Update()
        {
            if (Time.time - StartTime >= Delay)
            {
                Dialog.Hide();
                Pop();
            }
        }

        public override void Abort()
        {
            MobileGameInput.TouchOnTheScreen -= TouchOnTheScreenEvent_Handler;
            Dialog.Hide();
        }

        protected override void Pop()
        {
            MobileGameInput.TouchOnTheScreen -= TouchOnTheScreenEvent_Handler;
            base.Pop();
        }

        void TouchOnTheScreenEvent_Handler(Touch _)
        {
            if(!Dialog.Shown)
                return;
            
            Dialog.Hide();
            Pop();
        }
        
    }
}