using CardBattlerCourse.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace CardBattlerCourse.Systems
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject shopUI;
        [SerializeField] private List<CardData> sellCardPool;
        [SerializeField] private SellCardSlot[] sellCardSlots;

        private void OnEnable()
        {
            shopUI.SetActive(true);
            PopulateSellSlots();
        }

        private void OnDisable()
        {
            shopUI.SetActive(false);
            HideSellSlots();
        }

        public void PopulateSellSlots()
        {
            foreach (SellCardSlot sellCardSlot in sellCardSlots)
            {
                CardData cardForSale = sellCardPool[UnityEngine.Random.Range(0, sellCardPool.Count)];
                sellCardSlot.gameObject.SetActive(true);
                sellCardSlot.Initialize(cardForSale, OnSelectedCard);
            }
        }

        public void OnSelectedCard(SellCardSlot slot, CardData chosenCard)
        {
            if (PlayerData.Instance.TrySpendingGold(chosenCard.buyPrice))
            {
                print($"{chosenCard.CardName} has been added");
                DeckEvents.AddCardToDeck(chosenCard);
                print($"Player has gold left: {PlayerData.Instance.Gold}");

                slot.SetInteractable(false);
            }
        }

        public void HideSellSlots()
        {
            foreach (SellCardSlot sellCardSlot in sellCardSlots)
            {
                sellCardSlot.SetInteractable(true);
                sellCardSlot.gameObject.SetActive(false);
            }
        }

    }
}