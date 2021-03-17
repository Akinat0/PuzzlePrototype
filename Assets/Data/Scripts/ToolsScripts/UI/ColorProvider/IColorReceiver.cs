using UnityEngine;

namespace Abu.Tools.UI
{
    public interface IColorReceiver
    {
        void ApplyColor(params Color[] colors);
    }
}