using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementModule : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private int animationLayerIndex;
    [SerializeField] private string walkStateName;
    [SerializeField] private string idleStateName;
    private bool isWalking;
    private bool isSuspended;
    private Transform currentTarget;
    private float stopDistance;
    private Vector3 prevTargetPosition = Vector3.zero;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform target, float stopDistance)
    {
        currentTarget = target;
        this.stopDistance = stopDistance;
    }

    public void RemoveTarget()
    {
        currentTarget = null;
    }

    public void SuspendMovementForSeconds(float seconds, bool returnToIdle)
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            isSuspended = true;
            if (returnToIdle)
            {
                isWalking = false;
                animator.CrossFade(idleStateName, animationLayerIndex);
            }
            StartCoroutine(ResumeExecution(seconds));
        }
    }

    void Update()
    {
        if (!isSuspended)
        {
            if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) > stopDistance)
            {
                if (!isWalking)
                {
                    isWalking = true;
                    animator.CrossFade(walkStateName, animationLayerIndex);
                }
                if (currentTarget.position != prevTargetPosition)
                {
                    navMeshAgent.SetDestination(currentTarget.position);
                    prevTargetPosition = currentTarget.position;
                }
            }
            else
            {
                if (isWalking)
                {
                    isWalking = false;
                    animator.CrossFade(idleStateName, animationLayerIndex);
                }
                if (transform.position != prevTargetPosition)
                {
                    navMeshAgent.SetDestination(transform.position);
                    prevTargetPosition = transform.position;
                }
            }
        }
    }

    private IEnumerator ResumeExecution(float resumeAfterSeconds)
    {
        yield return new WaitForSeconds(resumeAfterSeconds);
        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;
            navMeshAgent.isStopped = false;
            isSuspended = false;
        }
        yield return null;
    }
}
