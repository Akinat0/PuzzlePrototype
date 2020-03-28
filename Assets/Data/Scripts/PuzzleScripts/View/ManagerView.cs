using UnityEngine;

namespace Puzzle
{
    public abstract class ManagerView : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        }

        protected virtual void OnDisable()
        {
            GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        }
        protected abstract void SetupLevelEvent_Handler(LevelColorScheme levelColorScheme);
    }
}