using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    [Header("Icon Sprites")]
    [SerializeField] private Sprite eliteIcon;
    [SerializeField] private Sprite bossIcon;
    [SerializeField] private Sprite shopIcon;
    [SerializeField] private Sprite restIcon;

    private Dictionary<NodeType, Sprite> nodeIconsDict;

    private void Awake()
    {
        nodeIconsDict = new()
        {
            [NodeType.Elite] = eliteIcon,
            [NodeType.Boss] = bossIcon,
            [NodeType.Shop] = shopIcon,
            [NodeType.Rest] = restIcon,
        };
    }

    public void Initialize(NodeType type, Action onClick)
    {
        if (nodeIconsDict.TryGetValue(type, out Sprite sprite))
            icon.sprite = sprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke());
    }

    public void SetInteractable(bool interactable) => button.interactable = interactable;
}
