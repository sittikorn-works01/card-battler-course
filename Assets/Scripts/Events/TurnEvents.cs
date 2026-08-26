using System;
using UnityEngine;

public static class TurnEvents
{
    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;
    public static event Action OnEnemyTurnStart;
    public static event Action OnEnemyTurnEnd;

    public static void PlayerTurnStart()
    {
        OnPlayerTurnStart?.Invoke();
    }

    public static void PlayerTurnEnd()
    {
        OnPlayerTurnEnd?.Invoke();
    }

    public static void EnemyTurnStart()
    {
        OnEnemyTurnStart?.Invoke();
    }

    public static void EnemyTurnEnd()
    {
        OnEnemyTurnEnd?.Invoke();
    }
}
