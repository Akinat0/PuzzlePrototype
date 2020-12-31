namespace Data.Scripts.ScreensScripts
{
    public class CollectionViewComponent : TextButtonComponent
    {
        public UIPuzzleView PuzzleView { get; protected set; }
        
        public int PuzzleID
        {
            set => PuzzleView = UIPuzzleView.Create(value, RectTransform);
        }
    }
}