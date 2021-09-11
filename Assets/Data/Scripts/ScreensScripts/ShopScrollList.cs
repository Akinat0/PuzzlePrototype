using System;
using System.Linq;
using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class ShopScrollList : VerticalScrollListComponent<ShopItem>
    {
        public ShopItem GetShopItem(Tier tier)
        {
            return Elements.FirstOrDefault(item => item.Tier == tier);
        }
        
        public void Show(Action finished = null)
        {
            int count = Elements.Count;

            DelegateGroup finishedGroup = new DelegateGroup(count, finished);

            const float delay = 0.02f; 
            const float duration = 0.23f; 

            for (int i = 0; i < count; i++)
                Elements[i].Show(i * delay, duration, finishedGroup);
        }
        
        public void Hide(Action finished = null)
        {
            int count = Elements.Count;

            DelegateGroup finishedGroup = new DelegateGroup(count, finished);
            
            foreach (ShopItem shopItem in Elements)
                shopItem.Hide(0, 0.23f, finishedGroup);
        }
        
        protected override void CreateList()
        {
            Selection = Account.GetTiersOfType(Tier.TierType.Shop).Select(ShopItem.Create).ToArray();
            
            GridLayoutGroup layoutGroup = (GridLayoutGroup) Layout;
            RectTransform layoutTransform = (RectTransform) layoutGroup.transform;
            
            //there are 2 elements in row, so spacing should be took into account once
            float cellSize = layoutTransform.rect.width / 2  - layoutGroup.spacing.x;
            
            layoutGroup.cellSize = new Vector2(cellSize, cellSize);
            
            base.CreateList();
        }
        
        protected override void AddElement(ShopItem listElement)
        {
            listElement.LinkToList(Layout.transform);

            if (Elements.Count % 2 != 1) return;
            
            GridLayoutGroup layoutGroup = (GridLayoutGroup) Layout;
            Content.offsetMin -= new Vector2(0, layoutGroup.cellSize.y + layoutGroup.spacing.y);
        }
    }
}