using System;
using UnityEngine;

namespace Abu.Tools
{
    public class Trigger
    {
        public Trigger(string id, bool defaultValue = false)
        {
            Id = id;
            Default = defaultValue;

            cachedValue = PlayerPrefs.GetInt(Id, Default ? 1 : 0) == 1;
        }

        public event Action<bool> Changed; 
        
        public bool Value
        {
            get => cachedValue;
            set
            {
                if (cachedValue == value)
                    return;

                cachedValue = value;
                
                Changed?.Invoke(cachedValue);
                
                PlayerPrefs.SetInt(Id, cachedValue ? 1 : 0);
            }
        }

        string Id { get; }
        bool Default { get; }

        bool cachedValue;

        public static implicit operator bool(Trigger trigger) => trigger.Value;
    }
}

