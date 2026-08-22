using System;

public static class BossEvents
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
