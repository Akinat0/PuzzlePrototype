using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollectionEventArgs 
{

    public LevelColorScheme ColorScheme { get; private set; }

    public ShowCollectionEventArgs(LevelColorScheme _Scheme)
    {
        ColorScheme = _Scheme;
    }
    
}
