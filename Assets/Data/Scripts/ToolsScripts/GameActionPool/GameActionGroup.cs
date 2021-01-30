using System.Collections.Generic;

namespace Abu.Tools.GameActionPool
{
    public class GameActionGroup : GameAction
    {
        readonly List<GameAction> GameActions;

        public GameActionGroup(params GameAction[] gameActions)
        {
            GameActions = new List<GameAction>(gameActions);
            
            foreach (GameAction gameAction in GameActions)
                gameAction.OnComplete += () => { OnGameActionCompleted(gameAction); };
        }
        
        public override void Start()
        {
            foreach (GameAction gameAction in GameActions)
                gameAction.Start();
        }

        public override void Update()
        {
            foreach (GameAction gameAction in GameActions)
                gameAction.Update();
        }

        public override void Abort()
        {
            foreach (GameAction gameAction in GameActions)
                gameAction.Abort();
        }

        void OnGameActionCompleted(GameAction gameAction)
        {
            GameActions.Remove(gameAction);
            
            if(GameActions.Count > 0 )
                return;
            
            Pop();
        }
    }
}