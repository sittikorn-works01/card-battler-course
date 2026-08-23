using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private MapView mapView;

    [SerializeField] private GameState startState;
    [SerializeField] private RunData runData;

    public RunData RunData => runData;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    protected override void Awake()
    {
        base.Awake();

        if (runData == null)
            runData = new RunData();
    }

    private void Start()
    {
        EnterState(startState);
    }

    public void EnterState(GameState newState)
    {
        battleManager.gameObject.SetActive(false);
        shopManager.gameObject.SetActive(false);
        mapView.gameObject.SetActive(false);

        CurrentState = newState;
        print($"Enter {newState} State");

        switch (newState)
        {
            case GameState.Battle:
                battleManager.gameObject.SetActive(true);
                break;

            case GameState.Shop:
                shopManager.gameObject.SetActive(true);
                break;
            case GameState.Map:
                mapView.gameObject.SetActive(true);
                break;
        }

        OnStateChanged?.Invoke(newState);
    }

    public void EnterNode(MapNode node)
    {
        node.Visited = true;

        switch (node.Type)
        {
            case NodeType.Battle: EnterState(GameState.Battle); break;
            case NodeType.Shop: EnterState(GameState.Shop); break;
        }
    }

    public bool IsBattleActive() => battleManager.IsBattleActive();
    public void ReturnToMap() => EnterState(GameState.Map);


}
