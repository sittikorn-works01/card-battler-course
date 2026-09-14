using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator empowerVFX;

    // Fixed move pattern, cycled through in order instead of rolled randomly.
    private readonly List<IntentType> movePattern = new()
    {
        IntentType.Buff, IntentType.Attack, IntentType.Attack
    };
    private int moveIndex;

    private int attackPower;

    protected override void Start()
    {
        // Set before base.Start(), which immediately telegraphs the first
        // move via ChooseNextMove() and needs attackPower to be valid.
        attackPower = enemyData.attackPower;
        base.Start();
    }

    protected override EnemyIntent ChooseNextMove()
    {
        IntentType nextType = movePattern[moveIndex];
        moveIndex = (moveIndex + 1) % movePattern.Count;

        return nextType == IntentType.Attack
            ? new EnemyIntent(IntentType.Attack, attackPower)
            : new EnemyIntent(IntentType.Buff, enemyData.empowerAmount);
    }

    protected override void PerformMove(EnemyIntent intent)
    {
        if (intent.Type == IntentType.Attack)
        {
            StartCoroutine(Attack());
        }
        else
        {
            Empower();
        }
    }

    private void Empower()
    {
        Dev.Log();
        attackPower += enemyData.empowerAmount;
        empowerVFX.Play("Empower");
        EndTurn();
    }

    private IEnumerator Attack()
    {
        Dev.Log();
        yield return MoveAndAttack("Attack", () => PlayerEvents.PlayerHit(attackPower));
        attackPower = enemyData.attackPower;
        EndTurn();
    }
}
