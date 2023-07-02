using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class InstantAreaDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float destructionDelay;
    [SerializeField] private float impactDestructionTime;
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private string targetTag;

    void Start()
    {
        Destroy(gameObject, destructionDelay);
        GameObject effect = Instantiate(impactPrefab, gameObject.transform.position, impactPrefab.transform.rotation);
        effect.transform.eulerAngles = transform.eulerAngles;
        Destroy(effect, impactDestructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            var stats = other.gameObject.GetComponentInChildren<CharacterStats>();
            if (stats != null)
            {
                stats.Damage(damage);
            }
        }
    }
}
