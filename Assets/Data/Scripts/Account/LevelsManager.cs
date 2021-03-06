﻿using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private LevelsData LevelsData;

    public LevelConfig[] LevelConfigs => LevelsData.LevelItems;

    public int DefaultItemID {
        get => LevelsData.DefaultLevelID;
        set => LevelsData.DefaultLevelID = value;
    }

    public LevelConfig DefaultLevel => LevelsData.DefaultItem;
    
    void Awake()
    {
        LevelsData.LoadSettings();
    }

    void OnApplicationQuit()
    {
        LevelsData.SaveSettings();
    }
}
