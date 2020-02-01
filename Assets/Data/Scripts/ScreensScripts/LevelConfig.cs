using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleLevelConfig", menuName = "Puzzle/CreatePuzzleConfig", order = 51)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private string m_LevelName;
    [SerializeField] private string m_SceneID;
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_BackgroundPrefab;
    
    public string Name => m_LevelName;
    public string SceneID => m_SceneID;
    public GameObject PlayerPrefab => m_PlayerPrefab;
    public GameObject BackgroundPrefab => m_BackgroundPrefab;
}
