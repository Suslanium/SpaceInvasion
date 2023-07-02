using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactObjectPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float impactLifetime = 2f;
    [SerializeField] private float bulletLifetime = 60f;

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactObjectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactObjectPrefab, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            Destroy(impactEffect, impactLifetime);
        }
        CharacterStats stats = collision.gameObject.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.Damage(damage);
        }
        Destroy(gameObject);
    }
}
