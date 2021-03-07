using UnityEngine;

namespace Abu.Console
{
    public class DebugHapticMenu : MonoBehaviour
    {
        [SerializeField] TextButtonComponent defaultHaptic;
        [SerializeField] TextButtonComponent failureHaptic;
        [SerializeField] TextButtonComponent successHaptic;
        [SerializeField] TextButtonComponent warningHaptic;
        [SerializeField] TextButtonComponent selectionHaptic;
        [SerializeField] TextButtonComponent heavyHaptic;
        [SerializeField] TextButtonComponent lightHaptic;
        [SerializeField] TextButtonComponent mediumHaptic;

        void Start()
        {
            defaultHaptic.Text = Haptic.Type.DEFAULT.ToString();
            failureHaptic.Text = Haptic.Type.FAILURE.ToString();
            successHaptic.Text = Haptic.Type.SUCCESS.ToString();
            warningHaptic.Text = Haptic.Type.WARNING.ToString();
            selectionHaptic.Text = Haptic.Type.SELECTION.ToString();
            heavyHaptic.Text = Haptic.Type.IMPACT_HEAVY.ToString();
            lightHaptic.Text = Haptic.Type.IMPACT_LIGHT.ToString();
            mediumHaptic.Text = Haptic.Type.IMPACT_MEDIUM.ToString();
            
            defaultHaptic.OnClick += () => Haptic.Run(Haptic.Type.DEFAULT);
            failureHaptic.OnClick += () => Haptic.Run(Haptic.Type.FAILURE);
            successHaptic.OnClick += () => Haptic.Run(Haptic.Type.SUCCESS);
            warningHaptic.OnClick += () => Haptic.Run(Haptic.Type.WARNING);
            selectionHaptic.OnClick += () => Haptic.Run(Haptic.Type.SELECTION);
            heavyHaptic.OnClick += () => Haptic.Run(Haptic.Type.IMPACT_HEAVY);
            lightHaptic.OnClick += () => Haptic.Run(Haptic.Type.IMPACT_LIGHT);
            mediumHaptic.OnClick += () => Haptic.Run(Haptic.Type.IMPACT_MEDIUM);
        }
    }
}