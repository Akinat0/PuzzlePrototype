using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        private int health = 1000;
        
        public bool[] Sides = {false, false, true, true}; //It's relative to Side // True means it's stick out
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        private void PlayerDied()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        public void DealDamage(int damage)
        {
            for (int i = 0; i < damage; i++)
            {
                GameSceneManager.Instance.healthManager.LoseHeart();
            }
            health -= damage;
            if (health == 0)
            {
                PlayerDied();
            }
            
        }

        public void ChangeSides()
        {
            for (int i = 0; i < Sides.Length; i++)
                Sides[i] = !Sides[i];
            transform.Rotate(new Vector3(0, 0, 180));
        }
    }
    
}