using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abu.Tools
{
    public class BoolToggle
    {
        private bool DefaultValue;
        private bool LatestValue;

        /// <summary>
        /// After getting the value toggle will return to the default value
        /// </summary>
        public bool Value
        {
            get
            {
                bool result = LatestValue;
                LatestValue = DefaultValue;
                return result;
            }
            set => LatestValue = value;
        }

        public bool GetValueWithoutFire()
        {
            return LatestValue;
        }
        
        public BoolToggle(bool defaultValue)
        {
            DefaultValue = defaultValue;
            LatestValue = DefaultValue;
        }
    }
}

