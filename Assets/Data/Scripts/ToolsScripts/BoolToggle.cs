using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abu.Tools
{
    public class BoolToggle
    {
        private bool m_DefaultValue;
        private bool m_LatestValue;

        /// <summary>
        /// After getting the value toggle will return to the default value
        /// </summary>
        public bool Value
        {
            get
            {
                bool result = m_LatestValue;
                m_LatestValue = m_DefaultValue;
                return result;
            }
            set { m_LatestValue = value; }
        }
        
        public BoolToggle(bool _DefaultValue)
        {
            m_DefaultValue = _DefaultValue;
            m_LatestValue = m_DefaultValue;
        }
    }
}

