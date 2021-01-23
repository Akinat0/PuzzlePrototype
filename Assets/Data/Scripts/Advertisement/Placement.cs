using System;
using UnityEngine.Advertisements;

namespace Puzzle.Advertisements
{
    public abstract class Placement : IUnityAdsListener
    {
        public Placement(Action finished, Action skipped, Action failed)
        {
            Finished = finished;
            Skipped = skipped;
            Failed = failed;
        }

        public event Action Finished;
        public event Action Skipped;
        public event Action Failed;
        public event Action Completed; //will be called anyway

        public bool WasShown { get; private set; }
        public abstract string ID { get; }

        public void Show()
        {
            if (WasShown)
            {
                PuzzleAdvertisement.Log($"Placement {ID} is trying to show itself several times");
                return;
            }

            bool success = Account.Advertisement.ShowPlacement(this);

            if (success)
                WasShown = false;
        }
        
        #region IUnityAdsListener
        
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    Finished?.Invoke();
                    break;
                case ShowResult.Skipped:
                    Skipped?.Invoke();
                    break;
                case ShowResult.Failed:
                    Failed?.Invoke();
                    break;
            }
            
            Completed?.Invoke();
            
            PuzzleAdvertisement.Log($"Placement {placementId} ads did finish with result {showResult}");
        }
        
        public void OnUnityAdsReady(string placementId)
        {
            PuzzleAdvertisement.Log($"Placement {placementId} is ready");
        }

        public void OnUnityAdsDidError(string message)
        {
            PuzzleAdvertisement.Log($" Error: {message}");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            PuzzleAdvertisement.Log($"Placement {placementId} ads did start");
        }
        
        #endregion
    }
}