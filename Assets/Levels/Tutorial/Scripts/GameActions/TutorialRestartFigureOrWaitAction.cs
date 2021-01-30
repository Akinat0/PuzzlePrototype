using System;
using Puzzle;

namespace Tutorial.GameActions
{
    public class TutorialRestartFigureOrWaitAction : TutorialGameAction
    {
        readonly GameEnemiesCounter EnemiesCounter;
        readonly Action RestartFigure;
        
        public TutorialRestartFigureOrWaitAction(GameEnemiesCounter enemiesCounter, Action restartFigure)
        {
            EnemiesCounter = enemiesCounter;
            RestartFigure = restartFigure;
        }

        public override void Start()
        {
            GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        }

        public override void Update()
        {
            if(EnemiesCounter.ExistingEnemies <= 0)
                Pop();
        }

        protected override void Pop()
        {
            GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
            base.Pop();
        }
        
        void PlayerLosedHpEvent_Handler()
        {
            GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
            RestartFigure?.Invoke();
        }

        
    }
}