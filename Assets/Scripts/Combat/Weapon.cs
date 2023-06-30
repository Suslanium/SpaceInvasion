using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected bool holdingIsAllowed;
    public bool HoldingIsAllowed
    {
        get
        {
            return holdingIsAllowed;
        }
    }

    public abstract float Attack();

    public abstract void StartHoldingAttack();

    public abstract void StopHoldingAttack();
}
