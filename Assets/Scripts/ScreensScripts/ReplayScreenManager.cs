using UnityEngine;
using UnityEngine.UI;

namespace ScreensScripts.ReplayScreen
{
    public class ReplayScreenManager : MonoBehaviour
    {
        [SerializeField]
        private Text _endGameText;

        void Start()
        {
            int score = PlayerPrefs.GetInt("score", 0);
            _endGameText.text += " " + score;
        }

        public void Replay()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}