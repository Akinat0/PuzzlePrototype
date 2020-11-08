using System;
using Abu.Tools.UI;
using UnityEngine;

public class DifficultyWindow : CancelableWindow
{
    [SerializeField] ButtonComponent EasyButton;
    [SerializeField] ButtonComponent MediumButton;
    [SerializeField] ButtonComponent HardButton;
    
    
    public static DifficultyWindow Create(LevelConfig levelConfig, Action onSuccess = null)
    {
        DifficultyWindow prefab = Resources.Load<DifficultyWindow>("UI/DifficultyWindow");
        DifficultyWindow difficultyWindow = Instantiate(prefab, Root);
        
        difficultyWindow.Initialize(levelConfig, onSuccess);
        
        return difficultyWindow;
    }

    LevelConfig LevelConfig;
    DifficultyLevel SelectedDifficulty;
    
    void Initialize(LevelConfig levelConfig, Action onSuccess)
    {
        LevelConfig = levelConfig;
        SelectedDifficulty = LevelConfig.DifficultyLevel;

        void OnSuccess()
        {
            if (LevelConfig.SupportsDifficultyLevel(SelectedDifficulty))
                LevelConfig.DifficultyLevel = SelectedDifficulty;
            
            onSuccess?.Invoke();
            
            Hide();
        }

        void OnCancel()
        {
            Hide();
        }

        EasyButton.OnClick += () =>
        {
            SelectedDifficulty = DifficultyLevel.Easy;
            ResetButtons();
            EasyButton.RectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        };
        
        MediumButton.OnClick += () =>
        {
            SelectedDifficulty = DifficultyLevel.Medium;
            ResetButtons();
            MediumButton.RectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        };
        
        HardButton.OnClick += () =>
        {
            SelectedDifficulty = DifficultyLevel.Hard;
            ResetButtons();
            HardButton.RectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        };

        EasyButton.Interactable = LevelConfig.SupportsDifficultyLevel(DifficultyLevel.Easy);
        MediumButton.Interactable = LevelConfig.SupportsDifficultyLevel(DifficultyLevel.Medium);
        HardButton.Interactable = LevelConfig.SupportsDifficultyLevel(DifficultyLevel.Hard);
        
        Initialize(null, OnSuccess, OnCancel, "Difficulty", "Select", "Cancel");
    }

    void ResetButtons()
    {
        EasyButton.RectTransform.localScale = Vector3.one;
        MediumButton.RectTransform.localScale = Vector3.one;
        HardButton.RectTransform.localScale = Vector3.one;
    }
    
}
