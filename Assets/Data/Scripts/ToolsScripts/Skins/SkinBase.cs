using System;
using UnityEngine.UI;
using UnityEngine;

namespace Abu.Tools.Skins
{
    [Serializable]
    public abstract class SkinBase
    {
        public abstract void Apply(SpriteRenderer spriteRenderer);
        public abstract void Apply(Image image);
    }
}