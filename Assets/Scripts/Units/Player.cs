using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
    [SerializeField] private ParticleSystem healVFX;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        PlayerEvents.OnCardPlayed += PlayerEvents_OnCardPlayed;
        PlayerEvents.OnPlayerHit += PlayerEvents_OnPlayerHit;
    }    

    private void OnDisable()
    {
        PlayerEvents.OnCardPlayed -= PlayerEvents_OnCardPlayed;
        PlayerEvents.OnPlayerHit -= PlayerEvents_OnPlayerHit;
    }

    private void PlayerEvents_OnCardPlayed(CardData cardData)
    {
        if(cardData.attackPower > 0)
        {
            Attack(cardData);
        }
        else if (cardData.healPower > 0)
        {
            Heal(cardData);
        }
    }
    private void PlayerEvents_OnPlayerHit(int damageAmount)
    {
        health.TakeDamage(damageAmount);

        if (!health.IsAlive())
        {
            animator.Play("Death");
            PlayerEvents.PlayerDeath();
        }
    }

    private void Attack(CardData cardData)
    {
        //print($"Attack with power: {cardData.attackPower}");
        StartCoroutine(PlayAttackAnimation(cardData));
    }

    private void Heal(CardData cardData)
    {
        health.Heal(cardData.healPower);
        healVFX.Play();
        PlayerEvents.PlayerHealed();
        PlayerEvents.SkillEnd();
    }

    private IEnumerator PlayAttackAnimation(CardData cardData)
    {
        Vector3 targetPosition = originalPosition + new Vector3(4, 0, 0);

        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, timeElapsed/duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        animator.Play("Attack 2");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        BossEvents.BossHit(cardData);

        timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (GameManager.Instance.IsBattleActive())
        {
            PlayerEvents.SkillEnd();
            yield return null;
        }
      
    }
}
