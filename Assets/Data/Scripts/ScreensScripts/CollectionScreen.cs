using System;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionScreen : ScreenComponent
    {
        [SerializeField] CollectionScrollList collection;
        
        public override void CreateContent()
        {
            base.CreateContent();
            collection.InitializeList();
        }
        
        public override bool Show(Action finished = null)
        {
            if (!base.Show(finished))
                return false;
            
            SetActive(true);
            collection.Show();
            return true;
        }

        public override bool Hide(Action finished = null)
        {
            if (!base.Hide(finished))
                return false;
            
            collection.Hide(() => SetActive(false));
            return true;
        }
    }
}