using UnityEngine;

public class DeckZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Card>(out Card card))
        {
            print($"{card.GetCardData().CardName} was added to your deck");
            DeckEvents.AddCardFromDeck(card.GetCardData());
        }
    }
}
