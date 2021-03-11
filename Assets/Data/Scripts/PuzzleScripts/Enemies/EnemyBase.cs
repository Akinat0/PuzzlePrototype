using System;
using Abu.Tools;
using Puzzle;
using UnityEngine;

namespace PuzzleScripts
{
    public enum EnemyType
    {
        Puzzle, 
        Shit,
        Virus,
        LongPuzzle
    }

    [RequireComponent(typeof(Renderer))]
    public class EnemyBase : MonoBehaviour, IEnemy
    {
        public static readonly float Distance = 10f; //Distance to target

        [SerializeField] protected GameObject vfx;
        [SerializeField] protected AudioClip sfx;
        [SerializeField] private int score;
        [SerializeField] private EnemyType type;

        protected EnemyParams _enemyParams;
        
        private float _speed = 0.5f;
    
        private int _damage = 1;

        private bool isAppearedOnScreen;

        protected bool Motion = true;

        private float _time = 0;
        private float _dist = 0;

        new Renderer renderer;
        public virtual Renderer Renderer
        {
            get
            {
                if (renderer == null)
                    renderer = GetComponent<Renderer>();

                return renderer;
            }
        }
        
        new Collider2D collider;
        protected virtual Collider2D Collider
        {
            get
            {
                if (collider == null)
                    collider = GetComponent<Collider2D>();

                return collider;
            }
        }

        protected virtual void Update()
        {
            if (!isAppearedOnScreen)
            {
                if ((GameSceneManager.Instance.Player.transform.position - transform.position).magnitude <
                    ScreenScaler.CameraSize.y / 2)
                {
                    isAppearedOnScreen = true;
                    GameSceneManager.Instance.InvokeEnemyAppearedOnScreen(this);
                }
            }
            
            if (!Motion)
                return;
            
            Move();
            
            
        }
        public EnemyType Type => type;

        public int Score => score;
        
        public int Damage
        {
            get { return _damage; }
            set { _damage = value;}
        }
        public virtual void OnHitPlayer(Player player)
        {
            Haptic.Run(Haptic.Type.FAILURE);
            Destroy(gameObject);
            player.DealDamage(Damage);
        }

        public virtual bool CanDamagePlayer(Player player)
        {
            return Motion;
        }

        public virtual Transform Die()
        {
            GameObject effect = null;
            if (vfx)
            {
                effect = Instantiate(vfx, GameSceneManager.Instance.GameSceneRoot);
                effect.transform.right = transform.right;
                effect.transform.position = transform.position;
                effect.transform.localScale *= transform.localScale.x;
            }

            if(sfx != null)
                SoundManager.Instance.PlayOneShot(sfx);
            
            GameSceneManager.Instance.InvokeEnemyDied(this);

            CoinHolder coinHolder = GetComponent<CoinHolder>();
            if (coinHolder != null)
               Account.AddCoins(coinHolder.Coins); 

            Haptic.Run(Haptic.Type.SUCCESS);
            
            Destroy(gameObject);

            return effect != null ? effect.transform : null;
        }
        
        public void Move()
        {
            transform.Translate(new Vector3(_speed * Time.deltaTime, 0), Space.Self);
        }
        
        

        public virtual void Instantiate(EnemyParams @params)
        {
            _enemyParams = @params;
            _speed =  @params.speed;

            if(@params.sfx != null)
                sfx = @params.sfx;
            
            Player player = GameSceneManager.Instance.Player;
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
    
        public virtual void SetCoinHolder(int CostOfEnemy)
        {
            gameObject.AddComponent<CoinHolder>().SetupCoinHolder(CostOfEnemy);
        }
        
        protected virtual void OnEnable()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        }

        protected virtual void OnDisable()
        {
            GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
            GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        }
        
        void ResetLevelEvent_Handler()
        {
            Destroy(gameObject);
        }
        
        protected virtual void PauseLevelEvent_Handler(bool paused)
        {
            Motion = !paused;
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
        public float trailTime;
     
        public AudioClip sfx;
        [Range(0, 1)] public float volume;
    }
}