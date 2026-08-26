using System;

public static class EnemyEvents
{
    public static event Action<int> OnBossHit;
    public static event Action OnBossDeath;

    public static void BossHit(int damage)
    {
        OnBossHit?.Invoke(damage);
    }

    public static void BossDeath()
    {
        OnBossDeath?.Invoke();
    }
}
