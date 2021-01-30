using UnityEngine;
using UnityEngine.Advertisements;

namespace Puzzle.Advertisements
{
    public class PuzzleAdvertisement
    {
        public PuzzleAdvertisement()
        {
            Advertisement.Initialize(StoreID, IsTestMode);
        }
        
        string StoreID
        {
            get
            {
#if UNITY_IOS
                return "3984570";
#elif UNITY_ANDROID
                return "3984571";
#else
                return string.Empty;
#endif
            }
        }
        
        bool IsTestMode = true; //switch it to false on distribution build

        public bool ShowPlacement(Placement placement)
        {
            if (!Advertisement.isSupported || Advertisement.isShowing
                || !Advertisement.IsReady(placement.ID) || placement.WasShown)
                return false;

            Advertisement.AddListener(placement);
            placement.Completed += () => Advertisement.RemoveListener(placement);
            Advertisement.Show(placement.ID);
            return true;
        }
        
        public static void Log(string msg)
        {
            Debug.Log($"<color=purple> [Advertisement] {msg} </color>");
        }
    }
}