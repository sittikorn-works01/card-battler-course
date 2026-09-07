using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum TextType
{
    Heal, Damage
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private MapView mapView;

    [SerializeField] private GameState startState;
    [SerializeField] private PlayerData runData;

    [SerializeField] private TextMeshPro textPopup;

    public PlayerData GetRunData => runData;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

   

    protected override void Awake()
    {
        base.Awake();

        if (runData == null)
            runData = new PlayerData();
    }

    private void Start()
    {
        EnterState(startState);
    }

    public void ShowTextPopup(TextType textType, string text, Vector2 position)
    {
        TextMeshPro spawnedText = Instantiate(textPopup, position, Quaternion.identity);
        switch (textType)
        {
            case TextType.Heal:
                spawnedText.color = Color.green;
                break;

            case TextType.Damage:
                spawnedText.color = Color.red;
                break;
        }
        spawnedText.text = text;
        Destroy(spawnedText.gameObject, 3f);
    }

    public void EnterState(GameState newState)
    {
        PlayerData.Instance.Save();

        battleManager.gameObject.SetActive(false);
        shopManager.gameObject.SetActive(false);
        mapView.gameObject.SetActive(false);

        CurrentState = newState;

        switch (newState)
        {
            case GameState.EliteBattle:
                battleManager.gameObject.SetActive(true);
                battleManager.SetupBattle(GameState.EliteBattle);
                break;
            case GameState.BossBattle:
                battleManager.gameObject.SetActive(true);
                battleManager.SetupBattle(GameState.BossBattle);
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
            case NodeType.Elite: EnterState(GameState.EliteBattle); break;
            case NodeType.Boss: EnterState(GameState.BossBattle); break;
            case NodeType.Shop: EnterState(GameState.Shop); break;
        }
    }

    public bool IsBattleActive() => battleManager.IsBattleActive();
    public void ReturnToMap() => EnterState(GameState.Map);


}
