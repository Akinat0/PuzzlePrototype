using UnityEngine;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView; 
    [SerializeField] private BackgroundView m_BackgroundView;



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
