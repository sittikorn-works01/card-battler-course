using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapView : MonoBehaviour
{
    [Header("Prefabs & containers")]
    [SerializeField] private RectTransform nodeContainer;   // parent for spawned node buttons
    [SerializeField] private GameObject nodeButtonPrefab;    // a Button + Image + TMP label
    //[SerializeField] private LineRenderer linePrefab;        // world-space or UI line for connections

    [Header("Layout")]
    [SerializeField] private float floorSpacingY = 160f;
    [SerializeField] private float nodeSpacingX = 140f;

    private readonly Dictionary<MapNode, Button> _nodeButtons = new Dictionary<MapNode, Button>();
    private readonly List<GameObject> _spawnedLines = new List<GameObject>();

    private void OnEnable()
    {
        RunData run = GameManager.Instance.RunData;

        if(run.CurrentMap == null || run.CurrentMap.Floors.Count == 0)
            run.CurrentMap = MapGenerator.GenerateAct(floorCount: 3, nodesPerFloor: 3);

        BuildView(run.CurrentMap);
        nodeContainer.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        nodeContainer.gameObject.SetActive(false);
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
                rt.anchoredPosition = new Vector2(node.Position.x * nodeSpacingX,
                                                   node.Floor * floorSpacingY);

                nodeButtonGO.GetComponentInChildren<TextMeshProUGUI>().text = node.Type.ToString();

                Button button = nodeButtonGO.GetComponent<Button>();
                MapNode capturedNode = node; // avoid closure-over-loop-variable bug
                button.onClick.AddListener(() => OnNodeClicked(capturedNode));

                _nodeButtons[node] = button;
            }
        }

        //// 2. Draw connector lines between each node and the nodes it leads to.
        //foreach (List<MapNode> floor in map.Floors)
        //{
        //    foreach (MapNode node in floor)
        //    {
        //        foreach (MapNode next in node.ConnectedNodes)
        //        {
        //            DrawConnection(node, next);
        //        }
        //    }
        //}

        RefreshInteractability(map);
    }

    /// <summary>
    /// Core rule: you can only click a node if it's one of the
    /// CurrentNode's ConnectedNodes (or any first-floor node if the act
    /// just started). Everything else is shown but disabled.
    /// </summary>
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
        RunData run = GameManager.Instance.RunData;
        run.CurrentMap.CurrentNode = node;

        // Hand off to the state manager, which enables Battle/Shop/Event/etc.
        GameManager.Instance.EnterNode(node);
    }

    //private void DrawConnection(MapNode from, MapNode to)
    //{
    //    LineRenderer line = Instantiate(linePrefab, nodeContainer);
    //    Vector3 a = new Vector3(from.Position.x * nodeSpacingX, from.Floor * floorSpacingY, 0f);
    //    Vector3 b = new Vector3(to.Position.x * nodeSpacingX, to.Floor * floorSpacingY, 0f);
    //    line.SetPosition(0, a);
    //    line.SetPosition(1, b);
    //    _spawnedLines.Add(line.gameObject);
    //}

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