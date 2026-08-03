using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string CardName;
    public string CardDescription;
    public int actionCost;
    public Sprite Illustration;
    public int attackPower;
    public int healPower;
}
