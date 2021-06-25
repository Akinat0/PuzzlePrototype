using UnityEngine;
using System.Collections;
using Promises;

public class PromiseTimer : MonoBehaviour
{
    #region public methods

    public void CancelInTime<TSuccess, TFail>(Deferred<TSuccess, TFail> _Deferred, float _Time)
    {
        StartCoroutine(DoCancelInTime(_Deferred, _Time));
    }

    #endregion

    #region service methods

    IEnumerator DoCancelInTime<TSuccess, TFail>(Deferred<TSuccess, TFail> _Deferred, float _Time)
    {
        yield return new WaitForSeconds(_Time);
        
        // This check is actually redundant, just for debug's sake
        if (!_Deferred.IsResolved)
        {
            Debug.LogFormat("Cancelling promise by timeout {0}", _Deferred);
            _Deferred.Cancel();
        }
    }

    #endregion
}