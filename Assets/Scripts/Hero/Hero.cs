public abstract class Hero
{
    public abstract int HealthPoints { get; }
    public abstract int ArmorPoints { get; }
    public abstract float DamageMultiplier { get; }
    public abstract float MovementSpeedMultiplier { get; }
    public abstract Ability DominantAbility { get; }
    public abstract Ability RecessiveAbility { get; }
}