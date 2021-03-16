using Puzzle;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New PuzzleLevelConfig", menuName = "Puzzle/CreatePuzzleConfig", order = 51)]
public partial class LevelConfig : ScriptableObject
{
    [SerializeField] string m_LevelName;
    [SerializeField] string m_SceneID;
    [SerializeField] GameObject m_LevelRootPrefab;
    [SerializeField] LevelColorScheme m_LevelColorScheme;

    [Space(10)]
    [SerializeField] PuzzleSides m_PuzzleSides;
    
    [Space(10)]
    [SerializeField] bool m_CollectionEnabled = false;
    [SerializeField] StarView m_StarView;

    [Space(10)]
    [SerializeField] TimelineAsset EasyTimeline;
    [SerializeField] TimelineAsset MediumTimeline;
    [SerializeField] TimelineAsset HardTimeline;
    
    public string Name => m_LevelName;
    public string SceneID => m_SceneID;
    public GameObject LevelRootPrefab => m_LevelRootPrefab;
    public LevelColorScheme ColorScheme => m_LevelColorScheme;
    public bool CollectionEnabled => m_CollectionEnabled;
    public bool StarsEnabled => StarView != null;
    public StarView StarView => m_StarView;
    
    public PuzzleSides PuzzleSides => m_PuzzleSides;

    public TimelineAsset GetTimeline(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                return EasyTimeline;
            case DifficultyLevel.Medium:
                return MediumTimeline;
            case DifficultyLevel.Hard:
                return HardTimeline;
        }

        return null;
    }

    public bool SupportsDifficultyLevel(DifficultyLevel difficulty)
    {
        return GetTimeline(difficulty) != null;
    }
}

public enum DifficultyLevel
{
    Invalid = 0,
    Easy = 1,
    Medium = 2,
    Hard = 3
}
