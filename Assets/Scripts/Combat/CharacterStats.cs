using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int maxArmor;
    [SerializeField] private int armor;
    [SerializeField] private GameObject characterGameobject;
    [SerializeField] private GameObject deathParticleEffectPrefab;
    [SerializeField] private float deathEffectLifetime;

    void Start()
    {
        health = maxHealth;
    }

    public void InitStats(int health, int armor)
    {
        maxHealth = health;
        this.health = health;
        maxArmor = armor;
        this.armor = armor;
    }

    private void Die()
    {
        var deathEffect = Instantiate(deathParticleEffectPrefab, characterGameobject.transform.position, characterGameobject.transform.rotation);
        Destroy(deathEffect, deathEffectLifetime);
        Destroy(characterGameobject);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    } 

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
