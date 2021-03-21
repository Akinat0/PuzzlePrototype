using System;
using System.Linq;
using Abu.Tools.UI;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionScrollList : VerticalScrollListComponent<CollectionListView>
    {
        const float Delay = 0.02f; 
        
        protected override void CreateList()
        {
            Selection = Account.CollectionItems.Select(item => new CollectionListView(item)).ToArray();
            base.CreateList();
        }

        protected override void AddElement(CollectionListView listElement)
        {
            listElement.Create(Layout.transform);
            
            if(Elements.Count % 3 != 1)
                return;

            FlexibleLayoutGroup layoutGroup = (FlexibleLayoutGroup) Layout;
            Content.offsetMin -= new Vector2(0, listElement.Size.y + layoutGroup.spacing.y);
        }

        public void Show(Action finished = null)
        {
            int count = Elements.Count;

            if (count == 0)
            {
                finished?.Invoke();
                return;
            }

            void Finished()
            {
                count--;
                
                if(count > 0)
                    return;
                
                finished?.Invoke();
            }
            
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].Show(Delay * i, Finished);
        }

        public void Hide(Action finished = null)
        {
            int count = Elements.Count;
            
            if (count == 0)
            {
                finished?.Invoke();
                return;
            }
            
            void Finished()
            {
                count--;
                
                if(count > 0)
                    return;
                
                finished?.Invoke();
            }
            
            foreach (CollectionListView view in Elements)
                view.Hide(0, Finished);
        }
    }
}