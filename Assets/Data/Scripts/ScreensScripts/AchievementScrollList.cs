using System.Linq;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementView>
    {
        protected override void Start()
        {
            Selection = Account.Achievements.Select(A => new AchievementView(A)).ToArray();
            base.Start();
        }

        protected override void AddElement(IListElement listElement)
        {
            base.AddElement(listElement);
            Content.offsetMin -= new Vector2(0, ((VerticalLayoutGroup) Layout).spacing);
        }
    }
}