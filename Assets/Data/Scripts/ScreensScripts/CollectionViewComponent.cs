using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionViewComponent : TextButtonComponent
    {
        UIPuzzleView puzzleView;
        
        public int PuzzleID
        {
            set => puzzleView = UIPuzzleView.Create(value, RectTransform);
        }
    }
}