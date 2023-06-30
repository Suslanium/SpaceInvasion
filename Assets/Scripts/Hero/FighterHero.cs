public class FighterHero : Hero
{
    public override int HealthPoints => 100;
    public override int ArmorPoints => 50;
    public override float DamageMultiplier => 0.8f;
    public override float MovementSpeedMultiplier => 1.15f;
    public override Ability DominantAbility => Ability.FighterDominantAbilityName;
    public override Ability RecessiveAbility => Ability.FighterRecessiveAbilityName;
}
