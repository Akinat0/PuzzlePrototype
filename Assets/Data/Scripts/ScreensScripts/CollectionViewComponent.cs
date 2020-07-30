using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionViewComponent : TextButtonComponent
    {
        public Sprite Icon
        {
            set => Background.sprite = value;
        }
    }
}