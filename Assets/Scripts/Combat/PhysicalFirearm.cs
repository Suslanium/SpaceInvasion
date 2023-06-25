using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalFirearm : Firearm
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootForce = 300f;
    [SerializeField] private float spread;
    [SerializeField] private float noFoundTargetRange = 75f;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform bulletInstantiationPoint;
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private LayerMask ignoreLayers;

    protected override void ApplyFireLogic()
    {
        foreach(ParticleSystem flash in muzzleFlash)
        {
            flash.Play();
        }
        RaycastHit hit;
        Vector3 targetPoint;
        Vector3 shootingDirection = shootingPoint.transform.forward + shootingPoint.transform.TransformDirection(new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread)));
        Ray shootingRay = new Ray(shootingPoint.transform.position, shootingDirection);
        if (Physics.Raycast(shootingRay, out hit, ~ignoreLayers))
        {
            targetPoint = hit.point;
        } 
        else
        {
            targetPoint = shootingRay.GetPoint(noFoundTargetRange);
        }
        Vector3 bulletDirection = targetPoint - bulletInstantiationPoint.position;
        GameObject bullet = Instantiate(bulletPrefab, bulletInstantiationPoint.position, Quaternion.identity);
        bullet.transform.forward = bulletDirection.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(bulletDirection.normalized * shootForce, ForceMode.Impulse);
    }
}
