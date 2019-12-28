using System;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private int _health = 3;
        public bool[] sides = {false, false, true, true}; //It's relative to Side // True means it's stick out
  
        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        public void DealDamage(int damage)
        {
            GameSceneManager.Instance.ShakeCamera();
            _health -= damage;
            for (int i = 0; i < damage; i++)
            {
                GameSceneManager.Instance.InvokePlayerLosedHp(_health);
            }
            if (_health == 0)
            {
                GameSceneManager.Instance.InvokePlayerDied();
            }
        }
         
        public void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            transform.Rotate(new Vector3(0, 0, 180));
        }

        private void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            MobileInput.TouchOnTheScreen += TouchOnScreen_Handler;

        }

        private void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            MobileInput.TouchOnTheScreen -= TouchOnScreen_Handler;
        }

        void ResetLevelEvent_Handler()
        {
            _health = 3;
        }
        void TouchOnScreen_Handler(Touch touch)
        {
            ChangeSides();
        }
    }
    
}