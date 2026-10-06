using System;
using TMPro;
using UnityEngine;

public class BattlePanel : BasePanel
{
    private enum TurnState
    {
        None,
        Player,
        Enemy
    }

    [SerializeField] private TextMeshProUGUI remainingActionText;
    [SerializeField] private TextMeshProUGUI displayTurnStateText;

    private void OnEnable()
    {
        TurnEvents.OnActionPointChanged += UpdateRemainingActionUI;
        TurnEvents.OnPlayerTurnStart += () => UpdateDisplayTurnStateText(TurnState.Player);
        TurnEvents.OnEnemyTurnStart += () => UpdateDisplayTurnStateText(TurnState.Enemy);
        PlayerEvents.OnPlayerDeath += () => UpdateDisplayTurnStateText(TurnState.None);
        EnemyEvents.OnEnemyDeath += () => UpdateDisplayTurnStateText(TurnState.None);
    }

    private void OnDisable()
    {
        TurnEvents.OnActionPointChanged -= UpdateRemainingActionUI;
        TurnEvents.OnPlayerTurnStart -= () => UpdateDisplayTurnStateText(TurnState.Player);
        TurnEvents.OnEnemyTurnStart -= () => UpdateDisplayTurnStateText(TurnState.Enemy);
        PlayerEvents.OnPlayerDeath -= () => UpdateDisplayTurnStateText(TurnState.None);
        EnemyEvents.OnEnemyDeath -= () => UpdateDisplayTurnStateText(TurnState.None);
    }

    private void UpdateDisplayTurnStateText(TurnState state)
    {
        switch (state)
        {
            default:
            case TurnState.Player:
                displayTurnStateText.text = "Player's Turn";
                break;
            case TurnState.Enemy:
                displayTurnStateText.text = "Enemy's Turn";
                break;
            case TurnState.None:
                displayTurnStateText.text = "";
                break;
        }
    }

    private void UpdateRemainingActionUI(int remainingActions)
    {
        remainingActionText.text = remainingActions.ToString();
    }
}
