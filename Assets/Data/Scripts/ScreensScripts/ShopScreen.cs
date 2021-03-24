using System;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class ShopScreen : ScreenComponent
    {

        [SerializeField] ShopItem[] shopItems;

        void Awake()
        {
            foreach (ShopItem shopItem in shopItems)
                shopItem.ScaleComponent.Phase = 0;
        }
        
        public override bool Show(Action finished = null)
        {
            if (!base.Show(finished))
                return false;
            
            SetActive(true);
            
            int count = shopItems.Length;
            
            void Finished()
            {
                count--;

                if (count > 0)
                    return;
                
                finished?.Invoke();
            }

            const float delay = 0.02f; 
            const float duration = 0.23f; 

            for (int i = 0; i < shopItems.Length; i++)
                shopItems[i].Show(i * delay, duration, Finished);
            
            return true;
        }

        public override bool Hide(Action finished = null)
        {
            if (!base.Hide(finished))
                return false;

            int count = shopItems.Length;
            
            void Finished()
            {
                count--;

                if (count > 0)
                    return;
                
                finished?.Invoke();
                SetActive(false);
            }
            
            const float duration = 0.23f; 
            
            foreach (ShopItem item in shopItems)
                item.Hide(0, duration, Finished);
            
            return true;
        }
    }
}