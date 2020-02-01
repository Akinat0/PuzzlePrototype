using PuzzleScripts;

namespace Puzzle
{
    public interface IEnemy
    {
        void OnHitPlayer(Player player);

        void Die();

        void Move();
        void Instantiate(EnemyParams @params);
        int Damage { get; set; }

    }
}