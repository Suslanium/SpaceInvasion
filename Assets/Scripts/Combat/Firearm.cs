using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Firearm : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    [Header("Animator-related settings")]
    [SerializeField] private string shootStateName;
    [SerializeField] private string reloadStateName;
    [SerializeField] private string unequipStateName;
    [SerializeField] private string equipStateName;
    [Header("Firearm sounds")]
    [SerializeField] private AudioClip firingSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip equipSound;
    [Header("Weapon specs")]
    [SerializeField] private int fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private float equipTime;
    [SerializeField] private bool holdingIsAllowed;
    [SerializeField] private float aimSpeed;
    [Header("Positions for procedural animations")]
    [SerializeField] private Transform aimPosition;
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Transform aimAnchor;
    [Header("Moving speeds when weapon is equipped")]
    [SerializeField] private int moveSpeed;
    [SerializeField] private int sprintSpeed;
    [SerializeField] private int aimMoveSpeed;
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
    public int AimSpeed
    {
        get
        {
            return aimMoveSpeed;
        }
    }
    public bool HoldingIsAllowed
    {
        get
        {
            return holdingIsAllowed;
        }
    }

    private bool isAiming = false;
    public bool IsAiming
    {
        get
        {
            return isAiming;
        }
    }
    private bool isEquipped = false;
    public bool IsEquipped
    {
        get
        {
            return isEquipped;
        }
    }
    private float nextAnimationTime = 0f;
    private bool isHolding = false;

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
        if (aimAnchor == null)
        {
            aimAnchor = gameObject.transform;
        }
        isEquipped = false;
        Equip();
    }

    public void Fire()
    {
        if (isEquipped && Time.time >= nextAnimationTime)
        {
            if (firingSound != null)
            {
                audioSource.PlayOneShot(firingSound);
            }
            animator.CrossFadeInFixedTime(shootStateName, 0f);
            nextAnimationTime = Time.time + 1f / fireRate;
        }
    }

    public void StartHoldingFire()
    {
        if (!isHolding && holdingIsAllowed)
        {
            isHolding = true;
            HoldFire();
        }
    }

    public void StopHoldingFire()
    {
        isHolding = false;
    }

    public void Reload()
    {
        if (isEquipped && Time.time >= nextAnimationTime)
        {
            if (reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            } 
            animator.Play(reloadStateName);
            nextAnimationTime = Time.time + reloadTime;
        }
    }

    public void Equip()
    {
        if (isEquipped && Time.time >= nextAnimationTime)
        {
            animator.Play(unequipStateName);
            nextAnimationTime = Time.time + equipTime;
            isEquipped = false;
            isAiming = false;
        }
        else if (Time.time >= nextAnimationTime)
        {
            if (equipSound != null)
            {
                audioSource.PlayOneShot(equipSound);
            }
            animator.Play(equipStateName);
            nextAnimationTime = Time.time + equipTime;
            isEquipped = true;
            isAiming = false;
        }
    }

    public void Aim()
    {
        isAiming = !isAiming;
    }

    private void Update()
    {
        UpdatePosition();
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

    private void UpdatePosition()
    {
        if (isAiming)
        {
            aimAnchor.transform.position = Vector3.Lerp(aimAnchor.transform.position, aimPosition.transform.position, Time.deltaTime * aimSpeed);
        } 
        else
        {
            aimAnchor.transform.position = Vector3.Lerp(aimAnchor.transform.position, originalPosition.transform.position, Time.deltaTime * aimSpeed);
        }
    }
}
