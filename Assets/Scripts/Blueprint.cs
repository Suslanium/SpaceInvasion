public class Blueprint
{
    public int GearsRequired;
    public int PlasmaRequired;
    public int CrystalRequired;
    public int SpringRequired;
    public int EnergyCoreRequired;
    public WeaponType WeaponType;

    public Blueprint(
        int gearsRequired,
        int plasmaRequired,
        int crystalRequired,
        int springRequired,
        int energyCoreRequired,
        WeaponType weaponType
    )
    {
        GearsRequired = gearsRequired;
        PlasmaRequired = plasmaRequired;
        CrystalRequired = crystalRequired;
        SpringRequired = springRequired;
        EnergyCoreRequired = energyCoreRequired;
        WeaponType = weaponType;
    }
}