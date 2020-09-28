using UnityEngine;

public class ParticlesWinFx : GameWinFx
{
    [SerializeField] ParticleSystem Particles;
    
    protected override void Show()
    {
        Particles.Play();
    }

    protected override void Hide()
    {
        Particles.Stop();
    }
}
