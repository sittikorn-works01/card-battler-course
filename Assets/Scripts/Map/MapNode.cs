using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapNode
{
    public NodeType Type;
    public int Floor;
    public Vector2 Position;
    public List<MapNode> ConnectedNodes = new List<MapNode>();
    public bool Visited;
}

[Serializable]
public class MapGraph
{
    public List<List<MapNode>> Floors = new List<List<MapNode>>();
    public MapNode CurrentNode;
}