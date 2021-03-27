using System;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementsScreen : ScreenComponent
    {
        [SerializeField] AchievementScrollList achievements;

        public override void CreateContent()
        {
            base.CreateContent();
            achievements.InitializeList();
        }

        public override bool Show(Action finished = null)
        {
            if (!base.Show(finished))
                return false;
            
            SetActive(true);
            achievements.Show();
            return true;
        }

        public override bool Hide(Action finished = null)
        {
            if (!base.Hide(finished))
                return false;
            
            achievements.Hide(() => SetActive(false));
            return true;
        }
    }
}