using System;
using Puzzle;
using UnityEngine;

namespace PuzzleScripts
{
    public enum EnemyType
    {
        Puzzle, 
        Shit,
        Virus
    }

    public class EnemyBase : MonoBehaviour, IEnemy
    {
        public static readonly float Distance = 10f; //Distance to target

        [SerializeField] private GameObject vfx;
        [SerializeField] private int score;
        [SerializeField] private EnemyType type;
        
        private float _speed = 0.5f;
    
        private int _damage = 1;

        private bool _motion = true;

        private float time = 0;
        private float dist = 0;
        
        private void Update()
        {
            if (!_motion)
                return;
            Move();
        }
        public EnemyType Type => type;

        public int Damage
        {
            get { return _damage; }
            set { _damage = value;}
        }

        public virtual void OnHitPlayer(Player player)
        {
            player.DealDamage(Damage);
            if(gameObject != null)
                Destroy(gameObject);
        }

        public virtual Transform Die()
        {
            GameObject effect = Instantiate(vfx, GameSceneManager.Instance.GameSceneRoot);
            effect.transform.right = transform.right;
            effect.transform.position = transform.position;
            effect.transform.localScale *= transform.localScale.x;
            GameSceneManager.Instance.InvokeEnemyDied(score);

            CoinHolder coinHolder = GetComponent<CoinHolder>();
            if (coinHolder != null)
               Account.AddCoins(coinHolder.Coins); 
            
            Destroy(gameObject);

            return effect.transform;
        }
        
        public void Move()
        {
            transform.Translate(new Vector3(_speed * Time.deltaTime, 0), Space.Self);
        }

        public virtual void Instantiate(EnemyParams @params)
        {
            _speed =  @params.speed;
            
            Player player = GameSceneManager.Instance.GetPlayer();
            PlayerView playerView = player.GetComponent<PlayerView>();
            
            if(playerView == null)
            {
                Debug.LogError("PlayerView is missing");
                return;
            }
            
            Vector3 target = playerView.GetSidePosition(@params.side);
            
            switch (@params.side)
            {
                case Side.Right:
                    transform.position = target + Vector3.right * Distance;
                    transform.right = Vector3.left;
                    break;
                case Side.Left:
                    transform.position = target + Vector3.left * Distance;
                    transform.right = Vector3.right;
                    break;
                case Side.Up:
                    transform.position = target + Vector3.up * Distance;
                    transform.right = Vector3.down;
                    break;
                case Side.Down:
                    transform.position = target + Vector3.down * Distance;
                    transform.right = Vector3.up;
                    break;
            }
        }
    
        private void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        }

        private void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            Destroy(gameObject);
        }
        
        void PauseLevelEvent_Handler(bool paused)
        {
            _motion = !paused;
        }
    }

    [Serializable]
    public struct EnemyParams
    {
        public EnemyType enemyType;
        public Side side;
        public float speed;
        public bool stickOut;
        [Range(0, 359)] public float radialPosition;
    }
}