using System;
using Abu.Tools;
using UnityEditor;
using UnityEngine;

namespace Puzzle
{
    public enum Side {Left = 0, Up = 1, Right = 2, Down = 3 }
    public class Spawner : MonoBehaviour
    {

        public static Spawner Instance;
        
        [SerializeField] private float spawnTimestep = 1;
        [SerializeField] private float startEnemySpeed = 0.02f;
        [SerializeField] private float hightestEnemySpeed = 0.1f;
        [SerializeField] private float timeToHighComplexity = 60f;

        [Tooltip("The percent which player's pazzle will take on the any screen")]
        [SerializeField] private float partOfThePLayerOnTheScreen = 0.25f;
        
        [SerializeField] private GameObject playerEntity;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject shitPrefab;

        [SerializeField] private GameObject background;
        
        private float _gameScale;
        
        private float _enemySpeed;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            RescaleGame();
        }

        private float _timeFromStart = 0;
        private float _spawnTimer = 0;
        private void Update()
        {
            //Update Timers
            _spawnTimer += Time.deltaTime;
            _timeFromStart += Time.deltaTime;
            
            //Update complexity
            float t = Mathf.Clamp01(_timeFromStart / timeToHighComplexity);
            _enemySpeed = Mathf.Lerp(startEnemySpeed, hightestEnemySpeed, t);
                
            
            if (_spawnTimer >= spawnTimestep)
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
                enemy.GetComponent<IEnemy>().Instantiate(initSide, _enemySpeed);
                
                _spawnTimer = 0;
            }
        }

        private void RescaleGame()
        {
            Vector2 backgroundScale = ScreenScaler.ScaleToFillScreen(background.GetComponent<SpriteRenderer>());
            background.transform.localScale = backgroundScale;
            
            _gameScale =
                ScreenScaler.ScaleToFillPartOfScreen(
                    playerEntity.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);
            
            playerEntity.transform.localScale = Vector3.one * _gameScale;
            enemyPrefab.transform.localScale = Vector3.one * _gameScale;
            shitPrefab.transform.localScale = Vector3.one * _gameScale;

        }
    }

}