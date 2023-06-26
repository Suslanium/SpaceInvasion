using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlModule : MonoBehaviour
{
    [SerializeField] private SmoothLookAt lookModule;
    [Header("Optional, character may not move")]
    [SerializeField] private MovementModule movementModule;
    [SerializeField] private float visionDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float stopToAttackDistance;
    [SerializeField] private string enemyTag;
    private GameObject currentEnemy;
    [SerializeField] private Transform attackPoint;

    void Update()
    {
        if (currentEnemy != null)
        {
            if (Vector3.Distance(transform.position, currentEnemy.transform.position) < visionDistance)
            {
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) < attackDistance)
                {
                    Ray ray = new Ray(attackPoint.transform.position, currentEnemy.transform.position - attackPoint.transform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject == currentEnemy)
                        {
                            //Unfinished
                            Debug.Log("Attack!");
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
                lookModule.RemoveTarget();
            }
        }
        else
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
                    lookModule.SetTarget(currentEnemy.transform);
                    break;
                }
            }
        }
    }
}
