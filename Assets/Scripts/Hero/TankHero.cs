public class TankHero : Hero
{
    public override string Name => "Tank";
    public override int HealthPoints => 100;
    public override int ArmorPoints => 150;
    public override float DamageMultiplier => 1.3f;
    public override float MovementSpeedMultiplier => 0.8f;
    public override Ability DominantAbility => Ability.TankDominantAbilityName;
    public override Ability RecessiveAbility => Ability.TankRecessiveAbilityName;
}
