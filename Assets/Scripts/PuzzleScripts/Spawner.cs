using System;
using UnityEngine;
using Random = System.Random;

namespace Puzzle
{
    public enum Side {Left = 0, Up = 1, Right = 2, Down = 3 }
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private float timestep = 1;
        [SerializeField] private float enemySpeed = 0.05f;
        public static Spawner Instance;
        public GameObject enemyPrefab;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _starttime = Time.time;
        }

        private float _starttime;
        private float _timer = 0;
        private void Update()
        {
            float deltatime = Time.time - _starttime;
            _timer += deltatime;
            if (_timer >= timestep)
            {
                Side initSide;
                initSide = (Side) Mathf.RoundToInt(UnityEngine.Random.value * 3);
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.GetComponent<IEnemy>().Instantiate(initSide, enemySpeed);
                enemy.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
                _timer = 0;
            }

            _starttime = Time.time;
        }
    }
}