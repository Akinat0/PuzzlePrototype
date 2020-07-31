using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class ShowCollectionEventArgs 
{
    public LevelColorScheme ColorScheme { get; private set; }
    
    public int? ItemID { get; set; }

    public ShowCollectionEventArgs(LevelColorScheme _Scheme, int? _ItemID = null)
    {
        ColorScheme = _Scheme;
        ItemID = _ItemID;
    }
    
}
