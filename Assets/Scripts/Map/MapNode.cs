using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapNode
{
    // Stable key used to reconnect node references after JSON loading.
    public int Id;
    public NodeType Type;
    public int Floor;
    public Vector2 Position;
    // Runtime references are rebuilt from ConnectedNodeIds after loading.
    [NonSerialized] public List<MapNode> ConnectedNodes = new List<MapNode>();
    public List<int> ConnectedNodeIds = new List<int>();
    public bool Visited = false;
}

[Serializable]
public class MapGraph
{
    public List<MapFloor> Floors = new();
    // Runtime reference is rebuilt from CurrentNodeId after loading.
    [NonSerialized] public MapNode CurrentNode;
    public int CurrentNodeId = -1;
}
[Serializable]
public class MapFloor
{
    public List<MapNode> NodesInFloor = new();
}
