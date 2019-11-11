using UnityEngine;
using Abu.Tools;
public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private AsyncLoader _asyncLoader;
    public void Play()
    {
        _asyncLoader?.LoadScene("GameScene");
       // UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
