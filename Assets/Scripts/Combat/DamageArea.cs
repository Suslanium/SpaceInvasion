using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damagePerSecond;
    [SerializeField] private bool targetOnlyOneTag;
    [SerializeField] private string targetTag;
    private List<CharacterStats> stats = new List<CharacterStats>();

    private void OnTriggerEnter(Collider other)
    {
        if (targetOnlyOneTag && other.gameObject.tag != targetTag)
        {
            return;
        }
        var stats = other.GetComponentInChildren<CharacterStats>();
        if (stats != null)
        {
            if (stats.StartRepeatedDamage(damagePerSecond))
            {
                this.stats.Add(stats);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetOnlyOneTag && other.gameObject.tag != targetTag)
        {
            return;
        }
        var stats = other.GetComponentInChildren<CharacterStats>();
        if (stats != null)
        {
            stats.StopRepeatedDamage();
        }
    }

    private void OnDestroy()
    {
        foreach(CharacterStats stats in stats)
        {
            if (stats != null)
            {
                stats.StopRepeatedDamage();
            }
        }
    }
}
