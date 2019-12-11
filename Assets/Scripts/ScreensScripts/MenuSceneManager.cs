using System;
using UnityEngine;
using Abu.Tools;

namespace ScreensScripts
{
    public class MenuSceneManager : MonoBehaviour
    {
        [SerializeField] private AsyncLoader asyncLoader;
        
        public void Play()
        {
            asyncLoader?.LoadScene("GameScene");
        }

    }
}