using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapGraph GenerateAct(int floorCount, int nodesPerFloor)
    {
        var map = new MapGraph();
        var rng = new System.Random();

        for (int floor = 0; floor < floorCount; floor++)
        {
            var nodes = new List<MapNode>();
            int countThisFloor = (floor == floorCount - 1) ? 1 : nodesPerFloor; // last floor = Boss only

            for (int i = 0; i < countThisFloor; i++)
            {
                var node = new MapNode
                {
                    Floor = floor,
                    Position = new Vector2(i - (countThisFloor - 1) / 2f, 0),
                    Type = PickNodeType(floor, floorCount, rng)
                };
                nodes.Add(node);
            }
            map.Floors.Add(nodes);
        }

        // Connect each node to 1-2 nodes on the next floor.
        for (int floor = 0; floor < map.Floors.Count - 1; floor++)
        {
            List<MapNode> current = map.Floors[floor];
            List<MapNode> next = map.Floors[floor + 1];

            foreach (MapNode node in current)
            {
                int connections = rng.Next(1, 3); // 1 or 2 outgoing paths
                for (int c = 0; c < connections; c++)
                {
                    MapNode target = next[rng.Next(next.Count)];
                    if (!node.ConnectedNodes.Contains(target))
                        node.ConnectedNodes.Add(target);
                }
            }
        }

        return map;
    }

    private static NodeType PickNodeType(int floor, int floorCount, System.Random rng)
    {
        if (floor == 0) return NodeType.Battle;             // always start on a normal fight
        if (floor == floorCount - 1) return NodeType.Boss;  // last floor is always the boss

        double roll = rng.NextDouble();
        if (roll < 0.50) return NodeType.Battle;
        if (roll < 0.92) return NodeType.Shop;
        return NodeType.Rest;
    }
}