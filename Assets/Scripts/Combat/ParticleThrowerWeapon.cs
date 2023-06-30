using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleThrowerWeapon : Weapon
{
    private AudioSource audioSource;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private AudioClip particleThrowingSoundLoop;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int sprintSpeed;
    private bool isHolding = false;

    private void OnEnable()
    {
        holdingIsAllowed = true;
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public override float Attack()
    {
        return 0f;
    }

    public override void StartHoldingAttack()
    {
        if (!isHolding)
        {
            isHolding = true;
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
            if (particleThrowingSoundLoop != null)
            {
                audioSource.loop = true;
                audioSource.clip = particleThrowingSoundLoop;
                audioSource.Play();
            }
        }
    }

    public override void StopHoldingAttack()
    {
        if (isHolding)
        {
            isHolding = false;
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Stop();
            }
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}
