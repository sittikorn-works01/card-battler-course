using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
    [SerializeField] private ParticleSystem healVFX;

    public List<Buff> activeBuffs = new();

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        PlayerEvents.OnCardPlayed += PlayerEvents_OnCardPlayed;
        PlayerEvents.OnPlayerHit += PlayerEvents_OnPlayerHit;
        PlayerEvents.OnSkillEnd += RefreshActiveBuffs;
    }

    private void RefreshActiveBuffs()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
        {
            if (activeBuffs[i].UseRemaining <= 0)
            {
                activeBuffs.RemoveAt(i);
            }
        }
    }

    private void OnDisable()
    {
        PlayerEvents.OnCardPlayed -= PlayerEvents_OnCardPlayed;
        PlayerEvents.OnPlayerHit -= PlayerEvents_OnPlayerHit;
        PlayerEvents.OnSkillEnd -= RefreshActiveBuffs;
    }

    private void PlayerEvents_OnCardPlayed(CardData cardData)
    {
        if (cardData.attackPower > 0)
        {
            Attack(cardData);
        }
        else if (cardData.healPower > 0)
        {
            Heal(cardData);
        }
        else
        {
            Empower(cardData);
        }


        print(cardData.CardName);
    }

    private void Empower(CardData cardData)
    {
        Buff buff = new()
        {
            buffPower = cardData.Buff.buffPower,
            UseRemaining = cardData.Buff.UseRemaining
        }; 

        activeBuffs.Add(buff);
        PlayerEvents.SkillEnd();
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
        int attackPower = cardData.attackPower;
        foreach (Buff buff in activeBuffs)
        {
            if (buff.UseRemaining > 0)
            {
                //add attack power
                print($"currentAttackPower increase by: {buff.buffPower}");
                attackPower += buff.buffPower;
                buff.UseRemaining--;
            }
        }
        
        StartCoroutine(PlayAttackAnimation(attackPower));
    }

    private void Heal(CardData cardData)
    {
        health.Heal(cardData.healPower);
        healVFX.Play();
        PlayerEvents.PlayerHealed();
        PlayerEvents.SkillEnd();
    }

    private IEnumerator PlayAttackAnimation(int attackPower)
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

        BossEvents.BossHit(attackPower);

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
