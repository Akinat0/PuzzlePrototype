namespace Puzzle
{
    public interface IEnemy
    {
        void OnHitPlayer(Player player);

        void Die();

        void Move();
        void Instantiate(Side side, float? speed = null);
        int Damage { get; set; }

    }
}