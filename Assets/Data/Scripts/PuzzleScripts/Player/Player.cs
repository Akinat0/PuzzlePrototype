using UnityEngine;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        private PlayerView _view;

        protected bool _immuneFramesEnabled;
        protected float _immuneTime = 0.2f;

        public bool[] sides = {false, false, true, true}; //It's relative to Side // True means it's stick out

        public PlayerView PlayerView => _view;

        protected virtual void Awake()
        {
            _view = GetComponent<PlayerView>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        public virtual void DealDamage(int damage)
        {
            GameSceneManager.Instance.ShakeCamera();
            if (!_immuneFramesEnabled)
            {
                for (int i = 0; i < damage; i++)
                    GameSceneManager.Instance.InvokePlayerLosedHp();
            }

            _immuneFramesEnabled = true;
            Invoke(nameof(DisableImmune), _immuneTime);
        }

        protected void DisableImmune()
        {
            _immuneFramesEnabled = false;
        }

        public virtual void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            _view.ChangeSides();
        }

        private void OnEnable()
        {
            MobileGameInput.TouchOnTheScreen += TouchOnScreen_Handler;
        }

        private void OnDisable()
        {
            MobileGameInput.TouchOnTheScreen -= TouchOnScreen_Handler;
        }
        
        void TouchOnScreen_Handler(Touch touch)
        {
            ChangeSides();
        }
        
        
    }
    
}