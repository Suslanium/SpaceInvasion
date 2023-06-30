using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeWeapon : MeleeWeapon
{
    [SerializeField] private string unequipStateName;
    [SerializeField] private string equipStateName;
    [SerializeField] private float equipLength;
    [SerializeField] private float equipAppearanceDelay;
    [SerializeField] private float unequipDisappearanceDelay;
    [SerializeField] private GameObject weaponMesh;
    [SerializeField] private AudioClip equipSound;
    [SerializeField] private AudioClip unequipSound;
    public bool IsEquipped
    {
        get
        {
            return isEquipped;
        }
    }
    private bool isEquipped = false;

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
        isEquipped = false;
        Equip();
    }

    public bool Equip()
    {
        if (isEquipped && !isHolding && Time.time >= nextAnimationTime)
        {
            if (unequipSound != null)
            {
                audioSource.PlayOneShot(unequipSound);
            }
            animator.Play(unequipStateName, animationLayerIndex);
            nextAnimationTime = Time.time + equipLength;
            isEquipped = false;
            StartCoroutine(UnequipDelayedDisappearance());
            return true;
        }
        else if (!isHolding && Time.time >= nextAnimationTime)
        {
            if (equipSound != null)
            {
                audioSource.PlayOneShot(equipSound);
            }
            animator.Play(equipStateName, animationLayerIndex);
            nextAnimationTime = Time.time + equipLength;
            isEquipped = true;
            StartCoroutine(EquipDelayedAppearance());
            return true;
        }
        return false;
    }

    public override float Attack()
    {
        if(isEquipped)
        {
            return base.Attack();
        }
        return 0f;
    }

    private IEnumerator EquipDelayedAppearance()
    {
        yield return new WaitForSeconds(equipAppearanceDelay);
        weaponMesh.SetActive(true);
        yield return null;
    }

    private IEnumerator UnequipDelayedDisappearance()
    {
        yield return new WaitForSeconds(unequipDisappearanceDelay);
        weaponMesh.SetActive(false);
        yield return null;
    }
}
