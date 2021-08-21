using System;
using Abu.Tools.UI;
using UnityEngine;

public class PuzzleColorWindow : CancelableWindow
{
    public static PuzzleColorWindow Create(int PuzzleID, PuzzleColorData colorData, Action onSuccess = null)
    {
        CancelableWindow prefab = Resources.Load<CancelableWindow>("UI/CancelableWindow");
        CancelableWindow cancelableWindow = Instantiate(prefab, GetRoot());

        PuzzleColorWindow window = ConvertTo<PuzzleColorWindow>(cancelableWindow);

        window.Initialize(PuzzleID, colorData, onSuccess);
        
        return window;
    }
    
    protected void Initialize(int PuzzleID, PuzzleColorData colorData, Action onSuccess)
    {
        VideoPurchase purchase = new VideoPurchase();
        
        void CreateContent(RectTransform container)
        {
            TextComponent.Create(container, $"Unlock color {colorData.ID}");
        }

        void OnSuccess()
        {
            purchase.Process(() =>
            {
                Account.UnlockCollectionItemColor(PuzzleID, colorData.ID);
                onSuccess?.Invoke();
            });
            Hide();
        }

        void OnCancel()
        {
            Hide();
        }
        
        Initialize(CreateContent, OnSuccess, OnCancel, "Unlock color", "Unlock", "Cancel");
    }
}
