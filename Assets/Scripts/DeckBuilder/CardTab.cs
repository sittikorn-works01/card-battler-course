using TMPro;
using UnityEngine;

public class CardTab : MonoBehaviour
{
    private CardData cardData;
    [SerializeField] private SpriteRenderer cardBase;
    [SerializeField] private TextMeshPro cardNameText;
    [SerializeField] private TextMeshPro cardActionCostText;
    [SerializeField] private SpriteRenderer illustration;

    private Color originalColor;

    private void Start()
    {
        originalColor = cardBase.color;   
    }

    public void LoadCardData(CardData cardData)
    {
        this.cardData = cardData;
        cardNameText.text = cardData.CardName;
        cardActionCostText.text = cardData.actionCost.ToString();
        illustration.sprite = cardData.Illustration;
    }

    private void OnMouseDown()
    {
        DeckEvents.RemoveCardFromDeck(cardData);
    }

    private void OnMouseEnter()
    {
        cardBase.color = Color.yellowGreen;
    }

    private void OnMouseExit()
    {
        cardBase.color = originalColor;
    }
}
