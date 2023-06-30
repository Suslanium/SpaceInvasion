using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    protected Animator animator;
    protected AudioSource audioSource;
    [SerializeField] protected string[] attackStateNames;
    [SerializeField] protected float[] attackLengths;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int sprintSpeed;
    [SerializeField] protected int animationLayerIndex;
    [SerializeField] protected float attackTransitionLength;
    [SerializeField] protected float swingSoundDelay;
    [SerializeField] protected AudioClip[] swingSounds;
    [SerializeField] protected MeleeWeaponDamageComponent damageComponent;
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
    protected bool isHolding = false;
    protected float nextAnimationTime = 0f;

    private void OnEnable()
    {
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
            animator.applyRootMotion = true;
        }
        if (audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public override float Attack()
    {
        if (Time.time >= nextAnimationTime)
        {
            int randomAttackIndex = Random.Range(0, attackStateNames.Length);
            animator.CrossFadeInFixedTime(attackStateNames[randomAttackIndex], attackTransitionLength, animationLayerIndex);
            nextAnimationTime = Time.time + attackLengths[randomAttackIndex];
            damageComponent.isAttacking = true;
            StartCoroutine(RemoveAttackingState());
            StartCoroutine(PlaySwingAudio());
            return attackLengths[randomAttackIndex];
        }
        return 0f;
    }

    public override void StartHoldingAttack()
    {
        if (!isHolding && holdingIsAllowed)
        {
            isHolding = true;
            HoldAttack();
        }
    }

    private void HoldAttack()
    {
        Attack();
        StartCoroutine(NextHoldAttackCheck());
    }

    private IEnumerator RemoveAttackingState()
    {
        yield return new WaitUntil(() => Time.time >= nextAnimationTime);
        damageComponent.isAttacking = false;
        yield return null;
    }

    private IEnumerator NextHoldAttackCheck()
    {
        yield return new WaitUntil(() => Time.time >= nextAnimationTime);
        if (isHolding) HoldAttack();
        yield return null;
    }

    private IEnumerator PlaySwingAudio()
    {
        yield return new WaitForSeconds(swingSoundDelay);
        if (audioSource != null && swingSounds.Length > 0)
        {
            audioSource.PlayOneShot(swingSounds[Random.Range(0, swingSounds.Length)]);
        }
        yield return null;
    }

    public override void StopHoldingAttack()
    {
        isHolding = false;
    }
}
