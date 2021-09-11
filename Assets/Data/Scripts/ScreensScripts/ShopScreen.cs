using System;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class ShopScreen : ScreenComponent
    {
        [SerializeField] ShopScrollList shop;

        public override bool Show(Action finished = null)
        {
            if (!base.Show(finished))
                return false;
            
            SetActive(true);
            shop.Show();
            return true;
        }

        public override bool Hide(Action finished = null)
        {
            if (!base.Hide(finished))
                return false;

            shop.Hide(() => SetActive(false));
            
            return true;
        }

        public ShopItem GetShopItem(Tier tier)
        {
            return shop.GetShopItem(tier);
        }
        
        public void FocusOn(Tier tier)
        {
            bool Predicate(ShopItem shopItem) 
                => shopItem.Tier == tier;
            
            shop.SnapTo(Predicate);
        }
        
        public bool IsScrollable
        {
            get => shop.IsScrollable;
            set => shop.IsScrollable = value;
        }
    }
}