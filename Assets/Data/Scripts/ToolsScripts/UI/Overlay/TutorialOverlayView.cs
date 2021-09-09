using UnityEngine;

namespace Abu.Tools.UI
{
    public class TutorialOverlayView : OverlayView
    {
        [SerializeField] TutorialFadeImage tutorialFade;

        protected override void ProcessPhase()
        {
            tutorialFade.Smoothness = 1 - Phase;
        }

        public void AddHole(TutorialHole hole)
        {
            tutorialFade.AddHole(hole);
        }

        public void RemoveHole(TutorialHole hole)
        {
            tutorialFade.RemoveHole(hole);
        }
    }
}