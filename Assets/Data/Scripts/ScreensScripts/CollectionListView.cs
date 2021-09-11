using System;
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

        public Vector2 Size => entity.RectTransform.rect.size;
        public Vector3 Position => entity.RectTransform.position;

        public CollectionViewComponent View => entity;
        public CollectionItem CollectionItem => collectionItem;

        CollectionViewComponent entity;

        public void LinkToList(Transform container)
        {
            entity = CollectionViewComponent.Create(container, collectionItem);

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