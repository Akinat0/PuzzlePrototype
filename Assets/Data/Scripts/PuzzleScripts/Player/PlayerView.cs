using Abu.Tools;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    [SerializeField] private float _partOfScreen = 0.25f;
    [SerializeField] public Transform shape;

    [Space(10), SerializeField, Tooltip("Top, Right, Bottom and Left transforms respectively")] 
    private Transform[] TRBL_positions;

    protected Animator Animator;
    
    private static readonly int Damaged = Animator.StringToHash("Damaged");
    private static readonly int Kill = Animator.StringToHash("Kill");

    public float PartOfScreen => _partOfScreen;

    public Transform[] TRBLPositions => TRBL_positions;

    Quaternion defaultShapeRotation;

    protected virtual void Start()
    {
        Animator = GetComponent<Animator>();
        SetScale(_partOfScreen);
        defaultShapeRotation = shape.rotation;
    }
    
    void SetScale(float partOfScreen)
    {
        transform.localScale = Vector3.one * 
            ScreenScaler.ScaleToFillPartOfScreen(
                shape.gameObject.GetComponent<SpriteRenderer>(),
                partOfScreen);
    }

    public Vector3 GetSidePosition(Side _Side)
    {
        switch (_Side)
        {
            case Side.Up:
                return TRBL_positions[0].position;
            case Side.Right:
                return TRBL_positions[1].position;
            case Side.Down:
                return TRBL_positions[2].position;
            case Side.Left:
                return TRBL_positions[3].position;
            default:
                Debug.LogError("_Side is unreadable");
                return Vector3.one;
        }
    }
    
    public virtual void ChangeSides()
    {
        shape.Rotate(0, 0, 180);
    }

    protected virtual void RestoreSides()
    {
        shape.rotation = defaultShapeRotation;
    }

    protected virtual void OnPlayerLoseHp()
    {
        Animator.SetTrigger(Damaged);
    }

    protected virtual void OnEnemyDied(EnemyBase enemy)
    {
        Animator.SetTrigger(Kill);
    }
    
    protected virtual void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        GameSceneManager.EnemyDiedEvent += OnEnemyDiedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
        GameSceneManager.EnemyDiedEvent -= OnEnemyDiedEvent_Handler;
    }

    void PlayerLosedHpEvent_Handler()
    {
        OnPlayerLoseHp();
    }
    
    void OnEnemyDiedEvent_Handler(EnemyBase enemy)
    {
        OnEnemyDied(enemy);
    }

    
    void LevelClosedEvent_Handler()
    {
        RestoreSides();
    }
}
