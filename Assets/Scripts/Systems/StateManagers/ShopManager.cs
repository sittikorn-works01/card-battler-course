using System.Collections.Generic;
using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Card[] blankDropCards;
    [SerializeField] private List<CardData> rewardPool;

    private int cardPrice = 2;

    private void OnEnable()
    {
        shopUI.SetActive(true);
        SpawnRewardCards();
    }

    private void OnDisable()
    {
        shopUI.SetActive(false);
        HideDropCards();
    }

    public void SpawnRewardCards()
    {
        foreach (Card dropCard in blankDropCards)
        {
            dropCard.Init(OnSelectedCard);
            CardData newCardDrop = rewardPool[UnityEngine.Random.Range(0, rewardPool.Count)];
            dropCard.LoadCardData(newCardDrop);
            dropCard.gameObject.SetActive(true);
        }

    }

    public void OnSelectedCard(CardData chosenCard)
    {
        if(PlayerData.Instance.TrySpendingGold(cardPrice))
        {
            print($"{chosenCard.CardName} has been added");
            DeckEvents.AddCardToDeck(chosenCard);
            print($"Player has gold left: {PlayerData.Instance.Gold}");

            foreach (Card dropCard in blankDropCards)
            {
                dropCard.SetInteractable(false);
            }
        }        
    }

    public void HideDropCards()
    {
        foreach (Card dropCard in blankDropCards)
        {
            dropCard.SetInteractable(true);
            dropCard.gameObject.SetActive(false);
        }
    }

}
