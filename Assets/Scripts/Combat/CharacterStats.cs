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
    private bool isGettingRepeatedDamage = false;
    private RoomBehaviour _ownerRoom;

    public HealthBar healthBar;
    public ArmorBar armorBar;

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
        armorBar.SetMaxArmor(maxArmor);
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
            if (armorBar != null)
            {
                armorBar.SetArmor(armor);
            }
            return;
        }
        else if (armor > 0)
        {
            amount -= armor;
            if (armorBar != null)
            {
                armorBar.SetArmor(armor);
            }
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

    public bool StartRepeatedDamage(int damagePerSecond)
    {
        if (!isGettingRepeatedDamage)
        {
            Debug.Log("Start damage");
            isGettingRepeatedDamage = true;
            StartCoroutine(RepeatedDamage(damagePerSecond));
            return true;
        }
        return false;
    }

    public void StopRepeatedDamage()
    {
        isGettingRepeatedDamage = false;
    }

    private IEnumerator RepeatedDamage(int damagePerSecond)
    {
        while (isGettingRepeatedDamage)
        {
            Damage(damagePerSecond);
            yield return new WaitForSeconds(1);
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
            if (armorBar != null)
            {
                armorBar.SetArmor(armor);
            }
        }
    }
}
