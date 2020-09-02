﻿using System;
using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        private PlayerView _view;

        protected bool _immuneFramesEnabled;
        protected float _immuneTime = 0.2f;

        public const int DEFAULT_HP = 5;
        protected int _health = DEFAULT_HP;
        
        public bool[] sides = {false, false, true, true}; //It's relative to Side // True means it's stick out

        public PlayerView PlayerView => _view;
        public int Hp => _health;
        
        protected virtual void Awake()
        {
            _view = GetComponent<PlayerView>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        public virtual void DealDamage(int damage)
        {
            GameSceneManager.Instance.ShakeCamera();
            if (!_immuneFramesEnabled)
            {
                _health -= damage;
                for (int i = 0; i < damage; i++)
                {
                    GameSceneManager.Instance.InvokePlayerLosedHp(_health);
                }

                if (_health == 0)
                {
                    //We will wait one frame before kill the player
                    StartCoroutine(Utility.WaitFrames(1, () => GameSceneManager.Instance.InvokePlayerDied()));
                }
            }

            _immuneFramesEnabled = true;
            Invoke(nameof(DisableImmune), _immuneTime);
        }

        protected void DisableImmune()
        {
            _immuneFramesEnabled = false;
        }

        public virtual void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            _view.ChangeSides();
        }

        private void ResetHp()
        {
            _health = DEFAULT_HP;
        }

        private void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PlayerReviveEvent += PlayerReviveEventHandler;
            MobileGameInput.TouchOnTheScreen += TouchOnScreen_Handler;

        }

        private void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PlayerReviveEvent -= PlayerReviveEventHandler;
            MobileGameInput.TouchOnTheScreen -= TouchOnScreen_Handler;
        }

        void ResetLevelEvent_Handler()
        {
            ResetHp();
        }
        
        void PlayerReviveEventHandler()
        {
            ResetHp();
        }
        
        void TouchOnScreen_Handler(Touch touch)
        {
            ChangeSides();
        }
        
        
    }
    
}