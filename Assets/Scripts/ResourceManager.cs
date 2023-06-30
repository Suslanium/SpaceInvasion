using System.Collections.Generic;
using System.Collections.Immutable;

public class ResourceManager
{
    public int GearCount { get; private set; }
    public int PlasmaCount { get; private set; }
    public int CrystalCount { get; private set; }
    public int SpringCount { get; private set; }
    public int EnergyCoreCount { get; private set; }
    public HashSet<Blueprint> Blueprints { get; } = new();

    public void AddGear(int amount)
    {
        GearCount += amount;
    }
    
    public void AddPlasma(int amount)
    {
        PlasmaCount += amount;
    }
    
    public void AddCrystal(int amount)
    {
        CrystalCount += amount;
    }
    
    public void AddSpring(int amount)
    {
        SpringCount += amount;
    }
    
    public void AddEnergyCore(int amount)
    {
        EnergyCoreCount += amount;
    }
    
    public void AddBlueprint(Blueprint blueprint)
    {
        Blueprints.Add(blueprint);
    }
    
    public void RemoveGear(int amount)
    {
        GearCount = GearCount >= amount ? GearCount -= amount : 0;
    }
    
    public void RemovePlasma(int amount)
    {
        PlasmaCount = PlasmaCount >= amount ? PlasmaCount -= amount : 0;
    }
    
    public void RemoveCrystal(int amount)
    {
        CrystalCount = CrystalCount >= amount ? CrystalCount -= amount : 0;
    }
    
    public void RemoveSpring(int amount)
    {
        SpringCount = SpringCount >= amount ? SpringCount -= amount : 0;
    }
    
    public void RemoveEnergyCore(int amount)
    {
        EnergyCoreCount = EnergyCoreCount >= amount ? EnergyCoreCount -= amount : 0;
    }
    
    public void RemoveBlueprint(Blueprint blueprint)
    {
        Blueprints.Remove(blueprint);
    }
}
