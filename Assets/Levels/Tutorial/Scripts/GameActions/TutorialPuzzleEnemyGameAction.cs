using Puzzle;
using PuzzleScripts;

namespace Tutorial.GameActions
{
    public class TutorialPuzzleEnemyGameAction : TutorialGameAction
    {
        readonly Side PuzzleSide;
        readonly float Speed;
        readonly bool StickOut;
        
        public TutorialPuzzleEnemyGameAction(Side side, float speed, bool stickOut)
        {
            PuzzleSide = side;
            Speed = speed;
            StickOut = stickOut;
        }

        public override void Start()
        {
            EnemyParams enemyParams = new EnemyParams
            {
                enemyType = EnemyType.Puzzle,
                side = PuzzleSide,
                speed = Speed,
                stickOut = StickOut
            };
            
            SceneManager.InvokeCreateEnemy(enemyParams);
            Pop();
        }
    }
}