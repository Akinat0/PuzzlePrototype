using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelRootView : MonoBehaviour
{
    [SerializeField] private PlayerView m_PlayerView;

    StarsView starsView;
    Renderer[] renderers;

    Renderer[] Renderers
    {
        get
        {
            if (renderers == null)
                renderers = GetComponentsInChildren<Renderer>();

            if(renderers.Any(r => r == null))
                renderers = GetComponentsInChildren<Renderer>();
            
            return renderers;
        }
    }

    SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = gameObject.AddComponent<SortingGroup>();
    }
    
    public void SetActiveLevelRoot(bool value)
    {
        foreach (Renderer renderer in Renderers)
            renderer.enabled = value;
    }

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

    public StarsView StarsView
    {
        get
        {
            if (starsView == null)
                starsView = StarsView.Create(transform);
            return starsView;
        }
    }
}
