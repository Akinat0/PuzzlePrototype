using Abu.Tools.UI;
using UnityEngine;

public class CollectionColorSelector : MonoBehaviour
{
    [SerializeField] ButtonComponent[] colorButtons;

    CollectionItem Current;
    void Awake()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            ButtonComponent button = colorButtons[i];
            int indexClosure = i;
            button.OnClick += () => SelectColor(indexClosure);
        }
    }

    void SelectColor(int colorIndex)
    {
        if (Current == null)
        {
            Debug.LogError("[CollectionSelector] Active item was not initialized");
            return;
        }

        PuzzleColorData puzzleColor = Current.PuzzleColors[colorIndex];

        if (!puzzleColor.IsUnlocked)
        {
            PuzzleColorWindow.Create(Current.ID, puzzleColor, () => SelectColor(colorIndex));
            return;
        }
        
        colorButtons[colorIndex].Icon.SetActive(false);
        Current.ActiveColorIndex = colorIndex;
    }
    
    #region ISelectionProcessor
    
    public void ProcessIndex(int index, CollectionItem[] selection)
    {
        Current = selection[index];
        
        //Don't show colors if puzzle locked
        if (!Current.Unlocked)
        {
            foreach (ButtonComponent button in colorButtons)
                button.SetActive(false);
            
            return;
        }

        PuzzleColorData[] puzzleColors = Current.PuzzleColors;

        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (i < puzzleColors.Length)
            {
                colorButtons[i].SetActive(true);
                colorButtons[i].Color = puzzleColors[i].Color;
                colorButtons[i].Icon.SetActive(!puzzleColors[i].IsUnlocked);
            }
            else
            {
                colorButtons[i].SetActive(false);
            }
        }
    }

    #endregion
}
