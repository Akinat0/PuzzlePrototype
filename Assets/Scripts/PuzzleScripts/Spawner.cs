using System;
using UnityEngine;
using Random = System.Random;

namespace Puzzle
{
    public enum Side {Left = 0, Up = 1, Right = 2, Down = 3 }
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private float timestep = 1;
        [SerializeField] private float enemySpeed = 0.02f;
        public static Spawner Instance;
        
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject shitPrefab;

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
                GameObject prefabToInstantiate;
                if (UnityEngine.Random.value > 0.7f)
                {
                    prefabToInstantiate = shitPrefab;
                }
                else
                {
                    prefabToInstantiate = enemyPrefab;
                }

                Side initSide;
                initSide = (Side) Mathf.RoundToInt(UnityEngine.Random.value * 3);
                GameObject enemy = Instantiate(prefabToInstantiate);
                enemy.GetComponent<IEnemy>().Instantiate(initSide, enemySpeed);
                
                _timer = 0;
            }

            _starttime = Time.time;
        }
    }

}