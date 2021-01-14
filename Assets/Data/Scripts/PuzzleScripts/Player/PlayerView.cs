using System;
using Abu.Tools;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public static PlayerView Create(Transform parent, int ID, PuzzleSides? sides = null)
    {
        return Create(parent, Account.GetCollectionItem(ID), sides);
    }
    
    public static PlayerView Create(Transform parent, CollectionItem collectionItem, PuzzleSides? sides = null)
    {
        GameObject puzzleVariant = sides != null 
            ? collectionItem.GetPuzzleVariant(sides.Value) 
            : collectionItem.GetAnyPuzzleVariant();

        if (puzzleVariant == null)
            return null;
        
        PlayerView playerView = Instantiate(puzzleVariant, parent).GetComponent<PlayerView>();
        playerView.ID = collectionItem.ID;
        return playerView;
    }
    
    [SerializeField] private float _partOfScreen = 0.25f;
    [SerializeField] public Transform shape;
    [SerializeField] PlayerViewColorSkin[] colorSkins;

    [Space(10), SerializeField, Tooltip("Top, Right, Bottom and Left transforms respectively")] 
    private Transform[] TRBL_positions;

    protected int ID;
    protected CollectionItem CollectionItem;
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

        CollectionItem = Account.GetCollectionItem(ID);
        
        if (CollectionItem != null)
        {
            CollectionItem.OnActiveColorChangedEvent += OnActiveColorChanged_Handler;
            UpdateColor();
        }
    }

    void OnDestroy()
    {
        if (CollectionItem != null)
            CollectionItem.OnActiveColorChangedEvent -= OnActiveColorChanged_Handler;
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

    protected virtual void RestoreView()
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

    void UpdateColor()
    {
        PuzzleColorData? puzzleColor = CollectionItem.ActiveColorData;
        
        if(puzzleColor == null)
            return;
        
        foreach (PlayerViewColorSkin viewColor in colorSkins)
            viewColor.ChangePuzzleSkin(puzzleColor.Value);
    }

    void OnActiveColorChanged_Handler(int colorIndex)
    {
        UpdateColor();
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
        RestoreView();
    }
    
    
    #if UNITY_EDITOR
    
    public static void SetEditorColorSkins(PlayerView playerView, PlayerViewColorSkin[] colorSkins)
    {
        playerView.colorSkins = colorSkins;
    }

    public static void SetEditorTRBL(PlayerView playerView, Transform[] trbl)
    {
        playerView.TRBL_positions = trbl;
    }
    
    #endif
}
