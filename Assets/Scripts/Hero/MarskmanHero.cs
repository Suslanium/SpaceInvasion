public class MarksmanHero : Hero
{
    public override int HealthPoints => 100;
    public override int ArmorPoints => 100;
    public override float DamageMultiplier => 1f;
    public override float MovementSpeedMultiplier => 1f;
    public override Ability DominantAbility => Ability.MarksmanDominantAbilityName;
    public override Ability RecessiveAbility => Ability.MarksmanRecessiveAbilityName;
}
