using UnityEngine;

namespace Abu.Tools.UI
{
    public class TutorialOverlayView : OverlayView
    {
        [SerializeField] TutorialFadeImage tutorialFade;

        protected override void ProcessPhase()
        {
            Color color = tutorialFade.color;
            color.a = Phase;
            tutorialFade.color = color;
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