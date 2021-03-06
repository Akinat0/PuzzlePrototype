using System;
using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionListView : IListElement
    {
        public CollectionListView(CollectionItem collectionItem)
        {
            this.collectionItem = collectionItem;
        }

        readonly CollectionItem collectionItem;

        public Vector2 Size => entity.RectTransform.rect.size;
        public Vector3 Position => entity.RectTransform.position;

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
            entity.ScaleComponent.Phase = 0;

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

            entity.TextField.Color = textFieldColor;

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
        
        public void Show(float delay, Action finished = null)
        {
            entity.SetActive(true);
            entity.Invoke(() => entity.ShowComponent(0.23f, finished), delay);
        }

        public void Hide(float delay, Action finished = null)
        {
            finished += () => entity.SetActive(false);
            entity.Invoke(() => entity.HideComponent(0.23f, finished), delay);
        }
        
    }
}