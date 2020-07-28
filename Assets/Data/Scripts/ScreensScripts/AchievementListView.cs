using System;
using System.Net.Mime;
using Abu.Tools.UI;
using Puzzle;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementListView : IListElement
    {
        public AchievementListView(Achievement achievement)
        {
            this.achievement = achievement;
        }

        readonly Achievement achievement;
        
        public Vector2 Size => entity.RectTransform.rect.size;
        
        AchievementViewComponent entity;

        const string pathToPrefab = "UI/AchievementView";

        static AchievementViewComponent prefab;
        
        static AchievementViewComponent Prefab
        {
            get
            {
                if (prefab == null)
                    prefab = Resources.Load<AchievementViewComponent>(pathToPrefab);

                return prefab;
            }        
        }

        public Transform Create()
        {
            entity = Object.Instantiate(Prefab);
            entity.name = achievement.Name + " achievement";
            entity.Text = achievement.Name;
            entity.SetupProgress(achievement.Progress, 0, achievement.TargetProgress);

            return entity.transform;
        }
        
    }
}