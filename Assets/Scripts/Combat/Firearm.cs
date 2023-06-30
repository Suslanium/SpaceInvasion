using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public abstract class Firearm : Weapon
{
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] protected string shootStateName;
    [SerializeField] protected string reloadStateName;
    [SerializeField] protected AudioClip firingSound;
    [SerializeField] protected bool hasHoldingFireSound;
    [SerializeField] protected AudioClip holdingFireSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected int fireRate;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int sprintSpeed;
    [SerializeField] protected int animationLayerIndex;
    public int MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }
    public int SprintSpeed
    {
        get
        {
            return sprintSpeed;
        }
    }
    protected float nextAnimationTime = 0f;
    protected bool isHolding = false;

    private void OnEnable()
    {
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public override float Attack()
    {
        Fire();
        return 0f;
    }

    protected virtual void Fire()
    {
        if (Time.time >= nextAnimationTime)
        {
            if (firingSound != null && !(isHolding && hasHoldingFireSound))
            {
                audioSource.PlayOneShot(firingSound);
            } 
            else if (isHolding && hasHoldingFireSound && !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.clip = holdingFireSound;
                audioSource.Play();
            }
            animator.CrossFadeInFixedTime(shootStateName, 0f, animationLayerIndex);
            ApplyAttackLogic();
            nextAnimationTime = Time.time + 1f / fireRate;
        }
    }

    public override void StartHoldingAttack()
    {
        StartHoldingFire();
    }

    private void StartHoldingFire()
    {
        if (!isHolding && holdingIsAllowed)
        {
            isHolding = true;
            HoldFire();
        }
    }

    public override void StopHoldingAttack()
    {
        StopHoldingFire();
    }

    private void StopHoldingFire()
    {
        isHolding = false;
        if (hasHoldingFireSound)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    public virtual void Reload()
    {
        if (!isHolding && Time.time >= nextAnimationTime)
        {
            if (reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            } 
            animator.Play(reloadStateName, animationLayerIndex);
            nextAnimationTime = Time.time + reloadTime;
        }
    }

    private void HoldFire()
    {
        Fire();
        StartCoroutine(NextHoldFireCheck());
    }

    private IEnumerator NextHoldFireCheck()
    {
        yield return new WaitUntil(() => Time.time >= nextAnimationTime);
        if (isHolding) HoldFire();
        yield return null;
    }

    protected abstract void ApplyAttackLogic();
}
