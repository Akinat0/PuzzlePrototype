namespace Tutorial.GameActions
{
    public class TutorialWaitForAllEnemiesDiedAction : TutorialGameAction
    {
        readonly GameEnemiesCounter EnemiesCounter;
        
        public TutorialWaitForAllEnemiesDiedAction(GameEnemiesCounter enemiesCounter)
        {
            EnemiesCounter = enemiesCounter;
        }

        public override void Update()
        {
            if(EnemiesCounter.ExistingEnemies <= 0)
                Pop();
        }
    }
}