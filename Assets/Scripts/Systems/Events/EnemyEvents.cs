using System;
using UnityEngine;

public static class EnemyEvents
{
    public static event Action<int> OnEnemyHit;
    public static event Action OnEnemyDeath;

    public static void EnemyHit(int damage)
    {
        OnEnemyHit?.Invoke(damage);
    }

    public static void EnemyDeath()
    {
        OnEnemyDeath?.Invoke();
    }
}
