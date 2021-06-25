using System;
using System.Collections.Generic;
using System.Linq;
using Promises;

namespace Abu.Tools
{
    public class Requests : IDisposable
    {
        public event Action<Promise> RequestAdded;

        readonly List<Promise> requests = new List<Promise>(); 
        
        public void Add(Promise request)
        {
            request.OnResolved(() => requests.Remove(request));
            requests.Add(request);
            RequestAdded?.Invoke(request);
        }

        public bool Has(Func<Promise, bool> predicate)
        {
            return requests.Any(predicate);
        }

        public bool Has<T>() where T : Promise
        {
            bool Predicate(Promise request) => request is T;
            return Has(Predicate);
        }
        
        public Promise Get(Func<Promise, bool> predicate)
        {
            return requests.FirstOrDefault(predicate);
        }
        
        public T Get<T>() where T : Promise
        {
            bool Predicate(Promise request) => request is T;
            return Get(Predicate) as T;
        }
        
        public void Dispose()
        {
            RequestAdded = null;
            requests.Clear();
        }
    }
}