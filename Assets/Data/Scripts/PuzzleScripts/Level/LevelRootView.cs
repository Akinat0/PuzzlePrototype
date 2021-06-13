using UnityEngine;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] PlayerView m_PlayerView;

    StarsController starsController;

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

    public StarsController GetStarsManager(LevelConfig config)
    {
        if (!config.StarsEnabled)
            return null;

        if (starsController == null)
            starsController = StarsController.Create(transform);
        
        return starsController;
    
    }
}
