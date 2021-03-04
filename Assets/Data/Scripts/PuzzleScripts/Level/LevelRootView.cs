using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView;

    StarsManager starsManager;

    public PlayerView PlayerView
    {
        get => m_PlayerView;
        set
        {
            m_PlayerView = value;
            m_PlayerView.transform.SetParent(transform);
            m_PlayerView.transform.localPosition = Vector3.zero;
        }
    }

    public StarsManager GetStarsManager(LevelConfig config)
    {
        if (!config.StarsEnabled)
            return null;

        if (starsManager == null)
            starsManager = StarsManager.Create(transform, config.StarView);
        
        return starsManager;
    
    }
}
