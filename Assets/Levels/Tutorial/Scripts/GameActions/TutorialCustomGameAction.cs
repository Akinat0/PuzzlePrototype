using System;

namespace Tutorial.GameActions
{
    public class TutorialCustomGameAction : TutorialGameAction
    {
        readonly Action CustomAction;
        
        public TutorialCustomGameAction(Action customAction)
        {
            CustomAction = customAction;
        }

        public override void Start()
        {
            CustomAction?.Invoke();
            Pop();
        }
    }
}