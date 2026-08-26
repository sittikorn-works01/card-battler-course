using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;


public class TurnSystem : Singleton<TurnSystem>
{
    private enum TurnState
    {
        PlayerTurn, BossTurn
    }

    private TurnState currentState = TurnState.PlayerTurn;

    [SerializeField] private int maxActionsPerTurn = 1;
    [SerializeField] private int drawCost = 1;
    [SerializeField] private int reshuffleCost = 1;
    [SerializeField] private TextMeshProUGUI remainingActionsText;
    private int remainingActions;    

    [SerializeField] private float turnWaitTime = 2f;
    [SerializeField] private float bossDelayTime = 1f;
    private const float MilliSecondMultiplier = 1000;
    [SerializeField] private TextMeshProUGUI displayTurnStateText;

    private void OnEnable()
    {
        PlayerEvents.OnCardPlayed += PlayerEvents_OnCardPlayed;
        PlayerEvents.OnDrawCardRequested += PlayerEvents_OnDrawCardRequested;
        PlayerEvents.OnReshuffleRequested += PlayerEvents_OnReshuffleRequested;

        PlayerEvents.OnPlayerDeath += ClearTurnStateDisplayText;
        EnemyEvents.OnBossDeath += ClearTurnStateDisplayText;
    }

    private void OnDisable()
    {
        PlayerEvents.OnCardPlayed -= PlayerEvents_OnCardPlayed;
        PlayerEvents.OnDrawCardRequested -= PlayerEvents_OnDrawCardRequested;
        PlayerEvents.OnReshuffleRequested -= PlayerEvents_OnReshuffleRequested;

        PlayerEvents.OnPlayerDeath -= ClearTurnStateDisplayText;
        EnemyEvents.OnBossDeath -= ClearTurnStateDisplayText;
    }

    public void Initialize()
    {
        StartPlayerTurn();
    }

    private void ClearTurnStateDisplayText()
    {
        displayTurnStateText.text = "";
    }

    private void StartPlayerTurn()
    {
        displayTurnStateText.text = "Player's Turn";
        ResetActionPoint();
        currentState = TurnState.PlayerTurn;
        TurnEvents.PlayerTurnStart();
    }

    private void EndPlayerTurn()
    {
        TurnEvents.PlayerTurnEnd();
        WaitBetweenTurns().Forget();
    }

    private async UniTaskVoid StartBossTurn()
    {
        displayTurnStateText.text = "Boss's Turn";
        currentState = TurnState.BossTurn;
        await UniTask.Delay((int)(bossDelayTime * MilliSecondMultiplier));
        BossTurn();
    }

    public void EndBossTurn()
    {
        TurnEvents.EnemyTurnEnd();
        WaitBetweenTurns().Forget();
    }

    private async UniTaskVoid WaitBetweenTurns()
    {
        float delayTime = turnWaitTime;
        while (delayTime > 0)
        {
            displayTurnStateText.text = $"{delayTime--}...";
            await UniTask.Delay((int)(1000));
        }
        

        if (GameManager.Instance.IsBattleActive())
        {
            if (currentState != TurnState.PlayerTurn)
            {
                StartPlayerTurn();
            }
            else
            {
                StartBossTurn().Forget();
            }
        }
        
    }

    private void PlayerEvents_OnCardPlayed(CardData cardData)
    {
        ConsumeAction(cardData.actionCost);
    }
    private void PlayerEvents_OnDrawCardRequested()
    {
        ConsumeAction(drawCost);
    }
    private void PlayerEvents_OnReshuffleRequested(List<CardData> discardPile)
    {
        ConsumeAction(reshuffleCost);
    }

    private void ConsumeAction(int amount)
    {
        remainingActions -= amount;
        if (remainingActions <= 0)
        {
            EndPlayerTurn();
        }
        UpdateActionsUI();
    }

    public bool HasActionsLeft()
    {
        return remainingActions > 0;
    }

    private void UpdateActionsUI()
    {
        if(remainingActions < 0)
        {
            remainingActions = 0;
        }
        remainingActionsText.text = $"Remaining Actions: {remainingActions}";
    }

    private void ResetActionPoint()
    {
        remainingActions = maxActionsPerTurn;
        UpdateActionsUI();
    }

    private void BossTurn()
    {
        TurnEvents.EnemyTurnStart();
        //EndBossTurn().Forget();
        
    }
}
