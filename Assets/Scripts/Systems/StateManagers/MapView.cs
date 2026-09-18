using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [Header("Prefabs & containers")]
    [SerializeField] private GameObject mapUI;
    [SerializeField] private RectTransform nodeContainer;   // parent for spawned node buttons
    [SerializeField] private RectTransform lineContainer;  
    [SerializeField] private GameObject nodeButtonPrefab;    // a Button + Image + TMP label
    [SerializeField] private Image linePrefab;        // world-space or UI line for connections

    [Header("Layout")]
    [SerializeField] private float floorSpacingY = 160f;
    [SerializeField] private float nodeSpacingX = 160f;

    private readonly Dictionary<MapNode, MapNodeButton> _nodeButtons = new Dictionary<MapNode, MapNodeButton>();
    private readonly List<GameObject> _spawnedLines = new List<GameObject>();

    private PlayerData PlayerData => PlayerData.Instance;

    private void OnEnable()
    {
        PlayerData PlayerData = PlayerData.Instance;

        if(PlayerData.CurrentMap == null || PlayerData.CurrentMap.Floors.Count == 0)
            PlayerData.CurrentMap = MapGenerator.GenerateAct(floorCount: 6, maxNodesPerFloor: 4);

        // Must activate mapUI before instantiating node buttons under it -
        // Awake() on a newly instantiated child is deferred until its
        // hierarchy is active, so building the view first left
        // MapNodeButton.nodeIconsDict null on every re-entry after the
        // first (mapUI starts active by default, but OnDisable turns it
        // off, so this only broke once you left and came back).
        mapUI.SetActive(true);
        BuildView(PlayerData.CurrentMap);
    }

    private void OnDisable()
    {
        mapUI.SetActive(false);
    }

    private void BuildView(MapGraph map)
    {
        Clear();

        foreach (List<MapNode> floor in map.Floors)
        {
            foreach (MapNode node in floor)
            {
                GameObject nodeButtonGO = Instantiate(nodeButtonPrefab, nodeContainer);
                RectTransform rt = nodeButtonGO.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(node.Position.x * nodeSpacingX, node.Floor * floorSpacingY);

                MapNodeButton nodeButton = nodeButtonGO.GetComponent<MapNodeButton>();
                MapNode capturedNode = node; // avoid closure-over-loop-variable bug
                print(node.Type);
                nodeButton.Initialize(node.Type, () => OnNodeClicked(capturedNode));

                _nodeButtons[node] = nodeButton;
            }
        }

        //// 2. Draw connector lines between each node and the nodes it leads to.
        foreach (List<MapNode> floor in map.Floors)
        {
            foreach (MapNode node in floor)
            {
                foreach (MapNode next in node.ConnectedNodes)
                {
                    DrawConnection(node, next);
                }
            }
        }

        RefreshInteractability(map);
    }

    private void RefreshInteractability(MapGraph map)
    {
        HashSet<MapNode> reachable = new HashSet<MapNode>();

        if (map.CurrentNode == null)
        {
            foreach (MapNode n in map.Floors[0])
                reachable.Add(n);
        }
        else
        {
            foreach (MapNode n in map.CurrentNode.ConnectedNodes)
                reachable.Add(n);
        }

        foreach (var kvp in _nodeButtons)
        {
            MapNode node = kvp.Key;
            MapNodeButton nodeButton = kvp.Value;
            nodeButton.SetInteractable(reachable.Contains(node) && !node.Visited);
        }
    }

    private void OnNodeClicked(MapNode node)
    {
        PlayerData.CurrentMap.CurrentNode = node;

        GameManager.Instance.EnterNode(node);
    }

    private void DrawConnection(MapNode from, MapNode to)
    {
        Image line = Instantiate(linePrefab, lineContainer);
        Vector3 posA = new Vector3(from.Position.x * nodeSpacingX, from.Floor * floorSpacingY, 0f);
        Vector3 posB = new Vector3(to.Position.x * nodeSpacingX, to.Floor * floorSpacingY, 0f);

        line.rectTransform.anchoredPosition = posA;
        Vector3 difference = posB - posA;
        float distance = difference.magnitude;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        line.rectTransform.sizeDelta = new Vector2(distance, 6);
        line.rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        _spawnedLines.Add(line.gameObject);
    }

    private void Clear()
    {
        foreach (var kvp in _nodeButtons)
            Destroy(kvp.Value.gameObject);
        _nodeButtons.Clear();

        foreach (GameObject line in _spawnedLines)
            Destroy(line);
        _spawnedLines.Clear();
    }
}