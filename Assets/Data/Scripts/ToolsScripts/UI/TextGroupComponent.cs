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
        public static TextGroupComponent AttachTo([NotNull] GameObject holder, float tolerance = 5f)
        {
            TextGroupComponent textGroupComponent = holder.AddComponent<TextGroupComponent>();
            textGroupComponent.Tolerance = tolerance;
            
            return textGroupComponent;
        }
        
        [SerializeField] TextObject[] TextObjects = Array.Empty<TextObject>();

        [SerializeField] public float FontSize;
        [SerializeField] public float Tolerance;
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

            FontSize = ResolveTextSize(TextObjects, Tolerance);
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

        [ContextMenu("UpdateTextSize")]
        public void UpdateTextSize()
        {
            IsTextDirty = true;
        }
        
        public static float ResolveTextSize(TextObject[] textObjects, float tolerance = 1f)
        {
            if (textObjects.Length == 0)
                return 0;
            
            //Keep in mind that TMP should be inited before resolve text size
            float sharedFontSize = float.MaxValue;
            
            foreach (TextObject textObject in textObjects)
            {
                if(textObject == null || textObject.Target == null)
                    continue;

                if (!textObject.IsSizeSource)
                    continue;
                
                var target = textObject.Target;
                string defaultContent = target.text;
                
                int targetContentIndex = 0;

                for (int i = 0; i < textObject.PossibleContents.Length; i++)
                {
                    if (textObject.PossibleContents[i].Length > textObject.PossibleContents[targetContentIndex].Length)
                        targetContentIndex = i;
                }    
                    
                target.text = textObject.PossibleContents[targetContentIndex];

                target.enableAutoSizing = true;
                target.ForceMeshUpdate();
                sharedFontSize = Mathf.Min(target.fontSize, sharedFontSize);

                target.text = defaultContent;
            }

            sharedFontSize -= tolerance;
            
            foreach (TextObject textObject in textObjects)
            {
                if(textObject == null || textObject.Target == null || !textObject.IsSizeTarget)
                    continue;
                
                textObject.Target.enableAutoSizing = false;   
                
                textObject.Target.fontSize = sharedFontSize;

                if (textObject.UpdateOnce)
                    textObject.IsSizeTarget = false;
                
            }

            return sharedFontSize;
        }
    }

    [Serializable]
    public class TextObject
    {
        public TextMeshProUGUI Target;

        public string[] PossibleContents;

        public bool IsSizeTarget;
        public bool IsSizeSource;
        public bool UpdateOnce;

        public TextObject(TextMeshProUGUI target, string[] possibleContent = null, bool isSizeTarget = true, bool isSizeSource = true, bool updateOnce = false)
        {
            Target = target;
            IsSizeTarget = isSizeTarget;
            IsSizeSource = isSizeSource;
            UpdateOnce = updateOnce;

            PossibleContents = new []{Target.text};

            if (possibleContent != null)
                PossibleContents = PossibleContents.Concat(possibleContent).ToArray();
        }

    }
}