using System;
using Random = System.Random;

public static class PlayerRepository
{
    public static PlayerHero PlayerHero { get; private set; }
    
    public static void CreatePlayerHero(Hero firstParent, Hero secondParent)
    {
        const float dominantAbilityChance = 0.7f;
        var random = new Random();

        int playerHeroHealthPoints = 100;
        int playerHeroArmorPoints;
        float playerHeroDamageMultiplier;
        float playerHeroMovementSpeedMultiplier;

        if (firstParent is TankHero && secondParent is FighterHero ||
            firstParent is FighterHero && secondParent is TankHero)
        {
            playerHeroArmorPoints = 100;
            playerHeroDamageMultiplier = 1f;
            playerHeroMovementSpeedMultiplier = 1f;
        }
        else if (firstParent is TankHero && secondParent is MarksmanHero ||
                 secondParent is TankHero && firstParent is MarksmanHero)
        {
            playerHeroArmorPoints = 150;
            playerHeroDamageMultiplier = 1.3f;
            playerHeroMovementSpeedMultiplier = 0.8f;
        }
        else if (firstParent is FighterHero && secondParent is MarksmanHero ||
                 secondParent is FighterHero && firstParent is MarksmanHero)
        {
            playerHeroArmorPoints = 50;
            playerHeroDamageMultiplier = 0.8f;
            playerHeroMovementSpeedMultiplier = 1.15f;
        }
        else
        {
            throw new ArgumentException();
        }

        var firstAbility = random.NextDouble() <= dominantAbilityChance
            ? firstParent.DominantAbility
            : firstParent.RecessiveAbility;
        var secondAbility = random.NextDouble() <= dominantAbilityChance
            ? secondParent.DominantAbility
            : secondParent.RecessiveAbility;
        
        PlayerHero = new PlayerHero(
            playerHeroHealthPoints,
            playerHeroArmorPoints,
            playerHeroDamageMultiplier,
            playerHeroMovementSpeedMultiplier,
            firstAbility,
            secondAbility
        );
    }
}
