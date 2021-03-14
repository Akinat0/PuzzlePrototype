using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class UIColorProvider : MonoBehaviour, IColorReceiver
    {
        [SerializeField] Color[] colors;
        [SerializeField] MonoBehaviour[] receivers;

        IColorReceiver[] internalReceivers;

        public IColorReceiver[] Receivers
        {
            get
            {
                if(!Application.isPlaying || internalReceivers == null)
                    CreateInternalReceivers();
                
                return internalReceivers;
            }
        }

        public Color[] Colors
        {
            get => colors;
            set
            {
                if(colors == value)
                    return;

                colors = value;
                ApplyColor(colors);
            }
        }
        
        public void ApplyColor(params Color[] colors)
        {
            Colors = colors;
            
            foreach (IColorReceiver receiver in Receivers)
                receiver?.ApplyColor(colors);
        }
        
        void OnValidate()
        {
            ApplyColor(Colors);
        }

        void OnDidApplyAnimationProperties()
        {
            ApplyColor(Colors);
        }

        void CreateInternalReceivers()
        {
            List<IColorReceiver> colorReceivers = new List<IColorReceiver>();
            colorReceivers.AddRange(receivers.Where(receiver => receiver is IColorReceiver).Cast<IColorReceiver>());
            
            foreach (MonoBehaviour receiver in receivers)
            {
                IColorReceiver colorReceiver = receiver as IColorReceiver;
                
                if (colorReceiver != null)
                {
                    colorReceivers.Add(colorReceiver);
                    continue;
                }
                
                colorReceiver = receiver.GetComponent<IColorReceiver>();
                
                if (colorReceiver != null)
                {
                    colorReceivers.Add(colorReceiver);
                    continue;
                }
                
                Debug.Log($"Component {receiver.name} is not ColorReceiver");
            }

            internalReceivers = colorReceivers.ToArray();
        }
    }
}