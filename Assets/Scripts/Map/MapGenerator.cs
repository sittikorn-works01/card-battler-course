using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapGraph GenerateAct(int floorCount, int maxNodesPerFloor)
    {
        var map = new MapGraph();
        var rng = new System.Random();

        

        for (int floor = 0; floor < floorCount; floor++)
        {
            MapFloor nodes = new();
            int nodesPerFloor = Random.Range(1, maxNodesPerFloor + 1);
            int countThisFloor = (floor == floorCount - 1) ? 1 : nodesPerFloor; // last floor = Boss only

            for (int i = 0; i < countThisFloor; i++)
            {
                var node = new MapNode
                {
                    Floor = floor,
                    Position = new Vector2(i - (countThisFloor - 1) / 2f, 0),
                    Type = PickNodeType(floor, floorCount, rng)
                };
                nodes.NodesInFloor.Add(node);
            }
            map.Floors.Add(nodes);
        }

        // Connect each node to 1-2 nodes on the next floor.
        for (int floor = 0; floor < map.Floors.Count - 1; floor++)
        {
            MapFloor currentFloorNodes = map.Floors[floor];
            MapFloor nextFloorNodes = map.Floors[floor + 1];

            foreach (MapNode node in currentFloorNodes.NodesInFloor)
            {
                int connections = rng.Next(1, 3); // 1 or 2 outgoing paths
                for (int c = 0; c < connections; c++)
                {
                    MapNode target = nextFloorNodes.NodesInFloor[rng.Next(nextFloorNodes.NodesInFloor.Count)];
                    if (!node.ConnectedNodes.Contains(target))
                        node.ConnectedNodes.Add(target);
                }
            }

            // The random pass above can leave some next-floor nodes with no
            // incoming connection at all, making them unreachable regardless
            // of path. Force-connect any orphan to a random node this floor.
            foreach (MapNode target in nextFloorNodes.NodesInFloor)
            {
                bool hasIncoming = false;
                foreach (MapNode source in currentFloorNodes.NodesInFloor)
                {
                    if (source.ConnectedNodes.Contains(target))
                    {
                        hasIncoming = true;
                        break;
                    }
                }

                if (!hasIncoming)
                {
                    MapNode fallbackSource = currentFloorNodes.NodesInFloor[rng.Next(currentFloorNodes.NodesInFloor.Count)];
                    fallbackSource.ConnectedNodes.Add(target);
                }
            }
        }

        return map;
    }

    private static NodeType PickNodeType(int floor, int floorCount, System.Random rng)
    {
        if (floor == 0) return NodeType.Elite;             // always start on a normal fight
        if (floor == floorCount - 1) return NodeType.Boss;  // last floor is always the boss

        double roll = rng.NextDouble();
        if (roll < 0.50) return NodeType.Elite;
        if (roll < 0.92) return NodeType.Shop;
        return NodeType.Rest;
    }
}