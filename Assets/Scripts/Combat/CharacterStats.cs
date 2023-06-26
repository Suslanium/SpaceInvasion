using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject characterGameobject;
    [SerializeField] private GameObject deathParticleEffectPrefab;
    [SerializeField] private float deathEffectLifetime;
    private int health;

    void Start()
    {
        health = maxHealth;
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
