using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int maxArmor;
    [SerializeField] private int armor;
    [SerializeField] private int armorRegenPerSecond;
    [SerializeField] private GameObject characterGameobject;
    [SerializeField] private GameObject deathParticleEffectPrefab;
    [SerializeField] private float deathEffectLifetime;
    private RoomBehaviour _ownerRoom;

    public HealthBar healthBar;

    void Start()
    {
        health = maxHealth;
        if (armorRegenPerSecond != 0)
        {
            StartCoroutine(ArmorRegen());
        }
    }

    public void InitStats(int health, int armor)
    {
        maxHealth = health;
        this.health = health;
        healthBar.SetMaxHealth(maxHealth);
        maxArmor = armor;
        this.armor = armor;
    }

    public void SetOwnerRoom(RoomBehaviour room)
    {
        _ownerRoom = room;
    }

    private void Die()
    {
        var deathEffect = Instantiate(deathParticleEffectPrefab, characterGameobject.transform.position, characterGameobject.transform.rotation);
        Destroy(deathEffect, deathEffectLifetime);
        if (_ownerRoom != null)
        {
            _ownerRoom.deathEnemy();
        }
        Destroy(characterGameobject);
    }

    public void Damage(int amount)
    {
        if (armor >= amount)
        {
            armor -= amount;
            return;
        }
        else if (armor > 0)
        {
            amount -= armor;
            armor = 0;
        }
        health -= amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        
        if (health <= 0)
        {
            Die();
        }
    } 

    public void Heal(int amount)
    {
        health += amount;
        healthBar.SetHealth(health);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private IEnumerator ArmorRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            armor += armorRegenPerSecond;
            if (armor > maxArmor)
            {
                armor = maxArmor;
            }
        }
    }
}
