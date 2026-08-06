using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<CardData> drawPile = new();
    [SerializeField] private GameObject cardBackPrefab;

    private const float VerticalSpacing = 0.1f;

    private void OnEnable()
    {
        PlayerEvents.OnReshuffleRequested += PlayerEvents_OnReshuffleRequested;
    }

    private void OnDisable()
    {
        PlayerEvents.OnReshuffleRequested -= PlayerEvents_OnReshuffleRequested;
    }

    private void Start()
    {
        drawPile = DeckManager.Instance.GetDeck();
        Shuffle();
        UpdateDeckVisual();
    }

    private void PlayerEvents_OnReshuffleRequested(List<CardData> discardPile)
    {
        drawPile.AddRange(discardPile);
        UpdateDeckVisual();
        Shuffle();
    }

    private void OnMouseDown()
    {
        // TODO: add more condition to prevent bug when player can draw card eventhough the hand is full and no card's added but the system is still deducting action point
        if (!GameManager.Instance.IsBattleActive())
        {
            return;
        }
        else if (drawPile.Count <= 0)
        {
            print("No cards left in deck!");
            return;
        }
        
        if (TurnSystem.Instance.HasActionsLeft())
        {
            PlayerEvents.DrawCardRequested();
        }

    }

    public CardData DrawCard()
    {
        if(drawPile.Count > 0)
        {
            int topIndex = drawPile.Count - 1;
            CardData newCard = drawPile[topIndex];
            drawPile.RemoveAt(topIndex);
            UpdateDeckVisual();
            return newCard;
        }
        return null;
    }

    private void UpdateDeckVisual()
    {
        foreach (Transform card in transform)
        {
            Destroy(card.gameObject);
        }

        for (int i = 0; i < drawPile.Count; i++)
        {
            GameObject newCardBack = Instantiate(cardBackPrefab, transform);
            newCardBack.GetComponent<SpriteRenderer>().sortingOrder = i;
            newCardBack.transform.localPosition = new Vector3(0f, -i* VerticalSpacing, 0f);
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            CardData cardData = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = cardData;
        }
    }
}
