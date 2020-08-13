using TMPro;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class TextGroupComponent : UIComponent
    {
        [SerializeField] TextMeshProUGUI[] TextObjects;

        bool IsTextSizeDirty = false;
        
        readonly TextGroup textGroup = new TextGroup();
        
        void Awake()
        {
            IsTextSizeDirty = true;
            textGroup.AddRange(TextObjects);
        }

        void OnWillRenderObject()
        {
            if (!IsTextSizeDirty)
                return;
            
            textGroup.ResolveTextSize();
            IsTextSizeDirty = false;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            textGroup.RemoveAll();
            textGroup.AddRange(TextObjects);
            textGroup.ResolveTextSize();
        }
    }
}