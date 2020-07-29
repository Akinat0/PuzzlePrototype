using Abu.Tools.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementListView : IListElement
    {
        public AchievementListView(Achievement achievement)
        {
            this.achievement = achievement;
            achievement.ProgressChangedEvent += ProgressChangedEvent_Handler;
            achievement.AchievementReceivedEvent += AchievementReceivedEvent_Handler;
            achievement.AchievementClaimedEvent += AchievementClaimedEvent_Handler;
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

        public void Create(Transform container)
        {
            entity = Object.Instantiate(Prefab, container);
            entity.name = achievement.Name + " achievement";
            entity.Text = achievement.Name;
            entity.DescriptionText = achievement.Description;
            entity.CreateReward(achievement.Reward);
            entity.Icon = achievement.Icon;
            
            UpdateView();

            entity.OnClick += () =>
            {
                if (achievement.State == Achievement.AchievementState.Received)
                    achievement.Claim();
            };
        }

        void UpdateView()
        {
            
            entity.SetupProgressBar(achievement.Progress, 0, achievement.Goal);
            entity.Interactable = achievement.State == Achievement.AchievementState.Received;

            Color achievementColor;

            switch (achievement.State)
            {
                case Achievement.AchievementState.InProgress:
                    achievementColor = Color.gray;
                    break;
                case Achievement.AchievementState.Received:
                    achievementColor = Color.green;
                    break;
                case Achievement.AchievementState.Claimed:
                    achievementColor = Color.yellow;
                    break;
                default:
                    achievementColor = Color.magenta;
                    break;
            }

            entity.Color = achievementColor;
            
            
        }
        
        void ProgressChangedEvent_Handler(float progress)
        {
            UpdateView();
        }
        
        void AchievementReceivedEvent_Handler()
        {
            UpdateView();
        }
        
        void AchievementClaimedEvent_Handler()
        {
            UpdateView();
        }
    }
}