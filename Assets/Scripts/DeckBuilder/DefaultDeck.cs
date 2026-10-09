using System.Collections.Generic;
using CardBattlerCourse.Systems;
using UnityEngine;

namespace CardBattlerCourse.DeckBuilder
{
    [CreateAssetMenu(fileName = "DefaultDeck", menuName = "Scriptable Objects/DefaultDeck")]
    public class DefaultDeck : ScriptableObject
    {
        public List<CardData> cards;
    }
}
