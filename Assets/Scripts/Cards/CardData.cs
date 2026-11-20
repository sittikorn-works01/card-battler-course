using UnityEngine;

namespace CardBattlerCourse.Systems
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        public string Id;
        public string CardName;
        public string CardDescription;
        public CardType Type;
        public int actionCost;
        public Sprite Illustration;
        public int power;
        public int buffUses;
        public int buyPrice;
    }
}
