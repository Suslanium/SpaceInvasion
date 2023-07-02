using UnityEngine;

public abstract class Hero
{
    public abstract string Name { get; }
    public abstract int HealthPoints { get; }
    public abstract int ArmorPoints { get; }
    public abstract float DamageMultiplier { get; }
    public abstract float MovementSpeedMultiplier { get; }
    public abstract Ability DominantAbility { get; }
    public abstract Ability RecessiveAbility { get; }

    public override string ToString()
    {
        return $"HP: {HealthPoints}\n" +
               $"Armor: {ArmorPoints}\n" +
               $"Damage Multiplier: {DamageMultiplier * 100}%\n" +
               $"Movement Speed Multiplier: {MovementSpeedMultiplier * 100}%\n" +
               $"Dominant Ability: {DominantAbility}\n" +
               $"Recessive Ability: {RecessiveAbility}";
    }
}