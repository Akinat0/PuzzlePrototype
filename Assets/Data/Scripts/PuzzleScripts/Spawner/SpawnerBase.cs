using System;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using PuzzleScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle
{
    public enum Side {Left = 0, Up = 1, Right = 2, Down = 3 }
    
    public  class SpawnerBase : MonoBehaviour
    {
        [Header("Enemy params")]
        [SerializeField] protected GameObject[] enemyPrefab;

        GameObject m_PlayerEntity;
        PlayerView m_PlayerView;
        Player m_Player;

        int enemiesCount;
        
        public GameObject PlayerEntity
        {
            get => m_PlayerEntity;
            set
            {
                m_PlayerEntity = value;
                m_Player = m_PlayerEntity.GetComponent<Player>();
                m_PlayerView = m_PlayerEntity.GetComponent<PlayerView>();
                if(m_Player == null)
                    Debug.LogError("There's no Player script on player entity");
                if(m_PlayerView == null)
                    Debug.LogError("There's no PlayerView script on player entity");
            }
        }
        
        readonly Dictionary<EnemyType, EnemyBase> Enemies = new Dictionary<EnemyType, EnemyBase>();
        
        void Awake()
        {
            foreach (GameObject prefab in enemyPrefab)
            {
                EnemyBase enemyBase = prefab.GetComponent<EnemyBase>();

                if (!Enemies.ContainsKey(enemyBase.Type))
                    Enemies[enemyBase.Type] = enemyBase;
            }
        }

        protected virtual void RescaleGame()
        {
            
            float playerScale =
                ScreenScaler.ScaleToFillPartOfScreen(
                    m_PlayerView.Background,
                    m_PlayerView.PartOfScreen);

            m_PlayerEntity.transform.localScale = Vector3.one * playerScale;

            foreach (var prefab in enemyPrefab)
            {
                float enemyScale = ScreenScaler.ScaleToFillPartOfScreen(
                    prefab.GetComponent<SpriteRenderer>(),
                    m_PlayerView.PartOfScreen);
                
                prefab.transform.localScale = Vector3.one * enemyScale;
            }
        }
        
        protected EnemyBase CreateEnemy(EnemyParams @params)
        {
            if (!Enemies.ContainsKey(@params.enemyType))
            {
                Debug.LogError("Spawner doesn't have this enemy type in pool", gameObject);
                return null;
            }

            EnemyBase prefabToInstantiate = Enemies[@params.enemyType];
            
            EnemyBase enemy = Instantiate(prefabToInstantiate, GameSceneManager.Instance.GameSceneRoot);
            enemy.Instantiate(@params);

            enemiesCount++;
            enemy.Renderer.sortingOrder = enemiesCount % 20000;
            
            return enemy;
        }

        protected virtual void OnEnable()
        {
            GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        }

        protected virtual void OnDisable()
        {
            GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        }
        

        protected void GameStartedEvent_Handler()
        {
            RescaleGame();
        }
    }
}