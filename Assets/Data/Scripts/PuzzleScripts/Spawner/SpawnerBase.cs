using System;
using System.Linq;
using Abu.Tools;
using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public enum Side {Left = 0, Up = 1, Right = 2, Down = 3 }
    public  class SpawnerBase : MonoBehaviour
    {
        
        [SerializeField] protected GameObject[] enemyPrefab;
        
        [Tooltip("The percent which player's pazzle will take on the any screen")]
        [SerializeField] protected float partOfThePLayerOnTheScreen = 0.25f;
        [SerializeField] protected GameObject background;

        private GameObject m_PlayerEntity;
        private PlayerView m_PlayerView;
        private Player m_Player;
        
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
            Vector2 backgroundScale = ScreenScaler.ScaleToFillScreen(background.GetComponent<SpriteRenderer>());
            background.transform.localScale = backgroundScale;
            
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
            GameObject enemy = Instantiate(prefabToInstantiate, GameSceneManager.Instance.GameSceneRoot);
            enemy.GetComponent<IEnemy>().Instantiate(@params.side, @params.speed);
            
            return enemy.GetComponent<IEnemy>() as EnemyBase;
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