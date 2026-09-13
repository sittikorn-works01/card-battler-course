using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator empowerVFX;

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
        bool willAttack = attackPower >= enemyData.attackPower + enemyData.empowerAmount || Random.Range(0, 2) > 0;

        return willAttack
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
