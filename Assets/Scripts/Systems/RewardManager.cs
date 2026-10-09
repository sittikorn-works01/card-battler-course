using CardBattlerCourse.Cards;
using System.Collections.Generic;
using CardBattlerCourse.UI;
using UnityEngine;

namespace CardBattlerCourse.Systems
{
    public class RewardManager : Singleton<RewardManager>
    {
        [SerializeField] private Card[] blankDropCards;
        [SerializeField] private List<CardData> rewardPool;
        [SerializeField] private ResultPanel resultPanel;

        public void SpawnRewardCards()
        {
            foreach (Card dropCard in blankDropCards)
            {
                dropCard.Init(OnSelectedCard);
                CardData newCardDrop = rewardPool[Random.Range(0, rewardPool.Count)];
                dropCard.LoadCardData(newCardDrop);
                dropCard.gameObject.SetActive(true);
            }

        }

        public void OnSelectedCard(CardData chosenCard)
        {
            print($"{chosenCard.CardName} has been added");
            DeckEvents.AddCardToDeck(chosenCard);

            foreach (Card dropCard in blankDropCards)
            {
                dropCard.SetInteractable(false);
            }

            resultPanel.OnSelectRewardCard();
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
}