using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleLevelConfig", menuName = "Puzzle/CreatePuzzleConfig", order = 51)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private string m_LevelName;
    [SerializeField] private string m_SceneID;
    [SerializeField] private GameObject m_LevelRootPrefab;
    [SerializeField] private LevelColorScheme m_LevelColorScheme;
    
    public string Name => m_LevelName;
    public string SceneID => m_SceneID;
    public GameObject LevelRootPrefab => m_LevelRootPrefab;
    public LevelColorScheme ColorScheme => m_LevelColorScheme;
}
