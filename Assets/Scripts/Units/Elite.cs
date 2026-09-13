using System.Collections;
using UnityEngine;

// Example of a simpler enemy built on the shared Enemy base: no empower
// mechanic, it just attacks every turn.
public class Elite : Enemy
{
    [SerializeField] private int attackPower = 2;

    protected override void DecideActions()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        Dev.Log();
        yield return MoveAndAttack("Attack", () => PlayerEvents.PlayerHit(attackPower));
        EndTurn();
    }
}
