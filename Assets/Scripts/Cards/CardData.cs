using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string CardName;
    public string CardDescription;
    public CardType Type;
    public int actionCost;
    public Sprite Illustration;
    // Meaning depends on Type: damage dealt for Attack, HP restored for Heal.
    public int power;
    public Buff Buff;
    public int buyPrice;
}
