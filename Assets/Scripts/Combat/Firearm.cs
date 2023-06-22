using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public abstract class Firearm : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private Recoil recoilModule;
    [Header("Animator-related settings")]
    [SerializeField] private string shootStateName;
    [SerializeField] private string reloadStateName;
    [SerializeField] private string unequipStateName;
    [SerializeField] private string equipStateName;
    [Header("Firearm sounds")]
    [SerializeField] private AudioClip firingSound;
    [SerializeField] private bool hasHoldingFireSound;
    [SerializeField] private AudioClip holdingFireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip equipSound;
    [Header("Weapon specs")]
    [SerializeField] private int fireRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private float equipTime;
    [SerializeField] private bool holdingIsAllowed;
    [SerializeField] private float aimSpeed;
    [SerializeField] private Vector3 rotationalRecoil;
    [SerializeField] private Vector3 aimRotationalRecoil;
    [SerializeField] private Vector3 positionalRecoil;
    [SerializeField] private Vector3 aimPositionalRecoil;
    [SerializeField] private float recoilSnappiness;
    [SerializeField] private float recoilReturnSpeed;
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
    public Recoil Recoil
    {
        set
        {
            recoilModule = value;
        }
    }
    public Vector3 RotationalRecoil
    {
        get
        {
            return rotationalRecoil;
        }
    }
    public Vector3 RotationalAimRecoil
    {
        get
        {
            return aimRotationalRecoil;
        }
    }
    public Vector3 PositionalRecoil
    {
        get
        {
            return positionalRecoil;
        }
    }
    public Vector3 PositionalAimRecoil
    {
        get
        {
            return aimPositionalRecoil;
        }
    }
    public float RecoilSnappiness
    {
        get
        {
            return recoilSnappiness;
        }
    }
    public float RecoilReturnSpeed
    {
        get
        {
            return recoilReturnSpeed;
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
            animator.CrossFadeInFixedTime(shootStateName, 0f);
            if (recoilModule != null)
            {
                recoilModule.ApplyRecoil();
            }
            ApplyFireLogic();
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
        if (hasHoldingFireSound)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    public void Reload()
    {
        if (isEquipped && !isHolding && Time.time >= nextAnimationTime)
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
        if (isEquipped && !isHolding && Time.time >= nextAnimationTime)
        {
            animator.Play(unequipStateName);
            nextAnimationTime = Time.time + equipTime;
            isEquipped = false;
            isAiming = false;
        }
        else if (!isHolding && Time.time >= nextAnimationTime)
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

    protected abstract void ApplyFireLogic();

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
