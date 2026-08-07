using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BattleManager BattleManager;
    [SerializeField] private ShopManager shopManager;

    [SerializeField] private GameState startState;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Start()
    {
        EnterState(startState);
    }

    public void EnterState(GameState newState)
    {
        BattleManager.gameObject.SetActive(false);
        shopManager.gameObject.SetActive(false);

        CurrentState = newState;
        print($"Enter {newState} State");

        switch (newState)
        {
            case GameState.Battle:
                BattleManager.gameObject.SetActive(true);
                break;

            case GameState.Shop:
                shopManager.gameObject.SetActive(true);
                break;
        }

        OnStateChanged?.Invoke(newState);
    }

    public void EnterNode(MapNode node)
    {
        //node.Visited = true;

        switch (node.Type)
        {
            case NodeType.Battle: EnterState(GameState.Battle); break;
            case NodeType.Shop: EnterState(GameState.Shop); break;
        }
    }

    public bool IsBattleActive() => BattleManager.IsBattleActive();
    //public void ReturnToMap() => EnterState(GameState.Map);


}
