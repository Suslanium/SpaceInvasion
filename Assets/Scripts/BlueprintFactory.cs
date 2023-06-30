using System;

// TODO replace with actual numbers of required resources
public static class BlueprintFactory
{
    public static Blueprint CreateBlueprint(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                return new Blueprint(
                    gearsRequired: 0,
                    plasmaRequired: 0,
                    crystalRequired: 0,
                    springRequired: 0,
                    energyCoreRequired: 0,
                    weaponType: weaponType
                );
            case WeaponType.AutomaticRifle:
                return new Blueprint(
                    gearsRequired: 0,
                    plasmaRequired: 0,
                    crystalRequired: 0,
                    springRequired: 0,
                    energyCoreRequired: 0,
                    weaponType: weaponType
                );
            case WeaponType.Minigun:
                return new Blueprint(
                    gearsRequired: 0,
                    plasmaRequired: 0,
                    crystalRequired: 0,
                    springRequired: 0,
                    energyCoreRequired: 0,
                    weaponType: weaponType
                );
            default:
                throw new NotImplementedException();
        }
    }
}
