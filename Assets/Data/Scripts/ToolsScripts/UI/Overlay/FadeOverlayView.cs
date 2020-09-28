using UnityEngine;

namespace Abu.Tools.UI
{
    public class FadeOverlayView : OverlayView
    {
        protected override void ProcessPhase()
        {
            Color color = Color;
            color.a = Phase;
            Color = color;

            Background.raycastTarget = Phase > Mathf.Epsilon;
        }
    }
}