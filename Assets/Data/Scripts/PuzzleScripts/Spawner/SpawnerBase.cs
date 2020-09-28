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
        [Header("Coins params")]
        [SerializeField] private bool spawnCoins = true;
        
        [Range(0,1)]
        [SerializeField] private float coinProbability = 0.2f;

        [SerializeField] private int costOfEnemy = 1;
        
        [Header("Enemy params")]
        [SerializeField] protected GameObject[] enemyPrefab;
        
        [Tooltip("The percent which player's puzzle will take on the any screen")]
        [SerializeField] protected float partOfThePLayerOnTheScreen = 0.25f;

        private GameObject m_PlayerEntity;
        private PlayerView m_PlayerView;
        private Player m_Player;

        public bool SpawnCoins => spawnCoins;
        public float CoinProbability => coinProbability;
        public int CostOfEnemy => costOfEnemy;

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
                    m_PlayerView.shape.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);

            m_PlayerEntity.transform.localScale = Vector3.one * playerScale;

            foreach (var prefab in enemyPrefab)
            {
                float enemyScale = ScreenScaler.ScaleToFillPartOfScreen(
                    prefab.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);
                
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

            if (Random.Range(0.0f, 1.0f) < coinProbability)
                enemy.SetCoinHolder(1);

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