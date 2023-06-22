using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 maxRecoil;
    private Vector3 positionalRecoil;
    private float snappiness;
    private float returnSpeed;
    private Quaternion originalRotation;
    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        targetRotation = Quaternion.Lerp(targetRotation, originalRotation, returnSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, snappiness * Time.deltaTime);
        targetPosition = Vector3.Lerp(targetPosition, originalPosition, returnSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, snappiness * Time.deltaTime);
    }

    public void ApplyRecoil()
    {
        targetRotation *= Quaternion.Euler(-maxRecoil.x, Random.Range(-maxRecoil.y, maxRecoil.y), Random.Range(-maxRecoil.z, maxRecoil.z));
        targetPosition += new Vector3(Random.Range(-positionalRecoil.x, positionalRecoil.x), Random.Range(-positionalRecoil.y, positionalRecoil.y), -positionalRecoil.z);
    }

    public void SetRecoilSpecs(Vector3 rotationalRecoil, Vector3 positionalRecoil, float snappiness, float returnSpeed)
    {
        maxRecoil = rotationalRecoil;
        this.positionalRecoil = positionalRecoil;
        this.snappiness = snappiness;
        this.returnSpeed = returnSpeed;
    }
}
