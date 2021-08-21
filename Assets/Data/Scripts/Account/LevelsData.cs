using UnityEngine;

[CreateAssetMenu(fileName = "new LevelsData", menuName = "Account/LevelsData", order = 51)]

public class LevelsData : ScriptableObject
{
    [SerializeField, HideInInspector] LevelConfig[] _LevelItems;

    const string Key = "default_level_id";
    int? defaultLevelID;

    public int DefaultLevelID
    {
        get => defaultLevelID ?? (defaultLevelID = PlayerPrefs.GetInt(Key, 0)).Value;
        set
        {
            if(defaultLevelID.HasValue && defaultLevelID == value)
                return;
            
            defaultLevelID = value;
            PlayerPrefs.SetInt(Key, defaultLevelID.Value);
        }
    }

    public LevelConfig DefaultItem => _LevelItems[DefaultLevelID];
    public LevelConfig[] LevelItems => _LevelItems;
    
}
