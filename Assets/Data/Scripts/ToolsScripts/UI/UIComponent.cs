
using System;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        protected abstract void OnValidate();
    }
}