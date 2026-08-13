using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RunData
{
    public int CurrentHp = 50;
    public int MaxHp = 50;
    public int Gold = 99;

    public int CurrentActIndex = 0;
    public int CurrentFloorIndex = 0;

    // The generated map for the current act, so MapView can rebuild
    // its display after returning from battle/shop/etc.
    public MapGraph CurrentMap = null;
}