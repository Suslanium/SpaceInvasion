using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactObjectPrefab;
    [SerializeField] private float impactLifetime = 2f;
    [SerializeField] private float bulletLifetime = 60f;
    [SerializeField] private LayerMask ignoreLayers;

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
        Destroy(gameObject);
    }
}
