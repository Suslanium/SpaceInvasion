using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageComponent : MonoBehaviour
{
    [SerializeField] private GameObject impactObjectPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float impactLifetime = 2f;
    [SerializeField] private AudioClip[] impactSounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string targetTag;
    public bool isAttacking { private get; set; } = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isAttacking && collision.gameObject.tag == targetTag)
        {
            if (impactObjectPrefab != null)
            {
                GameObject impactEffect = Instantiate(impactObjectPrefab, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                Destroy(impactEffect, impactLifetime);
            }
            if (audioSource != null && impactSounds.Length > 0)
            {
                audioSource.PlayOneShot(impactSounds[Random.Range(0, impactSounds.Length)]);
            }
            CharacterStats stats = collision.gameObject.GetComponent<CharacterStats>();
            if (stats != null)
            {
                stats.Damage(damage);
                isAttacking = false;
            }
        }
    }
}
