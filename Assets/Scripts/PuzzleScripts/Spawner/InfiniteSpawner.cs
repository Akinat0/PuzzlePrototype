using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleScripts;
using Abu.Tools;
using UnityEditor;
using UnityEngine;

namespace Puzzle
{
    
    public class InfiniteSpawner : SpawnerBase
    {
        [SerializeField] private float spawnTimestep = 1;
        [SerializeField] private float startEnemySpeed = 0.02f;
        [SerializeField] private float hightestEnemySpeed = 0.1f;
        [SerializeField] private float timeToHighComplexity = 60f;

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
                EnemyParams enemyParams = new EnemyParams{speed =  _enemySpeed};
                if (UnityEngine.Random.value > 0.7f)
                {
                    enemyParams.enemyType = EnemyType.Shit;
                }
                else
                {
                    enemyParams.enemyType = EnemyType.Puzzle;
                }

                enemyParams.side = (Side) Mathf.RoundToInt(UnityEngine.Random.value * 3);

                CreateEnemy(enemyParams);
                
                _spawnTimer = 0;
            }
        }
        
        protected override void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
            base.OnDisable();
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

    }

}