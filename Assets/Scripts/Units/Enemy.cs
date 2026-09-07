using UnityEngine;
using System.Collections;

public enum EnemyBehavior
{
    Attack, Empower
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator empowerVFX;

    private int originalATK = 3;
    private int attackPower;
    private int empower = 2;

    private Vector3 originalPosition;

    private void OnEnable()
    {
        EnemyEvents.OnBossHit += BossEvents_OnEnemyHit;
        TurnEvents.OnEnemyTurnStart += TurnEvents_OnEnemyTurnStart;
    }

    private void OnDisable()
    {
        EnemyEvents.OnBossHit -= BossEvents_OnEnemyHit;
        TurnEvents.OnEnemyTurnStart -= TurnEvents_OnEnemyTurnStart;
    }

    private void Start()
    {
        originalPosition = transform.position;
        attackPower = originalATK;
    }

    private void TurnEvents_OnEnemyTurnStart()
    {
        DecideActions();
    }

    private void DecideActions()
    {
        if(attackPower >= originalATK + empower)
        {
            StartCoroutine(Attack());
        }
        else
        {
            RandomEnemyAction();
        }
    }

    private void RandomEnemyAction()
    {
        if (Random.Range(0, 2) > 0)
        {
            StartCoroutine(Attack());
        }
        else
        {
            Empower();
        }
    }

    #region Skills
    private void Empower()
    {
        Dev.Log();
        attackPower += empower;
        TurnSystem.Instance.EndBossTurn();
        empowerVFX.Play("Empower");
    }

    private IEnumerator Attack()
    {
        Dev.Log();
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

        attackPower = originalATK;
        TurnSystem.Instance.EndBossTurn();
        yield return null;
    }
    #endregion

    private void BossEvents_OnEnemyHit(int damage)
    {
        print($"Boss received {damage} damage!");
        health.TakeDamage(damage);

        GameManager.Instance.ShowTextPopup(TextType.Damage, damage.ToString(), transform.position);

        if (!health.IsAlive())
        {
            animator.Play("Death");
            EnemyEvents.BossDeath();
        }
    }
}
