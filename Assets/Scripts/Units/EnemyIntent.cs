public readonly struct EnemyIntent
{
    public readonly IntentType Type;
    public readonly int Value;

    public EnemyIntent(IntentType type, int value)
    {
        Type = type;
        Value = value;
    }
}
