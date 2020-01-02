﻿using System;
using Puzzle;
using UnityEngine;

namespace PuzzleScripts
{
    public enum EnemyType
    {
        Puzzle, 
        Shit
    }

    public class EnemyBase : MonoBehaviour, IEnemy, ITouchProcessor
    {
        public static readonly float Distance = 10f; //Distance to target
        
        [SerializeField] private GameObject vfx;
        [SerializeField] private int score;
        [SerializeField] private EnemyType type;
        
        private float _speed = 0.05f;
        private Vector3 _target;
    
        private int _damage = 1;

        public EnemyType Type { get { return type; } }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value;}
        }
    
        private void FixedUpdate()
        {
            Move();
        }

        public virtual void OnHitPlayer(Player player)
        {
            player.DealDamage(Damage);
            if(gameObject != null)
                Destroy(gameObject);
        }

        public void Die()
        {
            GameObject effect = Instantiate(vfx);
            effect.transform.position = transform.position;
            GameSceneManager.Instance.InvokeEnemyDied(score);
            Destroy(gameObject);
        }

        public void Move()
        {
            transform.Translate(new Vector3(_speed, 0), Space.Self);
        }

        public virtual void Instantiate(Side side, float? speed = null)
        {
            if (speed != null)
                _speed = (float) speed;
            transform.Rotate(new Vector3(0, 0, 90 * side.GetHashCode()));
            _target = GameSceneManager.Instance.GetPlayerPos();
            switch (side)
            {
                case Side.Right:
                    transform.position = _target + Vector3.right * Distance;
                    transform.right = Vector3.left;
                    break;
                case Side.Left:
                    transform.position = _target + Vector3.left * Distance;
                    transform.right = Vector3.right;
                    break;
                case Side.Up:
                    transform.position = _target + Vector3.up * Distance;
                    transform.right = Vector3.down;
                    break;
                case Side.Down:
                    transform.position = _target + Vector3.down * Distance;
                    transform.right = Vector3.up;
                    break;
            }
        }
    
        private void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        }

        private void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            Destroy(gameObject);
        }
    
        public void OnTouch(Touch touch)
        {
            Die();
        }
    }

    [Serializable]
    public struct EnemyParams
    {
        public EnemyType enemyType;
        public Side side;
        public float speed;
    }
}