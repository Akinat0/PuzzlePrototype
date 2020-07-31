using Abu.Tools.UI;
using ScreensScripts;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionListView : IListElement
    {
        public CollectionListView(CollectionItem collectionItem)
        {
            this.collectionItem = collectionItem;
        }

        readonly CollectionItem collectionItem;

        public Vector2 Size => prefab.RectTransform.rect.size;
        
        CollectionViewComponent entity;

        const string pathToPrefab = "UI/CollectionView";

        static CollectionViewComponent prefab;
        
        static CollectionViewComponent Prefab
        {
            get
            {
                if (prefab == null)
                    prefab = Resources.Load<CollectionViewComponent>(pathToPrefab);

                return prefab;
            }        
        }
        
        public void Create(Transform container)
        {
            entity = Object.Instantiate(Prefab, container);
            entity.PuzzleID = collectionItem.ID; 
            entity.Text = collectionItem.Name;
            entity.name = collectionItem.Name + " puzzle";

//            entity.Text = achievement.Name;

//            entity.DescriptionText = achievement.Description;
//            entity.CreateReward(achievement.Reward);
//            entity.Icon = achievement.Icon;
//            
//            UpdateView();

//            entity.OnClick += () =>
//            {
//                if (achievement.State == Achievement.AchievementState.Received)
//                    achievement.Claim();
//            };

        }
        
        static Sprite GetSprite(CollectionItem item)
        { 
            return Resources.Load<Sprite>("Achievements/DefaultAchievement");
        }
        
        
    }
}