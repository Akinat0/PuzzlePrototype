using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemHelper : MonoBehaviour
{
   
    public float m_SimulationTime;
    private ParticleSystem m_Particles;
 
    void Awake()
    {
        m_Particles = GetComponent<ParticleSystem>();
        m_Particles.Simulate(m_SimulationTime, true, true);
        m_Particles.Play();
    }
    
}
