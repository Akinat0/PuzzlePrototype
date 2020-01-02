using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public  class SpawnerBase : MonoBehaviour
    {
        [NonSerialized] public GameObject playerEntity;
        
        [SerializeField] protected GameObject[] enemyPrefab;
        //[SerializeField] protected GameObject shitPrefab;
        
        [Tooltip("The percent which player's pazzle will take on the any screen")]
        [SerializeField] protected float partOfThePLayerOnTheScreen = 0.25f;
        [SerializeField] protected GameObject background;
        
        protected virtual void RescaleGame()
        {
            Vector2 backgroundScale = ScreenScaler.ScaleToFillScreen(background.GetComponent<SpriteRenderer>());
            background.transform.localScale = backgroundScale;
            
            float playerScale =
                ScreenScaler.ScaleToFillPartOfScreen(
                    playerEntity.GetComponent<PlayerView>().shape.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);

            playerEntity.transform.localScale = Vector3.one * playerScale;
            
            foreach (var prefab in enemyPrefab)
            {
                float enemyScale = ScreenScaler.ScaleToFillPartOfScreen(
                    prefab.GetComponent<SpriteRenderer>(),
                    partOfThePLayerOnTheScreen);
                
                prefab.transform.localScale = Vector3.one * enemyScale;
            }
        }
        
        protected virtual  void OnEnable()
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


        protected EnemyBase CreateEnemy(EnemyParams @params)
        {
            GameObject prefabToInstantiate = 
                enemyPrefab.FirstOrDefault(_P => _P.GetComponent<EnemyBase>().Type == @params.enemyType);
            GameObject enemy = Instantiate(prefabToInstantiate);
            enemy.GetComponent<IEnemy>().Instantiate(@params.side, @params.speed);
            
            return enemy.GetComponent<IEnemy>() as EnemyBase;
        }
    }
}