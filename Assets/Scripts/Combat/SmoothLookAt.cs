using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Axis
{
    X,
    Y,
    Z
}

public class SmoothLookAt : MonoBehaviour
{
    private Quaternion verticalAnchorOriginalRotation = Quaternion.identity;
    private Quaternion horizontalAnchorOriginalRotation = Quaternion.identity;
    private Quaternion prevFrameVerticalAnchorRotation = Quaternion.identity;
    private Quaternion prevFrameHorizontalAnchorRotation = Quaternion.identity;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform horizontalRotationAnchor;
    [SerializeField] private Axis horizontalRotationAxis;
    [SerializeField] private Axis verticalRotationAxis;
    [SerializeField] private Transform verticalRotationAnchor;
    [SerializeField] private bool horizontalRotationOnly;
    [SerializeField] private bool invertHorizontalRotation;
    [SerializeField] private bool invertVerticalRotation;
    [SerializeField] private bool returnToOriginalRotationOnRemoveTarget;
    [SerializeField] private Vector3 horizontalRotationOffset;
    [SerializeField] private Vector3 verticalRotationOffset;
    [SerializeField] private Transform lookTarget;

    void Start()
    {
        if (!horizontalRotationOnly)
        {
            verticalAnchorOriginalRotation = verticalRotationAnchor.localRotation;
            prevFrameVerticalAnchorRotation = verticalAnchorOriginalRotation;
        }
        horizontalAnchorOriginalRotation = horizontalRotationAnchor.rotation;
        prevFrameHorizontalAnchorRotation = horizontalAnchorOriginalRotation;
    }

    public void SetTarget(Transform target)
    {
        lookTarget = target;
        if (!horizontalRotationOnly)
        {
            verticalAnchorOriginalRotation = verticalRotationAnchor.localRotation;
            prevFrameVerticalAnchorRotation = verticalAnchorOriginalRotation;
        }
        horizontalAnchorOriginalRotation = horizontalRotationAnchor.rotation;
        prevFrameHorizontalAnchorRotation = horizontalAnchorOriginalRotation;
    }

    public void RemoveTarget()
    {
        lookTarget = null;
    }

    void LateUpdate()
    {
        Quaternion verticalLookRotation = verticalAnchorOriginalRotation;
        Quaternion horizontalLookRotation = horizontalAnchorOriginalRotation;
        if (lookTarget != null)
        {
            if (!horizontalRotationOnly)
            {
                Vector3 verticalLookDirection = lookTarget.position - verticalRotationAnchor.position;
                verticalLookRotation = Quaternion.LookRotation(verticalLookDirection);
                verticalLookRotation.eulerAngles = new Vector3(
                    (verticalRotationAxis == Axis.X ? (invertVerticalRotation ? -verticalLookRotation.eulerAngles.x : verticalLookRotation.eulerAngles.x) : verticalAnchorOriginalRotation.eulerAngles.x) + verticalRotationOffset.x,
                    (verticalRotationAxis == Axis.Y ? (invertVerticalRotation ? -verticalLookRotation.eulerAngles.x : verticalLookRotation.eulerAngles.x) : verticalAnchorOriginalRotation.eulerAngles.y) + verticalRotationOffset.y,
                    (verticalRotationAxis == Axis.Z ? (invertVerticalRotation ? -verticalLookRotation.eulerAngles.x : verticalLookRotation.eulerAngles.x) : verticalAnchorOriginalRotation.eulerAngles.z) + verticalRotationOffset.z
                );
            }
            Vector3 horizontalLookDirection = lookTarget.position - horizontalRotationAnchor.position;
            horizontalLookRotation = Quaternion.LookRotation(horizontalLookDirection);
            horizontalLookRotation.eulerAngles = new Vector3(
                (horizontalRotationAxis == Axis.X ? (invertHorizontalRotation ? -horizontalLookRotation.eulerAngles.y : horizontalLookRotation.eulerAngles.y) : horizontalAnchorOriginalRotation.eulerAngles.x) + horizontalRotationOffset.x,
                (horizontalRotationAxis == Axis.Y ? (invertHorizontalRotation ? -horizontalLookRotation.eulerAngles.y : horizontalLookRotation.eulerAngles.y) : horizontalAnchorOriginalRotation.eulerAngles.y) + horizontalRotationOffset.y,
                (horizontalRotationAxis == Axis.Z ? (invertHorizontalRotation ? -horizontalLookRotation.eulerAngles.y : horizontalLookRotation.eulerAngles.y) : horizontalAnchorOriginalRotation.eulerAngles.z) + horizontalRotationOffset.z
            );
        }
        if (lookTarget != null || returnToOriginalRotationOnRemoveTarget)
        {
            horizontalRotationAnchor.transform.rotation = Quaternion.Lerp(prevFrameHorizontalAnchorRotation, horizontalLookRotation, rotationSpeed * Time.deltaTime);
            prevFrameHorizontalAnchorRotation = horizontalRotationAnchor.transform.rotation;
            if (!horizontalRotationOnly)
            {
                verticalRotationAnchor.transform.localRotation = Quaternion.Lerp(prevFrameVerticalAnchorRotation, verticalLookRotation, rotationSpeed * Time.deltaTime);
                prevFrameVerticalAnchorRotation = verticalRotationAnchor.transform.localRotation;
            }
        }
    }
}
