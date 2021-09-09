using UnityEngine;

namespace Abu.Tools.UI
{
    public class FadeOverlayView : OverlayView
    {
        Color FadeColor
        {
            get => fadeColor;
            set
            {
                if(fadeColor == value)
                    return;

                fadeColor = value;
                ProcessPhase();
            }
        }

        [SerializeField] Color fadeColor;
        
        protected override void ProcessPhase()
        {
            Background.color = Color.Lerp(Color.clear, FadeColor, Phase);
        }
    }
}