using UnityEngine;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView; 
    [SerializeField] private BackgroundView m_BackgroundView;

    Renderer[] renderers;
    Renderer[] Renderers => renderers ?? (renderers = GetComponentsInChildren<Renderer>());

    public void SetActiveLevelRoot(bool value)
    {
        foreach (Renderer renderer in Renderers)
            renderer.enabled = value;
    }

    public PlayerView PlayerView
    {
        get { return m_PlayerView;}
        set
        {
            m_PlayerView = value;
            m_PlayerView.transform.SetParent(transform);
        }
    }
    
    public BackgroundView BackgroundView => m_BackgroundView;

}
