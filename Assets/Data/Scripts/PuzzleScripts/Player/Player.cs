using System.Collections;
using System.Collections.Generic;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        #region attributes
        
        //It's relative to Side // True means it's stick out
        [SerializeField] public bool[] sides = {false, false, true, true};
        
        PlayerCollisionDetector outerCollisionDetector;
        PlayerCollisionDetector innerCollisionDetector;
        
        bool ImmuneFramesEnabled;
        readonly float ImmuneTime = 0.2f;

        Vector3 defaultShapeScale;
        
        IEnumerator currentScaleRoutine;

        readonly List<IEnemy> innerCollisions = new List<IEnemy>();
        readonly List<IEnemy> outerCollisions = new List<IEnemy>();
        
        #endregion

        #region engine
        protected virtual void Awake()
        {
            PlayerView = GetComponent<PlayerView>();
            defaultShapeScale = PlayerView.shape.transform.localScale;

            outerCollisionDetector = PlayerView.OuterCollisionDetector;
            innerCollisionDetector = PlayerView.InnerCollisionDetector;
            outerCollisionDetector.Disable();
        }

        void OnEnable()
        {
            MobileGameInput.TouchOnTheScreen += TouchOnScreen_Handler;
            innerCollisionDetector.OnCollisionDetected += InnerCollisionDetected_Handler;
            outerCollisionDetector.OnCollisionDetected += OuterCollisionDetected_Handler;
        }

        void OnDisable()
        {
            MobileGameInput.TouchOnTheScreen -= TouchOnScreen_Handler;
            innerCollisionDetector.OnCollisionDetected -= InnerCollisionDetected_Handler;
            outerCollisionDetector.OnCollisionDetected -= OuterCollisionDetected_Handler;
        }

        void LateUpdate()
        {
            if (currentScaleRoutine != null)
            {
                foreach (IEnemy enemy in outerCollisions)
                {
                    if (enemy.CanDamagePlayer(this))
                        continue;
                    
                    enemy.OnHitPlayer(this);
                    
                    GameSceneManager.Instance.InvokePerfectKill();
                    
                    int enemyIndexInInnerList = innerCollisions.IndexOf(enemy);

                    if (enemyIndexInInnerList >= 0)
                        innerCollisions.RemoveAt(enemyIndexInInnerList);
                }
            }

            foreach (IEnemy enemy in innerCollisions)
                enemy.OnHitPlayer(this);
            
            innerCollisions.Clear();
            outerCollisions.Clear();
        }

        #endregion
        
        #region public
        
        public PlayerView PlayerView { get; set; }
        
        public virtual void DealDamage(int damage)
        {
            LauncherUI.Instance.MainCamera.Shake();
            if (!ImmuneFramesEnabled)
            {
                for (int i = 0; i < damage; i++)
                    GameSceneManager.Instance.InvokePlayerLosedHp();
            }

            ImmuneFramesEnabled = true;
            Invoke(nameof(DisableImmune), ImmuneTime);
        }

        public virtual void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            PlayerView.ChangeSides();
        }
        
        #endregion
        
        #region private
      
        protected void DisableImmune()
        {
            ImmuneFramesEnabled = false;
        }
        
        IEnumerator ScaleRoutine()
        {
            outerCollisionDetector.Enable();
            
            float duration = 0.25f;
            float time = 0;
            
            float sourceScaleFactor = 1;
            float targetScaleFactor = 2;

            while (time < duration)
            {
                PlayerView.shape.localScale =
                    defaultShapeScale * Mathf.Lerp(sourceScaleFactor, targetScaleFactor,
                        Mathf.PingPong(time, duration / 2));
                
                yield return null;
                
                time += Time.deltaTime;
            }
            
            outerCollisionDetector.Disable();
            currentScaleRoutine = null;
        }
        
        void TouchOnScreen_Handler(Vector3 _)
        {
            if(currentScaleRoutine != null)
                StopCoroutine(currentScaleRoutine);
            
            StartCoroutine(currentScaleRoutine = ScaleRoutine());
            
            ChangeSides();
        }

        void InnerCollisionDetected_Handler(IEnemy enemy)
        {
            innerCollisions.Add(enemy);
        }
        
        void OuterCollisionDetected_Handler(IEnemy enemy)
        {
            outerCollisions.Add(enemy);
        }
        
        #endregion
    }
}