using System.Collections;
using ScreensScripts;
using UnityEngine;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        #region attributes
        
        //It's relative to Side // True means it's stick out
        [SerializeField] public bool[] sides = {false, false, true, true};
        
        PlayerCollisionDetector collisionDetector;

        bool ImmuneFramesEnabled;
        readonly float ImmuneTime = 0.2f;

        Vector3 defaultShapeScale;
        
        IEnumerator currentScaleRoutine;

        #endregion

        #region engine
        protected virtual void Awake()
        {
            PlayerView = GetComponent<PlayerView>();
            defaultShapeScale = PlayerView.Shape.localScale;

            collisionDetector = PlayerView.CollisionDetector;
        }

        void OnEnable()
        {
            MobileGameInput.TouchOnTheScreen += TouchOnScreen_Handler;
            collisionDetector.OnCollisionDetected += CollisionDetected_Handler;
        }

        void OnDisable()
        {
            MobileGameInput.TouchOnTheScreen -= TouchOnScreen_Handler;
            collisionDetector.OnCollisionDetected -= CollisionDetected_Handler;
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
            float duration = 0.25f;
            float time = 0;
            
            float sourceScaleFactor = 1;
            float targetScaleFactor = 2;

            while (time < duration)
            {
                PlayerView.Shape.localScale =
                    defaultShapeScale * Mathf.Lerp(sourceScaleFactor, targetScaleFactor,
                        Mathf.PingPong(time, duration / 2));
                
                yield return null;
                
                time += Time.deltaTime;
            }
            
            currentScaleRoutine = null;
        }
        
        void TouchOnScreen_Handler(Vector3 _)
        {
            if(currentScaleRoutine != null)
                StopCoroutine(currentScaleRoutine);
            
            StartCoroutine(currentScaleRoutine = ScaleRoutine());
            
            ChangeSides();
        }

        void CollisionDetected_Handler(IEnemy enemy)
        {
            if (currentScaleRoutine != null && !enemy.CanDamagePlayer(this))
                GameSceneManager.Instance.InvokePerfectKill();
            
            enemy.OnHitPlayer(this);
        }
        
        #endregion
    }
}