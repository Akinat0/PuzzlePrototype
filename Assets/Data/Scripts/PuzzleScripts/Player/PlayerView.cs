using System.Collections.Generic;
using System.Linq;
using Abu.Tools;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    #region factory
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
    #endregion
    
    #region serialized
    
    [SerializeField] float _partOfScreen = 0.25f;
    [SerializeField] public Transform shape;
    [SerializeField] PlayerViewSkin[] skins;

    [Space(10), SerializeField, Tooltip("Top, Right, Bottom and Left transforms respectively")] 
    Transform[] TRBL_positions;
    
    [SerializeField] PlayerCollisionDetector outerCollisionDetector;
    [SerializeField] PlayerCollisionDetector innerCollisionDetector;

    #endregion

    #region public 
    
    public Transform[] TRBLPositions => TRBL_positions;
    public float PartOfScreen => _partOfScreen;

    public PlayerCollisionDetector InnerCollisionDetector => innerCollisionDetector;

    public PlayerCollisionDetector OuterCollisionDetector => outerCollisionDetector;

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
                return Vector3.one;
        }
    }
    
    public virtual void ChangeSides()
    {
        shape.Rotate(0, 0, 180);
    }
    
    #endregion
    
    #region private
    
    protected int ID;
    protected CollectionItem CollectionItem;
    protected Animator Animator;
    
    static readonly int Damaged = Animator.StringToHash("Damaged");
    // static readonly int Kill = Animator.StringToHash("Kill");

    
    Quaternion defaultShapeRotation;
    
    void SetScale(float partOfScreen)
    {
        transform.localScale = Vector3.one * 
                               ScreenScaler.ScaleToFillPartOfScreen(
                                   shape.gameObject.GetComponent<SpriteRenderer>(),
                                   partOfScreen);
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
        // Animator.SetTrigger(Kill);
    }
    
    void UpdateColor()
    {
        PuzzleColorData? puzzleColor = CollectionItem.ActiveColorData;
        
        if(puzzleColor == null)
            return;
        
        foreach (PlayerViewSkin skin in skins)
            skin.ChangePuzzleSkin(puzzleColor.Value);
    }
    
    #endregion

    #region engine
    
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
    
    #endregion

    #region event handlers
    
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

    #endregion
    
    #region editor
    #if UNITY_EDITOR
    
    public static void SetEditorColorSkins(PlayerView playerView, PlayerViewColorSkin[] colorSkins)
    {
        if (playerView.skins == null)
            playerView.skins = new PlayerViewSkin[0];
        
        List<PlayerViewSkin> playerViewSkins = playerView.skins.ToList();
        playerViewSkins.AddRange(colorSkins.Cast<PlayerViewSkin>().ToArray());
        playerView.skins = playerViewSkins.ToArray();
    }

    public static void SetEditorTRBL(PlayerView playerView, Transform[] trbl)
    {
        playerView.TRBL_positions = trbl;
    }
    
    #endif
    #endregion
}
