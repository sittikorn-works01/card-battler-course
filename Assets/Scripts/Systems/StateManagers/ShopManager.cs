using System.Collections.Generic;
using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private Card[] blankDropCards;
    [SerializeField] private List<CardData> rewardPool;

    private float cardPrice = 1;

    private void OnEnable()
    {
        shopCanvas.SetActive(true);
        SpawnRewardCards();
    }

    private void OnDisable()
    {
        shopCanvas.SetActive(false);
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
        //check if player can afford it
        print($"{chosenCard.CardName} has been added");
        DeckEvents.AddCardToDeck(chosenCard);

        foreach (Card dropCard in blankDropCards)
        {
            dropCard.SetInteractable(false);
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
