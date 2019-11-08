using UnityEngine.UI;
using UnityEngine;

namespace Puzzle
{
    public class Score : MonoBehaviour
    {

        private int _score = 0;
        private Text _scoreText;

        void Start()
        {
            _scoreText = GetComponent<Text>();
            AddScore(0);
        }

        public void AddScore(int score)
        {
            _score += score;
            _scoreText.text = "Score: " + _score;
        }

        public void SaveScore()
        {
            PlayerPrefs.SetInt("score", _score);
        }
    }
}