using System;
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
            GameObject prefabToInstantiate = 
                enemyPrefab.FirstOrDefault(_P => _P.GetComponent<EnemyBase>().Type == @params.enemyType);
            GameObject enemyGameObject = Instantiate(prefabToInstantiate, GameSceneManager.Instance.GameSceneRoot);
            IEnemy enemy = enemyGameObject.GetComponent<IEnemy>();
            enemy.Instantiate(@params);

            if (Random.Range(0.0f, 1.0f) < coinProbability)
                enemy.SetCoinHolder(1);
            
            return enemyGameObject.GetComponent<IEnemy>() as EnemyBase;
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