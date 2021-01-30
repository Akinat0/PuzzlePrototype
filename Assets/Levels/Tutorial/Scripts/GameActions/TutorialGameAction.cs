using Puzzle;

namespace Tutorial.GameActions
{
    public abstract class TutorialGameAction : GameAction
    {
        protected TutorialSceneManager SceneManager => GameSceneManager.Instance as TutorialSceneManager;
        
        public override void Start()
        { }

        public override void Update()
        { }

        public override void Abort()
        { }
    }
}