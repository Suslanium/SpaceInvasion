using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon prefabs")]
    [SerializeField] private GameObject[] weapons;
    [Header("Camera-related settings")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float aimFOV;
    [SerializeField] private float dashFOV;
    [SerializeField] private float FOVChangeSpeed;
    [SerializeField] private Cinemachine.NoiseSettings defaultNoise;
    [SerializeField] private Cinemachine.NoiseSettings aimNoise;
    [Header("Player-related settings")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private WeaponSway swayModule;
    [SerializeField] private Recoil recoilModule;
    [SerializeField] private StarterAssets.FirstPersonController playerController;
    private GameObject currentWeaponGameobject;
    private PlayerFireArm currentWeaponScript;
    private PlayerMeleeWeapon currentMeleeWeapon;
    private float currentTargetFOV;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        if (weaponParent == null)
        {
            weaponParent = transform;
        }
        currentTargetFOV = defaultFOV;
        EquipWeaponByIndex(currentWeaponIndex);
    }

    private void EquipWeaponByIndex(int index)
    {
        if (index < weapons.Length)
        {
            if (currentWeaponGameobject != null)
            {
                Destroy(currentWeaponGameobject);
                currentWeaponGameobject = null;
                currentWeaponScript = null;
            }

            GameObject newWeapon = Instantiate(weapons[index], weaponParent.position, weaponParent.rotation, weaponParent);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localEulerAngles = Vector3.zero;

            currentWeaponScript = newWeapon.GetComponentInChildren<PlayerFireArm>();
            if (currentWeaponScript != null)
            {
                if (recoilModule != null)
                {
                    currentWeaponScript.Recoil = recoilModule;
                }
            } 
            else
            {
                currentMeleeWeapon = newWeapon.GetComponentInChildren<PlayerMeleeWeapon>();
            }
            currentWeaponGameobject = newWeapon;
            ResetAim();
        }
    }

    private void ChangeTargetFOV()
    {
        if (currentTargetFOV == aimFOV)
        {
            if (virtualCamera != null)
            {
                virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = defaultNoise;
            }
            currentTargetFOV = defaultFOV;
        } 
        else
        {
            if (virtualCamera != null)
            {
                virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = aimNoise;
            }
            currentTargetFOV = aimFOV;
        }
    }

    private void Update()
    {
        if(virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, !playerController.IsDashing ? currentTargetFOV : dashFOV, Time.deltaTime * FOVChangeSpeed);
        }
    }

    private void ResetAim()
    {
        currentTargetFOV = defaultFOV;
        swayModule.SetAimingState(false);
        if (recoilModule != null && currentWeaponScript != null)
        {
            recoilModule.SetRecoilSpecs(currentWeaponScript.RotationalRecoil, currentWeaponScript.PositionalRecoil, currentWeaponScript.RecoilSnappiness, currentWeaponScript.RecoilReturnSpeed);
        }
        playerController.MoveSpeed = currentWeaponScript != null ? currentWeaponScript.MoveSpeed : currentMeleeWeapon.MoveSpeed;
        playerController.SprintSpeed = currentWeaponScript != null ? currentWeaponScript.SprintSpeed : currentMeleeWeapon.SprintSpeed;
        if (virtualCamera != null)
        {
            virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = defaultNoise;
        }
    }

    public void Fire()
    {
        if (currentWeaponScript != null)
        {
            currentWeaponScript.Attack();
        }
        else if (currentMeleeWeapon != null)
        {
            currentMeleeWeapon.Attack();
        }
    }

    public void SetHoldingAttackState(bool value)
    {
        if (currentWeaponScript != null)
        {
            if (value)
            {
                currentWeaponScript.StartHoldingAttack();
            }
            else
            {
                currentWeaponScript.StopHoldingAttack();
            }
        }
        else if (currentMeleeWeapon != null)
        {
            if (value)
            {
                currentMeleeWeapon.StartHoldingAttack();
            }
            else
            {
                currentMeleeWeapon.StopHoldingAttack();
            }
        } 
    }

    public void Reload()
    {
        if (currentWeaponScript != null)
        {
            currentWeaponScript.Reload();
        }
    }

    public void Equip()
    {
        if (currentWeaponScript != null)
        {
            if (currentWeaponScript.Equip())
            {
                ResetAim();
            }
        }
        else if (currentMeleeWeapon != null)
        {
            currentMeleeWeapon.Equip();
        }
    }

    public void Aim()
    {
        if (currentWeaponScript != null && currentWeaponScript.IsEquipped)
        {
            currentWeaponScript.Aim();
            ChangeTargetFOV();
            swayModule.UpdateAimingState();
            if (currentWeaponScript.IsAiming)
            {
                playerController.MoveSpeed = currentWeaponScript.AimSpeed;
                playerController.SprintSpeed = currentWeaponScript.AimSpeed;
                if (recoilModule != null)
                {
                    recoilModule.SetRecoilSpecs(currentWeaponScript.RotationalAimRecoil, currentWeaponScript.PositionalAimRecoil, currentWeaponScript.RecoilSnappiness, currentWeaponScript.RecoilReturnSpeed);
                }
            }
            else
            {
                playerController.MoveSpeed = currentWeaponScript.MoveSpeed;
                playerController.SprintSpeed = currentWeaponScript.SprintSpeed;
                if (recoilModule != null)
                {
                    recoilModule.SetRecoilSpecs(currentWeaponScript.RotationalRecoil, currentWeaponScript.PositionalRecoil, currentWeaponScript.RecoilSnappiness, currentWeaponScript.RecoilReturnSpeed);
                }
            }
        }
    }

    public void NextWeapon()
    {
        if (currentWeaponIndex + 1 < weapons.Length)
        {
            currentWeaponIndex++;
        } 
        else
        {
            currentWeaponIndex = 0;
        }
        EquipWeaponByIndex(currentWeaponIndex);
    }

    public void PreviousWeapon()
    {
        if (currentWeaponIndex - 1 >= 0)
        {
            currentWeaponIndex--;
        }
        else
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        EquipWeaponByIndex(currentWeaponIndex);
    }
}
