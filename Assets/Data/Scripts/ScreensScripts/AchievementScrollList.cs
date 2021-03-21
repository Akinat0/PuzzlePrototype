using System;
using System.Linq;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementListView>
    {
        const float Delay = 0.06f; 
        
        TextGroupComponent titleTextGroup;
        TextGroupComponent descriptionTextGroup;

        protected override void CreateList()
        {
            titleTextGroup = TextGroupComponent.AttachTo(gameObject);
            descriptionTextGroup = TextGroupComponent.AttachTo(gameObject);
            
            Selection = Account.Achievements.Select(a => new AchievementListView(a, titleTextGroup, descriptionTextGroup)).ToArray();
            
            base.CreateList();
        }

        protected override void AddElement(AchievementListView listElement)
        {
            base.AddElement(listElement);
            Content.offsetMin -= new Vector2(0, ((VerticalLayoutGroup) Layout).spacing);
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
                Elements[i].Show(i * Delay, Finished);
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
            
            float totalDelay = Elements.Count * Delay; 
            
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].Hide(totalDelay - i * Delay, Finished);
        }

    }
}