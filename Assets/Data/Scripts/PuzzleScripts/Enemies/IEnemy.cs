namespace Puzzle
{
    public interface IEnemy
    {
        void OnHitPlayer(Player player);
        bool CanDamagePlayer(Player player);
    }
}