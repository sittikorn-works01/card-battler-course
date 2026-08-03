using System;

public static class DeckEvents
{
    public static event Action<CardData> OnRemoveCardFromDeck;
    public static event Action<CardData> OnAddCardFromDeck;
    public static event Action OnDeckProcessed;

    public static void AddCardFromDeck(CardData cardData)
    {
        OnAddCardFromDeck?.Invoke(cardData);
    }

    public static void RemoveCardFromDeck(CardData cardData)
    {
        OnRemoveCardFromDeck?.Invoke(cardData);
    }

    public static void DeckProcessed()
    {
        OnDeckProcessed?.Invoke();
    }
}
