using System;
using UnityEngine;

namespace Puzzle
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerCollisionDetector : MonoBehaviour
    {
        Collider2D collider;
        
        void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        public event Action<IEnemy> OnCollisionDetected;

        void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            
            if(enemy == null)
                return;

            OnCollisionDetected?.Invoke(enemy);
        }
    }
}