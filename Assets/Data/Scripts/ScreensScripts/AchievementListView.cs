using System;
using Abu.Tools.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementListView : IListElement
    {
        readonly TextGroupComponent TitleTextGroup;
        readonly TextGroupComponent DescriptionTextGroup;

        public AchievementListView(Achievement achievement, TextGroupComponent titleTextGroup, TextGroupComponent descriptionTextGroup)
        {
            this.achievement = achievement;

            TitleTextGroup = titleTextGroup;
            DescriptionTextGroup = descriptionTextGroup;
            
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
            entity.ScaleComponent.Phase = 0;

            TitleTextGroup.Add(new TextObject(entity.TextField.TextMesh, updateOnce: true));
            DescriptionTextGroup.Add(new TextObject(entity.DescriptionField.TextMesh, updateOnce: true));

            UpdateView();

            entity.OnClick += () =>
            {
                if (achievement.State == Achievement.AchievementState.Received)
                    achievement.Claim();
            };
        }

        public void Show(float delay, Action finished = null)
        {
            entity.SetActive(true);
            entity.Invoke(() => entity.ShowComponent(0.23f, finished), delay);
        }

        public void Hide(float delay, Action finished = null)
        {
            finished += () => entity.SetActive(false);
            entity.Invoke(() => entity.HideComponent(0.23f, finished), delay);
        }
        
        void UpdateView()
        {
            
            entity.SetupProgressBar(achievement.Progress, 0, achievement.Goal);
            entity.Interactable = achievement.State == Achievement.AchievementState.Received;

            Color firstColor = Color.white;
            Color secondColor = Color.white;
            
            switch (achievement.State)
            {
                case Achievement.AchievementState.InProgress:
                    firstColor = new Color(0, 0.8f, 0.8f);
                    secondColor = new Color(0, 0.7f, 1);
                    break;
                
                case Achievement.AchievementState.Received:
                case Achievement.AchievementState.Claimed:
                    firstColor = Color.yellow;
                    secondColor = new Color(1, 0.7f, 0.1f);
                    break;
            }

            entity.Color = firstColor;
            entity.AlternativeColor = secondColor;
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