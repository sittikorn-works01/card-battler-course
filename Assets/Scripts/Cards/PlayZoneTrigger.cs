using System;
using UnityEngine;

namespace CardBattlerCourse.Cards
{
    public class PlayZoneTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerHand playerHand;
        [SerializeField] private BoxCollider2D playZoneCollider;

        private void OnEnable()
        {
            PlayerEvents.OnPlayerDeath += DisablePlayZone;
            EnemyEvents.OnEnemyDeath += DisablePlayZone;
        }

        private void OnDisable()
        {
            PlayerEvents.OnPlayerDeath -= DisablePlayZone;
            EnemyEvents.OnEnemyDeath -= DisablePlayZone;
        }

        public void EnablePlayZone()
        {
            playZoneCollider.enabled = true;
        }

        private void DisablePlayZone()
        {
            playZoneCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Card>(out Card card))
            {
                playerHand.PlayCard(card);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Card>(out Card card))
            {
            }
        }
    }
}