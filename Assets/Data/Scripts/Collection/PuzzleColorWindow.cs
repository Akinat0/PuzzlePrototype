
using System;
using TMPro;
using UnityEngine;

public class PuzzleColorWindow : CancelableWindow
{
    public static PuzzleColorWindow Create(int PuzzleID, PuzzleColorData colorData, Action onSuccess = null)
    {
        CancelableWindow prefab = Resources.Load<CancelableWindow>("UI/CancelableWindow");
        CancelableWindow cancelableWindow = Instantiate(prefab, Root);

        PuzzleColorWindow window = ConvertTo<PuzzleColorWindow>(cancelableWindow);

        window.Initialize(PuzzleID, colorData, onSuccess);
        
        return window;
    }
    
    protected void Initialize(int PuzzleID, PuzzleColorData colorData, Action onSuccess)
    {
        void CreateContent(RectTransform container)
        {
            var text = Instantiate(Resources.Load<TextMeshProUGUI>("UI/CommonText"), container);
            text.text = $"Unlock color {colorData.ID}";
        }

        void OnSuccess()
        {
            Account.UnlockCollectionItemColor(PuzzleID, colorData.ID);
            onSuccess?.Invoke();
            Hide();
        }

        void OnCancel()
        {
            Hide();
        }
        
        Initialize(CreateContent, OnSuccess, OnCancel, "Unlock color", "Unlock", "Cancel");
    }
}
