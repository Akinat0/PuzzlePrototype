using System.Linq;
using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementListView>
    {
        readonly TextGroup titleGroup = new TextGroup();
        readonly TextGroup descriptionGroup = new TextGroup();
        bool IsDirtyTextSize;
        
        protected override void Start()
        {
            Selection = Account.Achievements.Select(A => new AchievementListView(A, titleGroup, descriptionGroup)).ToArray();
            base.Start();
        }

        protected override void AddElement(AchievementListView listElement)
        {
            base.AddElement(listElement);
            Content.offsetMin -= new Vector2(0, ((VerticalLayoutGroup) Layout).spacing);
            IsDirtyTextSize = true;
        }

        void OnWillRenderObject()
        {
            if (!IsDirtyTextSize)
                return;
            
            titleGroup.ResolveTextSize();
            descriptionGroup.ResolveTextSize();
            IsDirtyTextSize = false;

        }
    }
}