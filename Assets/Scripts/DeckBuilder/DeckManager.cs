using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    private List<CardData> currentDeck = new();
    [SerializeField] private int maxDeckSize = 9;
    [SerializeField] private DefaultDeck defaultDeck;
 
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        DeckEvents.OnAddCardToDeck += DeckEvents_OnAddCardToDeck;
        DeckEvents.OnRemoveCardFromDeck += DeckEvents_OnRemoveCardFromDeck;
    }

    private void OnDisable()
    {
        DeckEvents.OnAddCardToDeck -= DeckEvents_OnAddCardToDeck;
        DeckEvents.OnRemoveCardFromDeck -= DeckEvents_OnRemoveCardFromDeck;
    }

    private void Start()
    {
        currentDeck = new List<CardData>(defaultDeck.cards);
    }

    private void DeckEvents_OnRemoveCardFromDeck(CardData cardData)
    {
        currentDeck.Remove(cardData);
        DeckEvents.DeckProcessed();
    }

    private void DeckEvents_OnAddCardToDeck(CardData cardData)
    {
        if (currentDeck.Count >= maxDeckSize)
        {
            print("Deck is full!");
            return;
        }
        currentDeck.Add(cardData);
        DeckEvents.DeckProcessed();
    }

    public List<CardData> GetDeck() => new List<CardData>(currentDeck);
}
