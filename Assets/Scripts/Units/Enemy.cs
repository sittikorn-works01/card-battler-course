using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyIntentUI intentUI;
    [SerializeField] protected EnemyData enemyData;

    protected Vector3 originalPosition;
    private EnemyIntent nextMove;

    protected virtual void OnEnable()
    {
        EnemyEvents.OnEnemyHit += OnHit;
        TurnEvents.OnEnemyTurnStart += OnTurnStart;
        TurnEvents.OnPlayerTurnStart += OnPlayerTurnStart;
    }

    protected virtual void OnDisable()
    {
        EnemyEvents.OnEnemyHit -= OnHit;
        TurnEvents.OnEnemyTurnStart -= OnTurnStart;
        TurnEvents.OnPlayerTurnStart -= OnPlayerTurnStart;
    }

    protected virtual void Start()
    {
        originalPosition = transform.position;
        health.Init(enemyData.maxHealth, enemyData.maxHealth);

        // The player's first turn already started before this enemy was
        // instantiated (see BattleManager.SetupBattle), so it never received
        // that event. Show an intent right away instead of waiting for the
        // next one.
        ShowNextMove();
    }

    private void OnPlayerTurnStart()
    {
        ShowNextMove();
    }

    private void ShowNextMove()
    {
        nextMove = ChooseNextMove();
        intentUI.SetIntent(nextMove);
    }

    private void OnTurnStart()
    {
        // Block only lasts until the guarding enemy acts again.
        health.ClearBlock();
        PerformMove(nextMove);
    }

    // Each enemy type decides what its next move will be, without acting on
    // it yet, so the intent can be telegraphed to the player one turn ahead.
    protected abstract EnemyIntent ChooseNextMove();

    // Executes the move that was previously telegraphed via ChooseNextMove.
    protected abstract void PerformMove(EnemyIntent intent);

    protected void EndTurn() => TurnSystem.Instance.EndEnemyTurn();

    protected void ApplyBlock(int amount) => health.AddBlock(amount);

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
