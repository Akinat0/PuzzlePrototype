using Puzzle;
using PuzzleScripts;

namespace Tutorial.GameActions
{
    public class TutorialShitEnemyGameAction : TutorialGameAction
    {
        readonly Side Side;
        readonly float Speed;
        
        public TutorialShitEnemyGameAction(Side side, float speed)
        {
            Side = side;
            Speed = speed;
        }

        public override void Start()
        {
            EnemyParams enemyParams = new EnemyParams
            {
                enemyType = EnemyType.Shit,
                side = Side,
                speed = Speed
            };
            
            SceneManager.InvokeCreateEnemy(enemyParams);
            Pop();
        }
    }
}