using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlModule : MonoBehaviour
{
    [SerializeField] private SmoothLookAt lookModule;
    [SerializeField] private MovementModule movementModule;
    [SerializeField] private bool suspendMovementOnAttack;
    [SerializeField] private float visionDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float stopToAttackDistance;
    [SerializeField] private string enemyTag;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float raycastThickness = 1f;
    [SerializeField] private bool useRegularRaycast = true;
    [SerializeField] private bool aiEnabled = false;
    [SerializeField] private Transform attackPoint;
    private GameObject currentEnemy;

    public void EnableAI()
    {
        aiEnabled = true;
    }

    void Update()
    {
        if (currentEnemy != null)
        {
            if (Vector3.Distance(transform.position, currentEnemy.transform.position) < visionDistance)
            {
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) < attackDistance)
                {
                    RaycastHit hit;
                    if ((!useRegularRaycast && Physics.SphereCast(attackPoint.transform.position, raycastThickness, attackPoint.transform.forward, out hit)) || (useRegularRaycast && Physics.Raycast(attackPoint.transform.position, attackPoint.transform.forward, out hit)))
                    {
                        if (hit.transform.gameObject == currentEnemy)
                        {
                            if (weapon.HoldingIsAllowed)
                            {
                                weapon.StartHoldingAttack();
                            }
                            else
                            {
                                float timeout = weapon.Attack();
                                if (timeout != 0f && suspendMovementOnAttack && movementModule != null)
                                {
                                    movementModule.SuspendMovementForSeconds(timeout);
                                }
                            }
                        } 
                        else
                        {
                            weapon.StopHoldingAttack();
                        }
                    }
                }
            }
            else
            {
                currentEnemy = null;
                if (movementModule != null)
                {
                    movementModule.RemoveTarget();
                }
                if (lookModule != null)
                {
                    lookModule.RemoveTarget();
                }
                weapon.StopHoldingAttack();
            }
        }
        else if (aiEnabled)
        {
            var visibleObjects = Physics.OverlapSphere(transform.position, visionDistance);
            foreach(var gameObj in visibleObjects)
            {
                if (gameObj.tag == enemyTag && gameObj.transform.gameObject != gameObject)
                {
                    currentEnemy = gameObj.transform.gameObject;
                    if(movementModule != null)
                    {
                        movementModule.SetTarget(currentEnemy.transform, stopToAttackDistance);
                    }
                    if (lookModule != null)
                    {
                        lookModule.SetTarget(currentEnemy.transform);
                    }
                    break;
                }
            }
        }
    }
}
