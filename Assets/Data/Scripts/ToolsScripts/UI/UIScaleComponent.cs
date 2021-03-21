
using UnityEngine;

namespace Abu.Tools.UI
{
    public class UIScaleComponent : UIComponent
    {
        [SerializeField] AnimationCurve scaleCurve = AnimationCurve.Linear(0,  0, 1, 1);
        [SerializeField, Range(0, 1)] float phase = 1;

        public float Phase
        {
            get => phase;
            set
            {
                value = Mathf.Clamp01(value);
                
                if (Mathf.Approximately(phase, value))
                    return;

                phase = value;
                ProcessPhase();
            }
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            ProcessPhase();
        }

        void ProcessPhase()
        {
            float scale = scaleCurve.Evaluate(phase);
            transform.localScale = Vector2.one * scale;
        }
    }
}