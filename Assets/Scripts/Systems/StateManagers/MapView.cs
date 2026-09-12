using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private float nodeSpacingX = 140f;

    private readonly Dictionary<MapNode, Button> _nodeButtons = new Dictionary<MapNode, Button>();
    private readonly List<GameObject> _spawnedLines = new List<GameObject>();

    private PlayerData PlayerData => PlayerData.Instance;

    private void OnEnable()
    {
        PlayerData PlayerData = PlayerData.Instance;

        if(PlayerData.CurrentMap == null || PlayerData.CurrentMap.Floors.Count == 0)
            PlayerData.CurrentMap = MapGenerator.GenerateAct(floorCount: 3, nodesPerFloor: 3);

        BuildView(PlayerData.CurrentMap);
        mapUI.SetActive(true);
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

                nodeButtonGO.GetComponentInChildren<TextMeshProUGUI>().text = node.Type.ToString();

                Button button = nodeButtonGO.GetComponent<Button>();
                MapNode capturedNode = node; // avoid closure-over-loop-variable bug
                button.onClick.AddListener(() => OnNodeClicked(capturedNode));

                _nodeButtons[node] = button;
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
            Button button = kvp.Value;
            button.interactable = reachable.Contains(node) && !node.Visited;
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