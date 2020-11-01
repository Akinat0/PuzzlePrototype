
using UnityEngine;

public abstract class Purchase
{
    public abstract bool Available { get; }
    public abstract bool Process();

    public abstract void CreateView(RectTransform rectTransform);
}
