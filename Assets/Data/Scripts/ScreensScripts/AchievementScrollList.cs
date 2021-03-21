using System;
using System.Linq;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementListView>
    {
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
            
            const float delay = 0.06f; 
            
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].Show(i * delay, Finished);
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
            
            const float delay = 0.025f; 
            
            float totalDelay = Elements.Count * delay; 
            
            for (int i = 0; i < Elements.Count; i++)
                Elements[i].Hide(totalDelay - i * delay, Finished);
        }

    }
}