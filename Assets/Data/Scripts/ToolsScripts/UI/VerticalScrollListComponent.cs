using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{

    public interface IListElement
    {
        void Create(Transform container);
        Vector2 Size { get; }
        Vector3 Position { get; }
    }
    
    public abstract class VerticalScrollListComponent<T> : ListBaseComponent<T> where T : IListElement
    {
        [SerializeField] protected RectTransform Content;
        [SerializeField] protected ScrollRect ScrollRect;
        [SerializeField] protected LayoutGroup layout;
        
        protected LayoutGroup Layout => layout;

        protected readonly List<T> Elements = new List<T>();

        bool isListInitialized = false;
        
        void Awake()
        {
            InitializeList();
        }

        public void InitializeList()
        {
            if(isListInitialized)
                return;

            isListInitialized = true; 
            CreateList();
        }
        
        protected virtual void CreateList()
        {
            Content.offsetMin += new Vector2(0, Layout.padding.bottom + Layout.padding.top);

            foreach (T item in Selection)
            {
                Elements.Add(item);
                AddElement(item);
            }
        }
        
        protected virtual void AddElement(T listElement)
        {
            listElement.Create(Layout.transform);
            Content.offsetMin -= new Vector2(0, listElement.Size.y);
        }

        public void SnapTo(Predicate<T> predicate)
        {
            int index = Elements.FindIndex(predicate);
            
            if (index < 0)
                return;

            T target = Elements[index];
            
            Canvas.ForceUpdateCanvases();

            Vector2 contentPanelPosition = RectTransform.InverseTransformPoint(Content.position);
            Vector2 targetPosition = RectTransform.InverseTransformPoint(target.Position);
            
            Content.anchoredPosition = contentPanelPosition - targetPosition;
        }
    }
}

