using System;
using UnityEngine;

public class PlayZoneTrigger : MonoBehaviour
{
    [SerializeField] private PlayerHand playerHand;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += DisablePlayZone;
        BossEvents.OnBossDeath += DisablePlayZone;
    }


    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= DisablePlayZone;
        BossEvents.OnBossDeath -= DisablePlayZone;
    }

    private void DisablePlayZone()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Card>(out Card card))
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
