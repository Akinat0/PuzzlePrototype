using System.Linq;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class UIColorProvider : MonoBehaviour, IColorReceiver
    {
        [SerializeField] Color[] colors;
        [SerializeField, RequireType(typeof(IColorReceiver))] 
        MonoBehaviour[] receivers;

        IColorReceiver[] colorReceivers;

        public IColorReceiver[] Receivers => 
            colorReceivers ?? (colorReceivers = receivers.OfType<IColorReceiver>().ToArray());

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
    }
}