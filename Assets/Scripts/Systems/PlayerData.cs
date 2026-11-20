using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;

namespace CardBattlerCourse.Systems
{
    public class PlayerData : Singleton<PlayerData>
    {
        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;
        [SerializeField] private int gold;
        [SerializeField] private MapGraph currentMap;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public int Gold => gold;
        public MapGraph CurrentMap => currentMap;

        //private int CurrentActIndex = 0;
        //private int CurrentFloorIndex = 0;

        private string savePath;
        public static event Action<int, int> OnGoldChanged;
        public static event Action<int, int> OnHealthChanged;

        protected override void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
            base.Awake();
        }

        public bool TrySpendingGold(int price)
        {
            if (gold < price) return false;
            gold -= price;
            OnGoldChanged?.Invoke(gold, price);
            return true;
        }

        public void AddGold(int amount)
        {
            gold += amount;
            OnGoldChanged?.Invoke(gold, amount);
        }

        public void TakeDamage(float amount)
        { 
            currentHealth = Mathf.Max(0, currentHealth - amount);
            OnHealthChanged?.Invoke((int)currentHealth, (int)amount);
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke((int)currentHealth, (int)amount);
        }

        // Mirrors an already-computed health value (e.g. from the in-battle
        // Health component, which accounts for block) instead of applying its
        // own delta - use this when something else already did the math.
        public void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke((int)currentHealth, 0);
        }

        public void SaveMapProgression(MapGraph unsavedGraph)
        {
            currentMap = unsavedGraph;
        }

        public void Save()
        {
            PrepareMapForSave(CurrentMap);
            SaveData saveData = new SaveData(CurrentHealth, MaxHealth, Gold, CurrentMap);

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath, json);
        }

        private static void PrepareMapForSave(MapGraph map)
        {
            if (map == null) return;

            List<MapNode> nodes = GetAllNodes(map);
            var nodeIds = new Dictionary<MapNode, int>();

            // Assign a unique ID to every node in the floor lists.
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Id = i;
                nodeIds.Add(nodes[i], i);
            }

            // Save each connection as the ID of its destination node.
            foreach (MapNode node in nodes)
            {
                node.ConnectedNodeIds = new List<int>();
                if (node.ConnectedNodes == null) continue;

                foreach (MapNode connectedNode in node.ConnectedNodes)
                {
                    if (connectedNode != null && nodeIds.TryGetValue(connectedNode, out int connectedId))
                        node.ConnectedNodeIds.Add(connectedId);
                }
            }

            // -1 means the player has not selected a node yet.
            map.CurrentNodeId = -1;
            if (map.CurrentNode != null && nodeIds.TryGetValue(map.CurrentNode, out int currentNodeId))
                map.CurrentNodeId = currentNodeId;
        }

        public void Load()
        {
            if (!File.Exists(savePath))
            {
                print($"Can't find save file from {savePath}");
                return;
            }
            try
            {
               string json = File.ReadAllText(savePath);
               SaveData saveData = JsonUtility.FromJson<SaveData>(json);

               currentHealth = saveData.CurrentHealth;
               maxHealth = saveData.MaxHealth;
               gold = saveData.Gold;
               currentMap = saveData.Map;
               RestoreMapReferences(currentMap);

               if (!HasUsableMap(currentMap))
               {
                  Debug.LogWarning("Saved map has no usable node IDs orconnections. Starting a new map.");
                  currentMap = MapGenerator.GenerateAct(floorCount: 6,maxNodesPerFloor: 4);
               }
               else if (CurrentMap.IsMapCompleted())
               {
                   currentMap = MapGenerator.GenerateAct(floorCount: 6,maxNodesPerFloor: 4);
               }

               // there is no changed amount here bc these 2 lines use forupdate UI from loading data
               OnGoldChanged?.Invoke(gold, 0);
               OnHealthChanged?.Invoke((int)currentHealth, 0);

            }
               catch (Exception e)
               {
                   Debug.LogWarning($"Save file corrupted or unreadable,starting fresh: {e.Message}");
               }
           }
        

        private static void RestoreMapReferences(MapGraph map)
        {
            if (map == null) return;

            List<MapNode> nodes = GetAllNodes(map);
            var nodesById = new Dictionary<int, MapNode>();

            // Build a lookup using the canonical nodes stored in the floors.
            foreach (MapNode node in nodes)
                nodesById[node.Id] = node;

            // Rebuild each runtime connection to point at a canonical floor node.
            foreach (MapNode node in nodes)
            {
                node.ConnectedNodes = new List<MapNode>();
                if (node.ConnectedNodeIds == null) continue;

                foreach (int connectedId in node.ConnectedNodeIds)
                {
                    if (nodesById.TryGetValue(connectedId, out MapNode connectedNode))
                        node.ConnectedNodes.Add(connectedNode);
                }
            }

            // Restore CurrentNode to the matching canonical floor node.
            map.CurrentNode = null;
            if (nodesById.TryGetValue(map.CurrentNodeId, out MapNode currentNode))
                map.CurrentNode = currentNode;
        }

        private static bool HasUsableMap(MapGraph map)
        {
            if (map == null || map.Floors == null || map.Floors.Count == 0)
                return false;

            if (map.Floors[0] == null || map.Floors[0].NodesInFloor == null || map.Floors[0].NodesInFloor.Count == 0)
                return false;

            List<MapNode> nodes = GetAllNodes(map);
            var seenIds = new HashSet<int>();
            foreach (MapNode node in nodes)
            {
                // Duplicate IDs mean the save cannot identify nodes reliably.
                if (!seenIds.Add(node.Id)) return false;
            }

            // A selected node must have resolved to one of the floor nodes.
            if (map.CurrentNodeId >= 0 && map.CurrentNode == null)
                return false;

            // A multi-floor map needs at least one restored connection.
            if (map.Floors.Count == 1) return true;
            foreach (MapNode node in nodes)
            {
                if (node.ConnectedNodes.Count > 0)
                    return true;
            }

            return false;
        }

        private static List<MapNode> GetAllNodes(MapGraph map)
        {
            var nodes = new List<MapNode>();
            if (map?.Floors == null) return nodes;

            foreach (MapFloor floor in map.Floors)
            {
                if (floor?.NodesInFloor == null) continue;

                foreach (MapNode node in floor.NodesInFloor)
                {
                    if (node != null) nodes.Add(node);
                }
            }

            return nodes;
        }
    }

    [Serializable]
    public class SaveData
    {
        public float CurrentHealth;
        public float MaxHealth;
        public int Gold;
        public MapGraph Map;

        public SaveData(float currentHealth, float maxHealth, int gold, MapGraph map)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Gold = gold;
            Map = map;
        }
    }
}
