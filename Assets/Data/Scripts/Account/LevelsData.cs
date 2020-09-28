using UnityEngine;

[CreateAssetMenu(fileName = "new LevelsData", menuName = "Account/LevelsData", order = 51)]

public class LevelsData : SaveableScriptableObject
{
    [SerializeField] LevelConfig[] _LevelItems;
    [SerializeField] public int DefaultLevelID;
    

    public LevelConfig DefaultItem => _LevelItems[DefaultLevelID];
    public LevelConfig[] LevelItems => _LevelItems;
    
    public override void LoadSettings()
    {
        base.LoadSettings();
        Debug.Log($"Configs was loaded. Default level is {DefaultLevelID}, his name is {DefaultItem.name}");
    }
    
    public override void SaveSettings()
    {
        base.SaveSettings();
        Debug.Log($"Configs was saved. Default level is {DefaultLevelID}, his name is {DefaultItem.name}");
    }
}
