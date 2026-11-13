using System.Collections;
using System.Collections.Generic;

namespace CardBattlerCourse.Enemies
{

    public class Elite : Enemy
    {
        private readonly List<IntentType> movePattern = new()
    {
        IntentType.Attack, IntentType.Attack, IntentType.Guard
    };
        private int moveIndex;

        protected override EnemyIntent ChooseNextMove()
        {
            IntentType nextType = movePattern[moveIndex];
            moveIndex = (moveIndex + 1) % movePattern.Count;

            int value = nextType == IntentType.Attack ? enemyData.attackPower : enemyData.blockAmount;
            return new EnemyIntent(nextType, value);
            //return new EnemyIntent(IntentType.Guard, 3);
        }

        protected override void PerformMove(EnemyIntent intent)
        {
            if (intent.Type == IntentType.Attack)
            {
                StartCoroutine(Attack());
            }
            else
            {
                Guard(intent.Value);
            }
        }

        private void Guard(int blockAmount)
        {
            Dev.Log();
            ApplyBlock(blockAmount);
            EndTurn();
        }

        private IEnumerator Attack()
        {
            Dev.Log();
            yield return MoveAndAttack("Attack", () => PlayerEvents.PlayerHit(enemyData.attackPower));
            EndTurn();
        }
    }
}