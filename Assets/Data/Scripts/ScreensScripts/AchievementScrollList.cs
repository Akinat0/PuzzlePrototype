using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementScrollList : VerticalScrollListComponent<AchievementData>
    {
        //TODO move to account, when achievement system will be done
        [SerializeField] AchievementData[] Achievements;

        protected override AchievementData[] Selection => Achievements;
        
        protected override void AddElement(IListElement listElement)
        {
            base.AddElement(listElement);
            Content.offsetMin -= new Vector2(0, ((VerticalLayoutGroup) Layout).spacing);
        }
    }
}