using System.Collections.Generic;
using CardBattlerCourse.Cards;
using CardBattlerCourse.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace CardBattlerCourse.DeckBuilder
{
    public class DeckBuilder : MonoBehaviour
    {
        [SerializeField] private List<CardData> availableCardList = new();
        [SerializeField] private Transform[] cardSlots;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Button exitButtton;

        private void Start()
        {
            for(int i = 0; i < availableCardList.Count; i++)
            {
                AddCardToCollection(i);
            }        
        }

        private void OnEnable()
        {
            exitButtton.onClick.AddListener(ExitRest);
        }

        private void ExitRest()
        {
            gameObject.SetActive(false);
            GameManager.Instance.EnterState(GameState.Map);
            exitButtton.onClick.RemoveAllListeners();
        }

        private void AddCardToCollection(int index)
        {
            Card card = Instantiate(cardPrefab, cardSlots[index], false);
            card.gameObject.transform.localScale = Vector3.one;
            card.LoadCardData(availableCardList[index]);

        }
    }
}
