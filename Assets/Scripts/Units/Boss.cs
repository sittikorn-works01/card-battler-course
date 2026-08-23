using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;

    private int attackPower = 3;

    private Vector3 originalPosition;

    private void OnEnable()
    {
        BossEvents.OnBossHit += BossEvents_OnBossHit;
        TurnEvents.OnBossTurnStart += TurnEvents_OnBossTurnStart;
    }

    private void OnDisable()
    {
        BossEvents.OnBossHit -= BossEvents_OnBossHit;
        TurnEvents.OnBossTurnStart -= TurnEvents_OnBossTurnStart;
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void TurnEvents_OnBossTurnStart()
    {
        //print("Boss starting");
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        Vector3 targetPosition = originalPosition + new Vector3(-4, 0, 0);

        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        animator.Play("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PlayerEvents.PlayerHit(attackPower);

        timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private void BossEvents_OnBossHit(CardData cardData)
    {
        //print($"Boss is hit with card: {cardData.CardName}");
        health.TakeDamage(cardData.attackPower);

        if(!health.IsAlive())
        {
            animator.Play("Death");
            BossEvents.BossDeath();
        }
    }
}
