using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleEnemy : MonoBehaviour, IEnemy
    {
        private float _speed = 0.05f;
        public Side Side;
        private Vector3 target;
        
        public void OnHitPlayer(Player player)
        {
            if (!player.Sides[Side.GetHashCode()])
            {
                Destroy(gameObject);
            } //That means everything's okay
            else
            {
                player.DealDamage(Damage);
                Destroy(gameObject);
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Move()
        {
            transform.Translate(new Vector3(_speed, 0), Space.Self);
        }

        public void Instantiate(Side side, float? speed = null)
        {
            if (speed != null)
                _speed = (float)speed;
            Side = side;
            transform.Rotate(new Vector3(0, 0, 90 * side.GetHashCode())); 
            target = GameSceneManager.Instance.player.transform.position;
            switch (Side)
            {
                case Side.Right: 
                    transform.position = target + Vector3.right * 10f;
                    transform.right = Vector3.left;
                    break;
                case Side.Left: transform.position = target + Vector3.left * 10f;
                    transform.right = Vector3.right;
                    break;
                case Side.Up: 
                    transform.position = target + Vector3.up * 10f;
                    transform.right = Vector3.down;
                    break;
                case Side.Down: transform.position = target + Vector3.down * 10f;
                    transform.right = Vector3.up;
                    break;
            }
        }


        private void FixedUpdate()
        {
            Move();
        }

        private int _damage = 1;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value;}
        }
    }
}