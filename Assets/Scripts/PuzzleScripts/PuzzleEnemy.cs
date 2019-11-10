using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] GameObject explosion;
        private float _speed = 0.05f;
        public Side side;
        private Vector3 _target;
        
        public void OnHitPlayer(Player player)
        {
            if (!player.sides[side.GetHashCode()])
            {
                Die();
            } //That means everything's okay
            else
            {
                player.DealDamage(Damage);
                Destroy(gameObject);
            }
        }

        public void Die()
        {
            var effect = Instantiate(explosion);
            effect.transform.position = transform.position;
            Destroy(effect, 1.5f);
            GameSceneManager.Instance.score.AddScore(15);
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
            this.side = side;
            transform.Rotate(new Vector3(0, 0, 90 * side.GetHashCode())); 
            _target = GameSceneManager.Instance.player.transform.position;
            switch (this.side)
            {
                case Side.Right: 
                    transform.position = _target + Vector3.right * 10f;
                    transform.right = Vector3.left;
                    break;
                case Side.Left: transform.position = _target + Vector3.left * 10f;
                    transform.right = Vector3.right;
                    break;
                case Side.Up: 
                    transform.position = _target + Vector3.up * 10f;
                    transform.right = Vector3.down;
                    break;
                case Side.Down: transform.position = _target + Vector3.down * 10f;
                    transform.right = Vector3.up;
                    break;
            }
            GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
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