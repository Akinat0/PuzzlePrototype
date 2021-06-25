/*
  modified from original source: https://bitbucket.org/mattkotsenas/c-promises/overview
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Promises
{
    public abstract class Promise
    {
        public abstract Promise OnSuccess(Action _Callback);

        public abstract Promise OnFail(Action _Callback);

        public abstract Promise OnFail(Action<string> _Callback);

        public abstract Promise OnFail(IEnumerable<Action<string>> _Callbacks);

        public abstract Promise OnCancel(Action _Callback);

        public abstract Promise OnCancel(Action<string> _Callback);

        public abstract Promise OnCancel(IEnumerable<Action<string>> _Callbacks);

        public abstract Promise OnResolved(Action _Callback);

        public abstract bool IsFailed    { get; }
        public abstract bool IsSuccess   { get; }
        public abstract bool IsCancelled { get; }
        public abstract bool IsResolved  { get; }
    }

    public static class ExtensionPromise
    {
        public static Deferred Then(this Promise _Promise, Action<Deferred> _Resolver)
        {
            Deferred thenPromise = new Deferred();
            _Promise.OnFail(_Error => thenPromise.Fail(_Error));
            _Promise.OnCancel(_Error => thenPromise.Cancel(_Error));
            _Promise.OnSuccess(() => _Resolver(thenPromise));
            return thenPromise;
        }
        
        public static Deferred<TSuccess2> Cast<TSuccess2>(this Promise _Promise, Func<TSuccess2> _Resolver)
        {
            Deferred<TSuccess2> thenPromise = new Deferred<TSuccess2>();
            _Promise.OnFail(_Error => thenPromise.Fail(_Error));
            _Promise.OnCancel(_Error => thenPromise.Cancel(_Error));
            _Promise.OnSuccess(
                    () =>
                    {
                        try
                        {
                            thenPromise.Success(_Resolver());
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
            
                            if (e.Message is string)
                                thenPromise.Fail(e.Message);
                            else
                                thenPromise.Fail();
                        }
                    }
                );
            return thenPromise;
        }            
        
        public static Deferred<TSuccess2> Cast<TSuccess, TSuccess2>(this Promise<TSuccess> _Promise, Func<TSuccess, TSuccess2> _Resolver)
        {
            Deferred<TSuccess2> thenPromise = new Deferred<TSuccess2>();
            _Promise.OnFail(_Error => thenPromise.Fail(_Error));
            _Promise.OnCancel(_Error => thenPromise.Cancel(_Error));
            _Promise.OnSuccess(
                    _Result =>
                    {
                        try
                        {
                            thenPromise.Success(_Resolver(_Result));
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
            
                            if (e.Message is string)
                                thenPromise.Fail(e.Message);
                            else
                                thenPromise.Fail();
                        }
                    }
                );
            return thenPromise;
        }        
        
        public static Deferred<TSuccess, TError2> Cast<TSuccess, TError, TError2>(this Promise<TSuccess, TError> _Promise, Func<TError, TError2> _ErrorResolver)
        {
            if (_ErrorResolver == null)
                _ErrorResolver = _Error => default;
            
            Deferred<TSuccess, TError2> thenPromise = new Deferred<TSuccess, TError2>();
            _Promise.OnFail(_Error => thenPromise.Fail(_ErrorResolver(_Error)));
            _Promise.OnCancel(_Error => thenPromise.Cancel(_ErrorResolver(_Error)));
            _Promise.OnSuccess(thenPromise.Success);
            
            return thenPromise;
        } 
        
        public static Deferred Then(this Promise _Promise, Func<Promise> _ThenPromiseFactory)
        {
            Deferred sumPromise = new Deferred();
            _Promise.OnFail(_Error => sumPromise.Fail(_Error));
            _Promise.OnCancel(_Error => sumPromise.Cancel(_Error));
            _Promise.OnSuccess(
                    () =>
                    {
                        try
                        {
                            Promise thenPromise = _ThenPromiseFactory();
                            thenPromise.OnFail(_Error => sumPromise.Fail(_Error));
                            thenPromise.OnCancel(_Error => sumPromise.Cancel(_Error));
                            thenPromise.OnSuccess(sumPromise.Success);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                            #if UNITY_EDITOR
                            Debug.LogException(e);
                            #endif

                            if (e.Message is string)
                                sumPromise.Fail(e.Message);
                            else
                                sumPromise.Fail();
                        }
                    }
                );
            return sumPromise;
        }

        public static Deferred<TSuccess> Then<TSuccess>(this Promise _Promise, Func<Promise<TSuccess>> _ThenPromiseFactory)
        {
            Deferred<TSuccess> sumPromise = new Deferred<TSuccess>();
            _Promise.OnFail(_Error => sumPromise.Fail(_Error));
            _Promise.OnCancel(_Error => sumPromise.Cancel(_Error));
            _Promise.OnSuccess(
                    () =>
                    {
                        try
                        {
                            Promise<TSuccess> thenPromise = _ThenPromiseFactory();
                            thenPromise.OnFail(_Error => sumPromise.Fail(_Error));
                            thenPromise.OnCancel(_Error => sumPromise.Cancel(_Error));
                            thenPromise.OnSuccess(sumPromise.Success);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                            #if UNITY_EDITOR
                            Debug.LogException(e);
                            #endif

                            if (e.Message is string)
                                sumPromise.Fail(e.Message);
                            else
                                sumPromise.Fail();
                        }
                    }
                );
            return sumPromise;
        }

        public static Deferred<TSuccess2> Then<TSuccess, TSuccess2>(this Promise<TSuccess> _Promise, Func<TSuccess, Promise<TSuccess2>> _ThenPromiseFactory)
        {
            Deferred<TSuccess2> sumPromise = new Deferred<TSuccess2>();
            _Promise.OnFail(_Error => sumPromise.Fail(_Error));
            _Promise.OnCancel(_Error => sumPromise.Cancel(_Error));
            _Promise.OnSuccess(
                    _Result =>
                    {
                        try
                        {
                            Promise<TSuccess2> thenPromise = _ThenPromiseFactory(_Result);
                            thenPromise.OnFail(_Error => sumPromise.Fail(_Error));
                            thenPromise.OnCancel(_Error => sumPromise.Cancel(_Error));
                            thenPromise.OnSuccess(sumPromise.Success);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                            #if UNITY_EDITOR
                            Debug.LogException(e);
                            #endif
        
                            if (e.Message is string)
                                sumPromise.Fail(e.Message);
                            else
                                sumPromise.Fail();
                        }
                    }
                );
            return sumPromise;
        }
    }

    public abstract class Promise<TSuccess> : Promise
    {
        public abstract Promise<TSuccess> OnSuccess(Action<TSuccess>              _Callback);
        public abstract Promise<TSuccess> OnSuccess(IEnumerable<Action<TSuccess>> _Callbacks);

        public abstract Promise<TSuccess> OnResolved(IEnumerable<Action> _Callbacks);
        public abstract Promise<TSuccess> OnResolved(Action<TSuccess>    _Callback);
    }

    public abstract class Promise<TSuccess, TFail> : Promise<TSuccess>
    {
        public abstract Promise<TSuccess, TFail> OnSuccess<TSuccess2, TFail2>(Action<TSuccess> _Callback);

        public abstract Promise OnFail(Action<TFail> _Callback);
        
        public abstract Promise OnCancel(Action<TFail> _Callback);
    }

    public interface IDeferred
    {
        void Success();
        void Fail();
        void Cancel();
    }

    public class Deferred<TSuccess, TFail> : Promise<TSuccess, TFail>, IDeferred
    {
        #region Types

        enum State
        {
            NOT_RESOLVED,
            SUCCESS,
            FAIL,
            CANCEL
        }

        abstract class AbstractCallback<TSuccess2>
        {
            protected readonly Deferred<TSuccess2, TFail> m_Deferred;

            public AbstractCallback(Deferred<TSuccess2, TFail> _Deferred)
            {
                m_Deferred = _Deferred;
            }

            public abstract void TryCall();
        }

        abstract class Callback<TSuccess2, TCallback> : AbstractCallback<TSuccess2>
        {
            protected readonly TCallback m_Delegate;

            public Callback(Deferred<TSuccess2, TFail> _Deferred, TCallback _Delegate) : base(_Deferred)
            {
                m_Delegate = _Delegate;
            }
        }

        class SuccessCallback : Callback<TSuccess, Action>
        {
            public SuccessCallback(Deferred<TSuccess, TFail> _Deferred, Action _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsSuccess)
                    m_Delegate();
            }
        }

        class SuccessArgCallback : Callback<TSuccess, Action<TSuccess>>
        {
            public SuccessArgCallback(Deferred<TSuccess, TFail> _Deferred, Action<TSuccess> _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsSuccess)
                    m_Delegate(m_Deferred.m_ArgSuccess);
            }
        }

        class FailCallback : Callback<TSuccess, Action>
        {
            public FailCallback(Deferred<TSuccess, TFail> _Deferred, Action _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsFailed)
                    m_Delegate();
            }
        }

        class FailArgCallback : Callback<TSuccess, Action<TFail>>
        {
            public FailArgCallback(Deferred<TSuccess, TFail> _Deferred, Action<TFail> _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsFailed)
                    m_Delegate(m_Deferred.m_ArgFail);
            }
        }

        class CancelCallback : Callback<TSuccess, Action>
        {
            public CancelCallback(Deferred<TSuccess, TFail> _Deferred, Action _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsCancelled)
                    m_Delegate();
            }
        }

        class CancelArgCallback : Callback<TSuccess, Action<TFail>>
        {
            public CancelArgCallback(Deferred<TSuccess, TFail> _Deferred, Action<TFail> _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsCancelled)
                    m_Delegate(m_Deferred.m_ArgCancel);
            }
        }

        class ResolveCallback : Callback<TSuccess, Action>
        {
            public ResolveCallback(Deferred<TSuccess, TFail> _Deferred, Action _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsResolved)
                    m_Delegate();
            }
        }

        class ResolveArgCallback : Callback<TSuccess, Action<TSuccess>>
        {
            public ResolveArgCallback(Deferred<TSuccess, TFail> _Deferred, Action<TSuccess> _Delegate) : base(_Deferred, _Delegate)
            {
            }

            public override void TryCall()
            {
                if (m_Deferred.IsResolved)
                    m_Delegate(m_Deferred.IsSuccess ? m_Deferred.m_ArgSuccess : default(TSuccess));
            }
        }

        #endregion

        #region Attributes

        readonly List<AbstractCallback<TSuccess>> m_Callbacks = new List<AbstractCallback<TSuccess>>();

        State    m_State;
        TSuccess m_ArgSuccess;
        TFail    m_ArgFail   = default(TFail);
        TFail    m_ArgCancel = default(TFail);

        #endregion

        #region Properties

        public override bool IsFailed
        {
            get { return m_State == State.FAIL; }
        }

        public override bool IsResolved
        {
            get { return m_State != State.NOT_RESOLVED; }
        }

        public override bool IsSuccess
        {
            get { return m_State == State.SUCCESS; }
        }

        public override bool IsCancelled
        {
            get { return m_State == State.CANCEL; }
        }

        public bool HasCallbacks
        {
            get { return m_Callbacks.Any(); }
        }

        #endregion

        #region Construction

        public Deferred()
        {
        }

        public Deferred(
                Action<TSuccess> _Success,
                Action<string>   _Fail,
                Action<string>   _Cancel
            )
        {
            OnSuccess(_Success);
            OnFail(_Fail);
            OnCancel(_Cancel);
        }

        #endregion

        #region Aggregates

        public static Promise When(IEnumerable<Promise> _Promises)
        {
            int      count         = _Promises.Count();
            Deferred masterPromise = new Deferred();

            foreach (Promise p in _Promises)
            {
                p.OnFail(_Error => masterPromise.Fail(_Error));
                p.OnCancel(_Error => masterPromise.Cancel(_Error));
                p.OnSuccess(
                        () =>
                        {
                            count--;
                            if (0 == count)
                            {
                                masterPromise.Success();
                            }
                        }
                    );
            }

            return masterPromise;
        }

        public static Promise When(object _D)
        {
            Deferred masterPromise = new Deferred();
            masterPromise.Success();
            return masterPromise;
        }

        public static Promise When(Deferred _D)
        {
            return _D;
        }

        public static Promise<TSuccess> When(Deferred<TSuccess> _D)
        {
            return _D;
        }

        #endregion

        #region Subscription

        public override Promise OnResolved(Action _Callback)
        {
            if (_Callback != null)
            {
                if (IsResolved)
                    _Callback();
                else
                    m_Callbacks.Add(new ResolveCallback(this, _Callback));
            }

            return this;
        }

        public override Promise<TSuccess> OnResolved(Action<TSuccess> _Callback)
        {
            if (_Callback != null)
            {
                if (IsResolved)
                    _Callback(m_ArgSuccess);
                else
                    m_Callbacks.Add(new ResolveArgCallback(this, _Callback));
            }

            return this;
        }

        public override Promise<TSuccess> OnResolved(IEnumerable<Action> _Callbacks)
        {
            foreach (Action callback in _Callbacks)
            {
                if (callback != null)
                    OnResolved(callback);
            }

            return this;
        }

        public override Promise OnSuccess(Action _Callback)
        {
            if (_Callback != null)
            {
                if (IsSuccess)
                    _Callback();
                else if (!IsResolved)
                    m_Callbacks.Add(new SuccessCallback(this, _Callback));
            }

            return this;
        }

        public override Promise<TSuccess> OnSuccess(Action<TSuccess> _Callback)
        {
            if (_Callback != null)
            {
                if (IsSuccess)
                    _Callback(m_ArgSuccess);
                else if (!IsResolved)
                    m_Callbacks.Add(new SuccessArgCallback(this, _Callback));
            }

            return this;
        }

        public override Promise<TSuccess, TFail> OnSuccess<TSuccess2, TFail2>(Action<TSuccess> _Callback)
        {
            if (_Callback != null)
            {
                if (IsSuccess)
                    _Callback(m_ArgSuccess);
                else if (!IsResolved)
                    m_Callbacks.Add(new SuccessArgCallback(this, _Callback));
            }

            return this;            
        }

        public override Promise<TSuccess> OnSuccess(IEnumerable<Action<TSuccess>> _Callbacks)
        {
            foreach (Action<TSuccess> callback in _Callbacks)
            {
                if (callback != null)
                    OnSuccess(callback);
            }

            return this;
        }

        public override Promise OnFail(Action<TFail> _Callback)
        {
            if (_Callback != null)
            {
                if (IsFailed)
                    _Callback(m_ArgFail);
                else if (!IsResolved)
                    m_Callbacks.Add(new FailArgCallback(this, _Callback));
            }

            return this;            
        }

        public override Promise OnFail(Action _Callback)
        {
            if (_Callback != null)
            {
                if (IsFailed)
                    _Callback();
                else if (!IsResolved)
                    m_Callbacks.Add(new FailCallback(this, _Callback));
            }

            return this;
        }

        public override Promise OnFail(Action<string> _Callback)
        {
            if (_Callback != null)
            {
                if (IsFailed)
                    _Callback(m_ArgFail.ToString());
                else if (!IsResolved)
                    m_Callbacks.Add(new FailArgCallback(this, _ArgFail => _Callback(_ArgFail != null ? _ArgFail.ToString() : null)));
            }

            return this;
        }

        public override Promise OnFail(IEnumerable<Action<string>> _Callbacks)
        {
            foreach (Action<string> callback in _Callbacks)
            {
                if (callback != null)
                    OnFail(callback);
            }

            return this;
        }
        
        public override Promise OnCancel(Action<TFail> _Callback)
        {
            if (_Callback != null)
            {
                if (IsCancelled)
                    _Callback(m_ArgCancel);
                else if (!IsResolved)
                    m_Callbacks.Add(new CancelArgCallback(this, _Callback));
            }

            return this;            
        }        

        public override Promise OnCancel(Action _Callback)
        {
            if (_Callback != null)
            {
                if (IsCancelled)
                    _Callback();
                else if (!IsResolved)
                    m_Callbacks.Add(new CancelCallback(this, _Callback));
            }

            return this;
        }

        public override Promise OnCancel(Action<string> _Callback)
        {
            if (_Callback != null)
            {
                if (IsCancelled)
                    _Callback(m_ArgCancel.ToString());
                else if (!IsResolved)
                    m_Callbacks.Add(new CancelArgCallback(this, _ArgFail => _Callback(_ArgFail != null ? _ArgFail.ToString() : null)));
            }

            return this;
        }

        public override Promise OnCancel(IEnumerable<Action<string>> _Callbacks)
        {
            foreach (Action<string> callback in _Callbacks)
            {
                if (callback != null)
                    OnCancel(callback);
            }

            return this;
        }

        #endregion

        #region Resolvement

        public void Success()
        {
            if (IsResolved)
                return;
            m_State = State.SUCCESS;
            DequeueCallbacks();
        }

        public void Success(TSuccess _Arg)
        {
            if (IsResolved)
                return;
            m_State      = State.SUCCESS;
            m_ArgSuccess = _Arg;
            DequeueCallbacks();
        }

        public void Fail()
        {
            if (IsResolved)
                return;
            m_State = State.FAIL;
            DequeueCallbacks();
        }

        public void Fail(TFail _Arg)
        {
            if (IsResolved)
                return;
            m_State   = State.FAIL;
            m_ArgFail = _Arg;
            DequeueCallbacks();
        }

        public void Cancel()
        {
            if (IsResolved)
                return;
            m_State = State.CANCEL;
            DequeueCallbacks();
        }

        public void Cancel(TFail _Arg)
        {
            if (IsResolved)
                return;
            m_State     = State.CANCEL;
            m_ArgCancel = _Arg;
            DequeueCallbacks();
        }

        #endregion


        #region Timeout

        static PromiseTimer m_PromiseTimer;

        static PromiseTimer PromiseTimer
        {
            get
            {
                if (m_PromiseTimer == null)
                    m_PromiseTimer = new GameObject("TimeoutHolder").AddComponent<PromiseTimer>();

                return m_PromiseTimer;
            }
        }

        public void Timeout(float _Time)
        {
            PromiseTimer.CancelInTime(this, _Time);
        }

        #endregion

        void DequeueCallbacks()
        {
            foreach (AbstractCallback<TSuccess> callback in m_Callbacks.ToArray())
            {
                callback.TryCall();
            }

            m_Callbacks.Clear();
        }
    }
    
    public class Deferred<TSuccess> : Deferred<TSuccess, string>
    {
    }
    
    public class Deferred : Deferred<object, string>
    {
        // generic object
    }    
}