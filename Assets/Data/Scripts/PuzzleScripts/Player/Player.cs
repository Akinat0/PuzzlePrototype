using System;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        private PlayerView _view;

        private bool _immuneFramesEnabled;
        private float _immuneTime = 0.2f;

        [SerializeField]
        private int _health = 3;
        public bool[] sides = {false, false, true, true}; //It's relative to Side // True means it's stick out
  
        
        void Start()
        {
            _view = GetComponent<PlayerView>();
            _immuneFramesEnabled = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        public void DealDamage(int damage)
        {
            GameSceneManager.Instance.ShakeCamera();
            if (!_immuneFramesEnabled) {
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
            _immuneFramesEnabled = true;
            Invoke( "DisableImmune", _immuneTime);
        }

        private void DisableImmune()
        {
            _immuneFramesEnabled = false;
        }
        
         
        public void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            _view.ChangeSides();
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