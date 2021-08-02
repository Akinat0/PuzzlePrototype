using UnityEngine;

namespace Abu.Tools.UI
{
    public class FadeOverlayView : OverlayView
    {
        protected override void ProcessPhase()
        {
            Color color = Background.color;
            color.a = Phase;
            Background.color = color;
        }
    }
}