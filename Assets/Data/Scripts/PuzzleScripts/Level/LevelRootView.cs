using UnityEngine;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView; 
    [SerializeField] private BackgroundView m_BackgroundView;



    public PlayerView PlayerView => m_PlayerView;
    public BackgroundView BackgroundView => m_BackgroundView;

}
