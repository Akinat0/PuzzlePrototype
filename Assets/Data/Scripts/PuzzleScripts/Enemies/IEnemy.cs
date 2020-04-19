using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public interface IEnemy
    {
        void OnHitPlayer(Player player);

        Transform Die();

        void Move();
        void Instantiate(EnemyParams @params);
        int Damage { get; set; }

        void SetCoinHolder(int CostOfEnemy);
    }
}