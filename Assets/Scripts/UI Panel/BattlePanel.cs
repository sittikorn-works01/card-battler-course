using DG.Tweening;
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
    [SerializeField] private AnimatedTextUI displayTurnStateUI;

    private void OnEnable()
    {
        TurnEvents.OnActionPointChanged += UpdateRemainingActionUI;
        TurnEvents.OnPlayerTurnEnd += () => UpdateDisplayTurnStateText(TurnState.Enemy);
        TurnEvents.OnEnemyTurnEnd += () => UpdateDisplayTurnStateText(TurnState.Player);
    }

    private void OnDisable()
    {
        TurnEvents.OnActionPointChanged -= UpdateRemainingActionUI;
        TurnEvents.OnPlayerTurnEnd -= () => UpdateDisplayTurnStateText(TurnState.Enemy);
        TurnEvents.OnEnemyTurnEnd -= () => UpdateDisplayTurnStateText(TurnState.Player);
    }

    private void Start()
    {
        //UpdateDisplayTurnStateText(TurnState.Player);
    }

    private void UpdateDisplayTurnStateText(TurnState state)
    {
        switch (state)
        {
            default:
            case TurnState.Player:
                displayTurnStateUI.SetText("Player's Turn");
                break;
            case TurnState.Enemy:
                displayTurnStateUI.SetText("Enemy's Turn");
                break;            
        }
        displayTurnStateUI.Play();
    }

    private void UpdateRemainingActionUI(int remainingActions)
    {
        remainingActionText.SetText(remainingActions.ToString());
    }
}
