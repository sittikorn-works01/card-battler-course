using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;

    private float maxHealth = 1;
    private float currentHealth = 1;

    protected Vector3 originalPosition;

    protected virtual void OnEnable()
    {
        EnemyEvents.OnEnemyHit += OnHit;
        TurnEvents.OnEnemyTurnStart += OnTurnStart;
    }

    protected virtual void OnDisable()
    {
        EnemyEvents.OnEnemyHit -= OnHit;
        TurnEvents.OnEnemyTurnStart -= OnTurnStart;
    }

    protected virtual void Start()
    {
        originalPosition = transform.position;
        health.Init(currentHealth, maxHealth);
    }

    private void OnTurnStart()
    {
        DecideActions();
    }

    // Each enemy type decides what happens on its turn: attack pattern, buffs, etc.
    protected abstract void DecideActions();

    protected void EndTurn() => TurnSystem.Instance.EndEnemyTurn();

    protected virtual void OnHit(int damage)
    {
        health.TakeDamage(damage);
        GameManager.Instance.ShowTextPopup(TextType.Damage, damage.ToString(), transform.position);

        if (!health.IsAlive())
        {
            animator.Play("Death");
            EnemyEvents.EnemyDeath();
        }
    }

    // Shared "lunge in, play attack anim, deal damage, lunge back" choreography
    // so subclasses reuse the same tween instead of re-implementing it per enemy.
    protected IEnumerator MoveAndAttack(string attackAnimation, Action onImpact, float lungeDistance = 4f, float duration = 0.5f)
    {
        Vector3 targetPosition = originalPosition + new Vector3(-lungeDistance, 0, 0);
        yield return MoveTo(targetPosition, duration);

        animator.Play(attackAnimation);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        onImpact?.Invoke();

        yield return MoveTo(originalPosition, duration);
    }

    private IEnumerator MoveTo(Vector3 destination, float duration)
    {
        Vector3 start = transform.position;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(start, destination, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }
}
