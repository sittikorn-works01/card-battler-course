using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] private List<CardData> discardPile = new List<CardData>();
    [SerializeField] private Card cardPrefab;
    private const float VerticalSpacing = 0.1f;

    [SerializeField] private Deck deck;

    public void DiscardCard(CardData cardData)
    {
        discardPile.Add(cardData);
        Card card = Instantiate(cardPrefab, transform);
        card.transform.localPosition = new Vector3(0, -VerticalSpacing * (discardPile.Count - 1), 0);
        card.LoadCardData(cardData);
        card.SetInteractable(false);

        SortingGroup sortingGroup = card.gameObject.GetComponent<SortingGroup>();
        sortingGroup.sortingOrder = discardPile.Count - 1;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsBattleActive())
        {
            if (discardPile.Count == 0)
            {
                print("You have no card in the discard pile");
                return;
            }

            if (TurnSystem.Instance.HasActionsLeft())
            {
                PlayerEvents.ReshuffleRequested(discardPile);
                ClearDiscardPile();
            }
        }        
    }

    private void ClearDiscardPile()
    {
        discardPile.Clear();
        foreach(Transform discardedCard in transform)
        {
            Destroy(discardedCard.gameObject);
        }
    }

    public void Dispose()
    {
        ClearDiscardPile();
    }
}
