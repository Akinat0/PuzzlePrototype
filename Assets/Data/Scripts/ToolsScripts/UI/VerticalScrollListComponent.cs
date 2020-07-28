using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{

    public interface IListElement
    {
        Transform Create();
        Vector2 Size { get; }
    }
    
    public abstract class VerticalScrollListComponent<T> : ListBaseComponent<T> where T : IListElement
    {
        [SerializeField] protected RectTransform Content;
        [SerializeField] protected ScrollRect ScrollRect;
        [SerializeField] protected LayoutGroup layout;
        
        protected LayoutGroup Layout => layout;


        protected virtual void Start()
        {
            CreateList();
        }

        protected virtual void CreateList()
        {
            Content.offsetMin += new Vector2(0, Layout.padding.bottom + Layout.padding.top);
            
            foreach (T item in Selection)
                AddElement(item);
        }
        
        protected virtual void AddElement(IListElement listElement)
        {
            listElement.Create().SetParent(Layout.transform);
            Content.offsetMin -= new Vector2(0, listElement.Size.y);
        }
        
    }
}

