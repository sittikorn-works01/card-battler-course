using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private List<CardData> availableCardList = new();
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private Card cardPrefab;

    private void Start()
    {
        for(int i = 0; i < availableCardList.Count; i++)
        {
            AddCardToCollection(i);
        }
    }

    private void AddCardToCollection(int index)
    {
        //TODO: fix bug that the heal card's local scale become 0, I think the cause is from where I try to instantiate a normal gameobject in a recttransform parent object, try removing recttransform for parent so you'll need to remove the old layout and implement normal manual arrangement instead; make parent a normal gameobject instead of some shit recttransform and normally instantiate card as its child
        Card card = Instantiate(cardPrefab, cardSlots[index], false);
        card.gameObject.transform.localScale = Vector3.one;
        card.LoadCardData(availableCardList[index]);

    }
}
