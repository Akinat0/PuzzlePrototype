using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class VirusEnemy : ShitEnemy
    {
        [HideInInspector] public float radialPosition;

        public override void Instantiate(EnemyParams @params)
        {
            radialPosition = @params.radialPosition;
            base.Instantiate(@params);

            Player player = GameSceneManager.Instance.GetPlayer();
            Vector3 zAxis = new Vector3(0, 0, 1);

            transform.RotateAround(player.transform.position, zAxis, radialPosition);
        }
    }
}