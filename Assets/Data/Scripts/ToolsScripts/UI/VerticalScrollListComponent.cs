using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{

    public interface IListElement
    {
        void LinkToList(Transform container);
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

        bool isListInitialized;
        
        public bool IsScrollable
        {
            get => ScrollRect.enabled;
            set => ScrollRect.enabled = value;
        } 
        
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

        public void SnapTo(Predicate<T> predicate)
        {
            int index = Elements.FindIndex(predicate);
            
            if (index < 0)
                return;

            T target = Elements[index];
            
            Canvas.ForceUpdateCanvases();

            float contentPanelPosition = RectTransform.InverseTransformPoint(Content.position).y;
            float targetPosition = RectTransform.InverseTransformPoint(target.Position).y;

            float elementOffset = 
                contentPanelPosition < targetPosition ? target.Size.y / 2 : - target.Size.y / 2;

            Vector2 anchoredPos = Content.anchoredPosition;
            anchoredPos.y = contentPanelPosition - targetPosition + elementOffset;
            Content.anchoredPosition = anchoredPos;
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
            listElement.LinkToList(Layout.transform);
            Content.offsetMin -= new Vector2(0, listElement.Size.y);
        }
    }
}

