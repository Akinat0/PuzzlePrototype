using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionViewComponent : TextButtonComponent
    {
        public static CollectionViewComponent Create(Transform container, CollectionItem item)
        {
            CollectionViewComponent view = Instantiate(Resources.Load<CollectionViewComponent>("UI/CollectionView"), container);
            view.Create(item);
            return view;
        }
        
        public UIPuzzleView PuzzleView { get; private set; }

        void Create(CollectionItem collectionItem)
        {
            PuzzleView = UIPuzzleView.Create(collectionItem.ID, Content);
            Text = collectionItem.Name;
            name = $"{collectionItem.Name} puzzle";
            ScaleComponent.Phase = 0;
            
            TextField.Color = collectionItem.Rarity.GetColor();
        }
    }
}