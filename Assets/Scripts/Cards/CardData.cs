using UnityEngine;

namespace CardBattlerCourse.Systems
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        public string CardName;
        public string CardDescription;
        public CardType Type;
        public int actionCost;
        public Sprite Illustration;
        // Meaning depends on Type: damage for Attack, HP restored for Heal,
        // block for Shield, per-use attack bonus for Buff.
        public int power;
        // Buff only: how many attacks the bonus lasts for.
        public int buffUses;
        public int buyPrice;
    }
}