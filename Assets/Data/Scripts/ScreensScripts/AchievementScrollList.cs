using System.Linq;
using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementListView>
    {
        TextGroupComponent titleTextGroup;
        TextGroupComponent descriptionTextGroup;

        protected override void Start()
        {
            titleTextGroup = TextGroupComponent.AttachTo(gameObject);
            descriptionTextGroup = TextGroupComponent.AttachTo(gameObject);
            
            Selection = Account.Achievements.Select(A => new AchievementListView(A, titleTextGroup, descriptionTextGroup)).ToArray();
            base.Start();
        }

        protected override void AddElement(AchievementListView listElement)
        {
            base.AddElement(listElement);
            Content.offsetMin -= new Vector2(0, ((VerticalLayoutGroup) Layout).spacing);
        }

        
    }
}