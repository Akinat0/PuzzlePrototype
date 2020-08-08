using Puzzle;
using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleLevelConfig", menuName = "Puzzle/CreatePuzzleConfig", order = 51)]
public partial class LevelConfig : ScriptableObject
{
    [SerializeField] private string m_LevelName;
    [SerializeField] private string m_SceneID;
    [SerializeField] private GameObject m_LevelRootPrefab;
    [SerializeField] private LevelColorScheme m_LevelColorScheme;

    [Space(10)] 
    [SerializeField] private PuzzleSides m_PuzzleSides;
    
    [Space(10)]
    [SerializeField] private bool m_CollectionEnabled = false;
    [SerializeField] private bool m_StarsEnabled = true;
    
    public string Name => m_LevelName;
    public string SceneID => m_SceneID;
    public GameObject LevelRootPrefab => m_LevelRootPrefab;
    public LevelColorScheme ColorScheme => m_LevelColorScheme;
    public bool CollectionEnabled => m_CollectionEnabled;
    public bool StarsEnabled => m_StarsEnabled;
    
    
    public PuzzleSides PuzzleSides => m_PuzzleSides;
    
    
}
