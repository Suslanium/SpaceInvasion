using UnityEngine;

public class RaycastFirearm : Firearm
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float spread;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private ParticleSystem shootingEffect;
    [SerializeField] private GameObject impactObjectPrefab;
    [SerializeField] private float impactLifetime = 2f;
    [SerializeField] private LayerMask ignoreLayers;
    protected override void ApplyFireLogic()
    {
        if (shootingEffect != null)
        {
            shootingEffect.Play();
        }

        RaycastHit hit;
        Vector3 shootingDirection = shootingPoint.transform.forward + shootingPoint.transform.TransformDirection(new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread)));
        if (Physics.Raycast(shootingPoint.transform.position, shootingDirection, out hit, range, ~ignoreLayers))
        {
            if (impactObjectPrefab != null)
            {
                GameObject impactEffect = Instantiate(impactObjectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactEffect, impactLifetime);
            }
        }
    }
}
