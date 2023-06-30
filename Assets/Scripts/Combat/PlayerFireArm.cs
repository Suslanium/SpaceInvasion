using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerFireArm : Firearm
{
    private Recoil recoilModule;
    [SerializeField] private string unequipStateName;
    [SerializeField] private string equipStateName;
    [SerializeField] private float equipTime;
    [SerializeField] private float aimSpeed;
    [SerializeField] private Vector3 rotationalRecoil;
    [SerializeField] private Vector3 aimRotationalRecoil;
    [SerializeField] private Vector3 positionalRecoil;
    [SerializeField] private Vector3 aimPositionalRecoil;
    [SerializeField] protected AudioClip equipSound;
    [SerializeField] private float recoilSnappiness;
    [SerializeField] private float recoilReturnSpeed;
    [SerializeField] private Transform aimPosition;
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Transform aimAnchor;
    [SerializeField] private int aimMoveSpeed;
    public int AimSpeed
    {
        get
        {
            return aimMoveSpeed;
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

    protected override void Fire()
    {
        if (isEquipped && Time.time >= nextAnimationTime)
        {
            base.Fire();
            if (recoilModule != null)
            {
                recoilModule.ApplyRecoil();
            }
        }
    }

    public override void Reload()
    {
        if (isEquipped)
        {
            base.Reload();
        }
    }

    public bool Equip()
    {
        if (isEquipped && !isHolding && Time.time >= nextAnimationTime)
        {
            animator.Play(unequipStateName, animationLayerIndex);
            nextAnimationTime = Time.time + equipTime;
            isEquipped = false;
            isAiming = false;
            return true;
        }
        else if (!isHolding && Time.time >= nextAnimationTime)
        {
            if (equipSound != null)
            {
                audioSource.PlayOneShot(equipSound);
            }
            animator.Play(equipStateName, animationLayerIndex);
            nextAnimationTime = Time.time + equipTime;
            isEquipped = true;
            isAiming = false;
            return true;
        }
        return false;
    }

    public void Aim()
    {
        isAiming = !isAiming;
    }

    private void Update()
    {
        UpdatePosition();
    }

    protected override abstract void ApplyAttackLogic();

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
