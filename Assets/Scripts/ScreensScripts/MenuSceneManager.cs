using System;
using UnityEngine;
using Abu.Tools;

namespace ScreensScripts
{
    public class MenuSceneManager : MonoBehaviour
    {
        public static MenuSceneManager Instance;
        
        [SerializeField] private AsyncLoader _asyncLoader;
        
        private void Awake()
        {
            Instance = this;
        }
        public void Play()
        {
            _asyncLoader?.LoadScene("GameScene");
        }

      
    }
}