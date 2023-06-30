public class PlayerHero : Hero
{
    public override string Name => "Your hero";
    public override int HealthPoints { get; }
    public override int ArmorPoints { get; }
    public override float DamageMultiplier { get; }
    public override float MovementSpeedMultiplier { get; }
    public override Ability DominantAbility { get; }
    public override Ability RecessiveAbility { get; }

    public PlayerHero(int healthPoints, int armorPoints, float damageMultiplier, float movementSpeedMultiplier,
        Ability dominantAbility, Ability recessiveAbility)
    {
        HealthPoints = healthPoints;
        ArmorPoints = armorPoints;
        DamageMultiplier = damageMultiplier;
        MovementSpeedMultiplier = movementSpeedMultiplier;
        DominantAbility = dominantAbility;
        RecessiveAbility = recessiveAbility;
    }
}