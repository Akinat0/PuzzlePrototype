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
        
        public UIPuzzleView PuzzleView { get; protected set; }

        void Create(CollectionItem collectionItem)
        {
            PuzzleView = UIPuzzleView.Create(collectionItem.ID, Content);
            Text = collectionItem.Name;
            name = $"{collectionItem.Name} puzzle";
            ScaleComponent.Phase = 0;
            
            Color textFieldColor = Color.gray;

            switch (collectionItem.Rarity)
            {
                case Rarity.Common:
                    textFieldColor = new Color(0.679f, 0.679f, 0.679f);
                    break;
                case Rarity.Rare:
                    textFieldColor = new Color(0.287f, 0.843f, 1f);
                    break;
                case Rarity.Epic:
                    textFieldColor = new Color(0.988f, 0.485f, 1f);
                    break;
            }

            TextField.Color = textFieldColor;
        }
    }
}