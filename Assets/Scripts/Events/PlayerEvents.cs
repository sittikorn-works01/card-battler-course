using System;
using System.Collections.Generic;

public static class PlayerEvents
{
    public static event Action<CardData> OnCardPlayed;
    public static event Action<int> OnPlayerHit;
    public static event Action OnPlayerDeath;
    public static event Action OnDrawCardRequested;
    public static event Action<List<CardData>> OnReshuffleRequested;
    public static event Action OnPlayerHealed;
    public static event Action OnAttackEnd;

    public static void CardPlayed(CardData cardData)
    {
        OnCardPlayed?.Invoke(cardData);
    }

    public static void PlayerHit(int damageAmount)
    {
        OnPlayerHit?.Invoke(damageAmount);
    }

    public static void DrawCardRequested()
    {
        OnDrawCardRequested?.Invoke();  
    }

    public static void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void ReshuffleRequested(List<CardData> discardPile)
    {
        OnReshuffleRequested?.Invoke(discardPile);
    }

    public static void PlayerHealed()
    {
        OnPlayerHealed?.Invoke();
    }
    public static void AttackEnd()
    {
        OnAttackEnd?.Invoke();
    }
}
