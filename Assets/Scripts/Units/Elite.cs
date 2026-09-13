using System.Collections;
using UnityEngine;

// Example of a simpler enemy built on the shared Enemy base: no empower
// mechanic, it just attacks every turn.
public class Elite : Enemy
{
    protected override EnemyIntent ChooseNextMove()
    {
        return new EnemyIntent(IntentType.Attack, enemyData.attackPower);
    }

    protected override void PerformMove(EnemyIntent intent)
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        Dev.Log();
        yield return MoveAndAttack("Attack", () => PlayerEvents.PlayerHit(enemyData.attackPower));
        EndTurn();
    }
}
