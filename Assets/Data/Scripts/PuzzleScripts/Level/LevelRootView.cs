using UnityEngine;

public class LevelRootView : MonoBehaviour
{
    private PlayerView m_PlayerView; 
    private BackgroundView m_BackgroundView;

    public PlayerView PlayerView
    {
        get
        {
            m_PlayerView = GetComponentInChildren<PlayerView>();
            if(m_PlayerView == null)
                Debug.LogError("Can't find player view in children of level root");
            return m_PlayerView;
        }
    }

    public BackgroundView BackgroundView
    {
        get
        {
            m_BackgroundView = GetComponentInChildren<BackgroundView>();
            if(m_BackgroundView == null)
                Debug.LogError("Can't find background view in children of level root");
            return m_BackgroundView;
        }
    }
    
}
