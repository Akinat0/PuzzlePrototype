using UnityEngine;

public class InfinitySoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip theme;
    private AudioSource currentThemeSource;

    private void Start()
    {
        PlayTheme(theme);
    } 

    private void PlayTheme(AudioClip clip)
    {
        if(currentThemeSource != null) 
            Destroy(currentThemeSource.gameObject);
        
        currentThemeSource = new GameObject("Theme " + clip.name).AddComponent<AudioSource>();
        currentThemeSource.clip = clip;
        currentThemeSource.Play();
    }
}
