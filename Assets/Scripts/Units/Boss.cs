using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator empowerVFX;

    private readonly int originalATK = 3;
    private readonly int empowerAmount = 2;
    private int attackPower;

    protected override void Start()
    {
        base.Start();
        attackPower = originalATK;
    }

    protected override void DecideActions()
    {
        if (attackPower >= originalATK + empowerAmount)
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

    private void Empower()
    {
        Dev.Log();
        attackPower += empowerAmount;
        empowerVFX.Play("Empower");
        EndTurn();
    }

    private IEnumerator Attack()
    {
        Dev.Log();
        yield return MoveAndAttack("Attack", () => PlayerEvents.PlayerHit(attackPower));
        attackPower = originalATK;
        EndTurn();
    }
}
