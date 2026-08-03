using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private DiscardPile discardPile;
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private int startingHandSize = 2;
    private List<Card> cardsInHand = new();

    [SerializeField] private ParticleSystem playCardVFX;
    [SerializeField] private int cardHoldTime = 300;

    private void OnEnable()
    {
        TurnEvents.OnPlayerTurnStart += PlayerEvents_OnPlayerTurnStart;
        TurnEvents.OnPlayerTurnEnd += PlayerEvents_OnPlayerTurnEnd;
        PlayerEvents.OnDrawCardRequested += PlayerEvents_OnDrawCardRequested;
        PlayerEvents.OnAttackEnd += () => EnablePlayerHand(true) ;
    }    

    private void OnDisable()
    {
        TurnEvents.OnPlayerTurnStart -= PlayerEvents_OnPlayerTurnStart;
        TurnEvents.OnPlayerTurnEnd -= PlayerEvents_OnPlayerTurnEnd;
        PlayerEvents.OnDrawCardRequested -= PlayerEvents_OnDrawCardRequested;
        PlayerEvents.OnAttackEnd -= () => EnablePlayerHand(true);
    }

    private void Start()
    {
        for (int i = 0; i < startingHandSize; i++)
        { 
            DrawNextCard(); 
        }        
    }
    private void PlayerEvents_OnDrawCardRequested()
    {
        DrawNextCard();
    }

    private void PlayerEvents_OnPlayerTurnStart()
    {
        EnablePlayerHand(true);
    }

    private void PlayerEvents_OnPlayerTurnEnd()
    {
        EnablePlayerHand(false);
    }

    private void EnablePlayerHand(bool value)
    {
        if (TurnSystem.Instance.HasActionsLeft())
        {
            foreach (Card card in cardsInHand)
            {
                card.SetInteractable(value);
            }
        }       
    }

    public void DrawNextCard()
    {
        if (cardSlots == null || cardsInHand.Count >= cardSlots.Length)
        {
            Debug.Log("Hand is full or slots are null");
            return;
        }
        CardData cardData = deck.DrawCard();
        if (cardData == null)
        {
            Debug.Log("No cards left in deck");
            return;
        }
        int slotIndex = cardsInHand.Count;
        Card newCard = Instantiate(cardPrefab, cardSlots[slotIndex].position, Quaternion.identity);
        newCard.LoadCardData(cardData);
        cardsInHand.Add(newCard);
        cardsInHand[slotIndex].transform.SetParent(cardSlots[slotIndex]);

        if (!TurnSystem.Instance.HasActionsLeft())
        {
            newCard.SetInteractable(false);
        }
    }

    public void RepositionCards()
    {
        for(int i = 0; i < cardsInHand.Count; i++)
        {
            cardsInHand[i].transform.SetParent(null);
        }

        for(int i = 0;i < cardsInHand.Count; i++)
        {
            cardsInHand[i].transform.SetParent(cardSlots[i]);
            cardsInHand[i].transform.position = cardSlots[i].position;
        }
    }

    public void PlayCard(Card card)
    {
        PlayCardWithDelay(card).Forget();
    }
    //TODO: fix bug: hands is not enable after player use non-attack card like heal, super heal card
    public async UniTask PlayCardWithDelay(Card card)
    {
        EnablePlayerHand(false);
        card.SetIsPlaying(true);
        card.Glow();
        cardsInHand.Remove(card);
        discardPile.DiscardCard(card.GetCardData());   
        PlayerEvents.CardPlayed(card.GetCardData());

        ParticleSystem cardPlayVFX = Instantiate(playCardVFX, card.transform.position, Quaternion.identity);
        Destroy(cardPlayVFX.gameObject, cardPlayVFX.main.duration);

        await UniTask.Delay(cardHoldTime);

        Destroy(card.gameObject);
        RepositionCards();
    }
}
