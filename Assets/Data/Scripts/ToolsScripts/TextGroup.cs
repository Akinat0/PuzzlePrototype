using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Abu.Tools
{
    public class TextGroup
    {
        readonly List<TextMeshProUGUI> TextObjects = new List<TextMeshProUGUI>();

        public TextGroup() { }

        public void Add(TextMeshProUGUI text)
        {
            TextObjects.Add(text);
        }
        
        public void Remove(TextMeshProUGUI text)
        {
            TextObjects.Remove(text);
        }
        
        public void ResolveTextSize()
        {
            //Keep in mind that TMP should be inited before resolve text size
            float sharedFontSize = float.MaxValue;
            
            foreach (var text in TextObjects)
            {
                if(text == null)
                    continue;
                
                text.enableAutoSizing = true;
                text.ForceMeshUpdate();
                sharedFontSize = Mathf.Min(text.fontSize, sharedFontSize);
                text.enableAutoSizing = false;
            }

            foreach (var text in TextObjects)
            {
                if(text == null)
                    continue;
                
                text.fontSize = sharedFontSize;
            }
        }
        
    }
}


