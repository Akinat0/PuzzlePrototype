using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionListView : IListElement
    {
        public CollectionListView(CollectionItem collectionItem)
        {
            this.collectionItem = collectionItem;
        }

        readonly CollectionItem collectionItem;

        public Vector2 Size => prefab.RectTransform.rect.size;
        
        CollectionViewComponent entity;

        const string pathToPrefab = "UI/CollectionView";

        static CollectionViewComponent prefab;
        
        static CollectionViewComponent Prefab
        {
            get
            {
                if (prefab == null)
                    prefab = Resources.Load<CollectionViewComponent>(pathToPrefab);

                return prefab;
            }        
        }
        
        public void Create(Transform container)
        {
            entity = Object.Instantiate(Prefab, container);
            entity.PuzzleID = collectionItem.ID; 
            entity.Text = collectionItem.Name;
            entity.name = collectionItem.Name + " puzzle";

            entity.OnClick += () =>
            {
                LauncherUI.Instance.InvokeShowCollection(
                    new ShowCollectionEventArgs(LauncherUI.Instance.LevelConfig.ColorScheme, collectionItem.ID));
            };

            entity.OnHoldDown += () =>
            {
                entity.PuzzleView.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            };
            
            entity.OnHoldUp += () =>
            {
                entity.PuzzleView.transform.localScale = new Vector3(1f, 1f, 1f);
            };

        }
        
    }
}