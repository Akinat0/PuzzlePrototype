using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class TextGroupComponent : UIComponent
    {
        public static TextGroupComponent AttachTo([NotNull] GameObject holder)
        {
            TextGroupComponent textGroupComponent = holder.AddComponent<TextGroupComponent>();
            
            return textGroupComponent;
        }
        
        [SerializeField] TextObject[] TextObjects = Array.Empty<TextObject>();

        [SerializeField] float FontSize;

        bool IsTextDirty = false;
        
        void Start()
        {
            if (TextObjects == null || TextObjects.Length <= 0)
                return;
            
            IsTextDirty = true;
        }

        void LateUpdate()
        {
            if (!IsTextDirty)
                return;

            FontSize = ResolveTextSize(TextObjects);
            IsTextDirty = false;
        }
        
        public void Add(TextObject textObject)
        {
            List<TextObject> objectsList = TextObjects.ToList();
            objectsList.Add(textObject);
            TextObjects = objectsList.ToArray();
            
            IsTextDirty = true;
        }

        public void Remove(TextObject textObject)
        {
            List<TextObject> objectsList = TextObjects.ToList();
            objectsList.Remove(textObject);
            TextObjects = objectsList.ToArray();
            
            IsTextDirty = true;
        }

        [ContextMenu("UpdateText")]
        public void UpdateText()
        {
            IsTextDirty = true;
        }
        
        public static float ResolveTextSize(TextObject[] textObjects)
        {
            if (textObjects.Length == 0)
                return 0;
            
            //Keep in mind that TMP should be inited before resolve text size
            float sharedFontSize = float.MaxValue;
            
            foreach (var textObject in textObjects)
            {
                if(textObject == null || textObject.Target == null)
                    continue;

                if (!textObject.IsSizeSource)
                    continue;
                
                var text = textObject.Target;
                
                text.enableAutoSizing = true;
                text.ForceMeshUpdate();
                sharedFontSize = Mathf.Min(text.fontSize, sharedFontSize);
                text.enableAutoSizing = false;
            }

            foreach (var textObject in textObjects)
            {
                if(textObject == null || textObject.Target == null || !textObject.IsSizeTarget)
                    continue;
                
                textObject.Target.fontSize = sharedFontSize;

                if (textObject.UpdateOnce)
                    textObject.IsSizeTarget = false;


            }

            return sharedFontSize;
        }
    }

    public class TextObject
    {
        public readonly TextMeshProUGUI Target;

        public bool IsSizeTarget;
        public bool IsSizeSource;
        public bool UpdateOnce;

        public TextObject(TextMeshProUGUI target, bool isSizeTarget = true, bool isSizeSource = true, bool updateOnce = false)
        {
            Target = target;
            IsSizeTarget = isSizeTarget;
            IsSizeSource = isSizeSource;
            UpdateOnce = updateOnce;
        }

    }
}