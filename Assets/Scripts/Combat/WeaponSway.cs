using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Bob")]
    [SerializeField] private CharacterController playerRigidbody;
    [SerializeField] private StarterAssets.FirstPersonController playerController;
    [SerializeField] private Vector3 travelIntensity = Vector3.one * 0.05f;
    [SerializeField] private Vector3 aimTravelIntensity = Vector3.one * 0.05f;
    [SerializeField] private Vector3 bobIntensity = Vector3.one * 0.01f;
    [SerializeField] private Vector3 aimBobIntensity = Vector3.one * 0.005f;
    [SerializeField] private float bobSmoothness;
    [SerializeField] private float bobSpeedMultiplier = 1f;
    [Header("Rotational bob")]
    [SerializeField] private Vector3 rotationalBobIntensity = Vector3.one * 0.01f;
    [SerializeField] private Vector3 rotationalAimBobIntensity = Vector3.one * 0.005f;
    [SerializeField] private float rotationalBobSmoothness;
    [Header("Rotational sway")]
    [SerializeField] private float rotationSwayIntensity;
    [Header("Tilt sway")]
    [SerializeField] private float mouseTiltIntensity;
    [SerializeField] private float moveTiltIntensity;
    [SerializeField] private float moveAimTiltIntensity;

    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveY;
    private bool isAiming = false;
    private float bobCurvePosition;

    private void Start()
    {
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        UpdatePositionalBob(isAiming ? aimBobIntensity : bobIntensity, isAiming ? aimTravelIntensity : travelIntensity);
        UpdateRotation(isAiming ? rotationalAimBobIntensity : rotationalBobIntensity);
    }

    private void UpdatePositionalBob(Vector3 bobIntensity, Vector3 travelIntensity)
    {
        Vector3 targetPosition = originalPosition;
        if (playerRigidbody.velocity.magnitude > 0)
        {
            bobCurvePosition += Time.deltaTime * (playerController.Grounded && !playerController.IsDashing ? playerRigidbody.velocity.magnitude * bobSpeedMultiplier : 1f) + 0.01f;

            float xAdjustment = (Mathf.Cos(bobCurvePosition) * bobIntensity.x * (playerController.Grounded ? 1 : 0))
                - (moveX * travelIntensity.x);
            float yAdjustment = (Mathf.Sin(bobCurvePosition) * bobIntensity.y)
                - (playerRigidbody.velocity.y * travelIntensity.y);
            float zAdjustment = -(moveY * travelIntensity.z);

            Vector3 positionAdjustment = new Vector3(xAdjustment, yAdjustment, zAdjustment);
            targetPosition += positionAdjustment;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * (1f / bobSmoothness));
    }

    private void UpdateRotation(Vector3 rotationalBobIntensity)
    {
        Quaternion horizontalSwayAdjustment = Quaternion.AngleAxis(-rotationSwayIntensity * mouseX, Vector3.up);
        Quaternion verticalSwayAdjustment = Quaternion.AngleAxis(-rotationSwayIntensity * mouseY, Vector3.right);
        Quaternion horizontalTiltAdjustment = Quaternion.AngleAxis(-mouseTiltIntensity * mouseX 
            -(isAiming ? moveAimTiltIntensity : moveTiltIntensity) * moveX, Vector3.forward);

        Quaternion bobAdjustment = Quaternion.Euler(0, 0, 0);

        if (playerRigidbody.velocity.magnitude > 0)
        {
            float xBobAdjustment = rotationalBobIntensity.x * Mathf.Sin(2 * bobCurvePosition);
            float yBobAdjustment = rotationalBobIntensity.y * Mathf.Cos(bobCurvePosition);
            float zBobAdjustment = rotationalBobIntensity.z * Mathf.Cos(bobCurvePosition) * moveX;
            bobAdjustment = Quaternion.Euler(xBobAdjustment, yBobAdjustment, zBobAdjustment);
        }

        Quaternion targetRotation = originalRotation * horizontalSwayAdjustment * verticalSwayAdjustment * horizontalTiltAdjustment * bobAdjustment;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * (1f / rotationalBobSmoothness));
    }

    public void UpdateMouseInput(Vector2 value)
    {
        mouseX = value.x;
        mouseY = value.y;
    }

    public void UpdateMoveInput(Vector2 value)
    {
        moveX = value.x;
        moveY = value.y;
    }

    public void UpdateAimingState()
    {
        isAiming = !isAiming;
    }

    public void SetAimingState(bool value)
    {
        isAiming = value;
    }
}
