using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    public NodeType Type;
    public Vector2 Position;
    public List<MapNode> ConnectedNodes = new();
    public bool Visited;
}
