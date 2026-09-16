using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SellCardSlot : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private TextMeshPro priceText;
    public void Initialize(CardData cardData, Action<SellCardSlot, CardData> onSelected)
    {
        card.LoadCardData(cardData);
        card.Init(selectedCardData => onSelected?.Invoke(this, selectedCardData));
        priceText.text = cardData.buyPrice.ToString();
    }

    public void SetInteractable(bool interactable) => card.SetInteractable(interactable);
}
