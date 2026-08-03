using UnityEngine;
using System.Collections.Generic;
using System;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private CardTab cardTabPrefab;
    private List<CardTab> cardTabList = new List<CardTab>();    

    private void OnEnable()
    {
        DeckEvents.OnDeckProcessed += DeckEvents_OnDeckProcessed;
    }

    private void OnDisable()
    {
        DeckEvents.OnDeckProcessed -= DeckEvents_OnDeckProcessed;
    }

    private void Start()
    {
        UpdateCardTabVisual();
    }

    private void DeckEvents_OnDeckProcessed()
    {
        UpdateCardTabVisual();
    }

    private void UpdateCardTabVisual()
    {
        foreach (CardTab cardTab in cardTabList)
        {            
            Destroy(cardTab.gameObject);
        }

        cardTabList.Clear();
        List<CardData> deck = DeckManager.Instance.GetDeck();

        for(int i = 0; i < deck.Count; i++)
        {
            CardTab cardTab = Instantiate(cardTabPrefab, transform);
            cardTab.LoadCardData(deck[i]);
            cardTabList.Add(cardTab);   
        }
    }

}
