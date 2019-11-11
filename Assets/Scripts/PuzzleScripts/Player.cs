using ScreensScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Puzzle
{
    public class Player : MonoBehaviour
    {
        
        private int _health = 3;
        public bool[] sides = {false, false, true, true}; //It's relative to Side // True means it's stick out
        
        private void Start()
        {
            
            MobileInput.TouchOnTheScreen += TouchOnScreen_Handler;
        }

        private void OnDestroy()
        {
            MobileInput.TouchOnTheScreen -= TouchOnScreen_Handler;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            enemy.OnHitPlayer(this);
        }

        private void PlayerDied()
        {
            GameSceneManager.Instance.score.SaveScore();
            SceneManager.LoadScene("GameEndScene");
        }


        public void DealDamage(int damage)
        {
            GameSceneManager.Instance.ShakeCamera();
            for (int i = 0; i < damage; i++)
            {
                GameSceneManager.Instance.healthManager.LoseHeart();
            }
            _health -= damage;
            if (_health == 0)
            {
                PlayerDied();
            }
            
        }

        private void TouchOnScreen_Handler(Touch touch)
        {
            ChangeSides();
        }
        
        public void ChangeSides()
        {
            for (int i = 0; i < sides.Length; i++)
                sides[i] = !sides[i];
            transform.Rotate(new Vector3(0, 0, 180));
        }
    }
    
}