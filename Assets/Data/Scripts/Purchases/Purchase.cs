using System;
using UnityEngine;

public abstract class Purchase
{
    public abstract bool Available { get; }
    
    public abstract bool Process(Action success);

    public abstract GameObject CreateView(RectTransform rectTransform);
}
