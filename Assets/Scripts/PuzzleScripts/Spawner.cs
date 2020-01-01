using System;
using System.Collections.Generic;
using Abu.Tools;
using UnityEditor;
using UnityEngine;

namespace Puzzle
{
    public enum Side {Left =
        0, Up = 1, Right = 2, Down = 3 }
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float spawnTimestep = 1;
        [SerializeField] private float startEnemySpeed = 0.02f;
        [SerializeField] private float hightestEnemySpeed = 0.1f;
        [SerializeField] private float timeToHighComplexity = 60f;

        [Tooltip("The percent which player's pazzle will take on the any screen")]
        [SerializeField] private float partOfThePLayerOnTheScreen = 0.25f;

        
        [NonSerialized] public GameObject playerEntity;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject shitPrefab;

        [SerializeField] private GameObject background;
        
        private float _enemySpeed;
        public bool _spawn = false;

        private float _timeFromStart = 0;
        private float _spawnTimer = 0;
        
        private void Update()
        {
            if (!_spawn)
                return;
                
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
            
            float playerScale =
                ScreenScaler.ScaleToFillPartOfScreen(
                    playerEntity.GetComponent<PlayerView>().shape.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);
            
            float enemyScale = ScreenScaler.ScaleToFillPartOfScreen(
                enemyPrefab.GetComponent<SpriteRenderer>(),
                partOfThePLayerOnTheScreen);
            
            
            playerEntity.transform.localScale = Vector3.one * playerScale;
            enemyPrefab.transform.localScale = Vector3.one * enemyScale;
            shitPrefab.transform.localScale = Vector3.one * enemyScale;

        }

        private void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
            GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        }

        private void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
            GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            _enemySpeed = startEnemySpeed;
            _timeFromStart = 0;
            _spawnTimer = 0;
        }

        void PauseLevelEvent_Handler(bool pause)
        {
            _spawn = !pause;
        }

        void GameStartedEvent_Handler()
        {
            RescaleGame();
        }
    }

}