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
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField] private GameObject fighterDominantAbilityPrefab;
    [SerializeField] private Transform fighterDominantAbilityInstantiationPoint;
    [SerializeField] private GameObject companionPrefab;
    [SerializeField] private ParticleSystem aiRings;
    [SerializeField] private ParticleSystem healRings;
    [SerializeField] private ParticleSystem immunityRings;
    private GameObject currentWeaponGameobject;
    private PlayerFireArm currentWeaponScript;
    private PlayerMeleeWeapon currentMeleeWeapon;
    private float currentTargetFOV;
    private int currentWeaponIndex = 0;
    private PlayerHero _playerHero;
    private float nextAbilityTime = 0;

    private void Start()
    {
        _playerHero = PlayerRepository.PlayerHero;
        if (_playerHero != null)
        {
            _characterStats.InitStats(_playerHero.HealthPoints, _playerHero.ArmorPoints);
        }
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
        playerController.MoveSpeed = (currentWeaponScript != null ? currentWeaponScript.MoveSpeed : currentMeleeWeapon.MoveSpeed) * _playerHero.MovementSpeedMultiplier;
        playerController.SprintSpeed = (currentWeaponScript != null ? currentWeaponScript.SprintSpeed : currentMeleeWeapon.SprintSpeed) * _playerHero.MovementSpeedMultiplier;
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

    public void FirstAbility()
    {
        if (Time.time > nextAbilityTime)
        {
            InvokeAbility(_playerHero.DominantAbility);
        }
    }

    public void SecondAbility()
    {
        if (Time.time > nextAbilityTime)
        {
            InvokeAbility(_playerHero.RecessiveAbility);
        }
    }

    private void InvokeAbility(Ability ability)
    {
        switch(ability)
        {
            case Ability.TankDominantAbilityName:
                var hitObjects = Physics.SphereCastAll(gameObject.transform.position, 50f, gameObject.transform.up, 1f);
                foreach(var hitObject in hitObjects)
                {
                    var enemy = hitObject.transform.gameObject.GetComponentInChildren<CharacterControlModule>();
                    if (enemy != null)
                    {
                        enemy.DisableAIForSeconds(5f);
                    }
                }
                nextAbilityTime = Time.time + 25f;
                aiRings.Play();
                break;
            case Ability.TankRecessiveAbilityName:
                _characterStats.DamageImmunityForSeconds(5f);
                nextAbilityTime = Time.time + 25f;
                immunityRings.Play();
                break;
            case Ability.FighterDominantAbilityName:
                if (playerController.Grounded)
                {
                    Instantiate(fighterDominantAbilityPrefab, fighterDominantAbilityInstantiationPoint.transform.position, fighterDominantAbilityInstantiationPoint.transform.rotation);
                }
                nextAbilityTime = Time.time + 15f;
                break;
            case Ability.FighterRecessiveAbilityName:
                _characterStats.Heal(45);
                nextAbilityTime = Time.time + 5f;
                healRings.Play();
                break;
            case Ability.MarksmanDominantAbilityName:
                Instantiate(companionPrefab, fighterDominantAbilityInstantiationPoint.transform.position, fighterDominantAbilityInstantiationPoint.transform.rotation);
                nextAbilityTime = Time.time + 60f;
                break;
            case Ability.MarksmanRecessiveAbilityName:
                _characterStats.Heal(45);
                nextAbilityTime = Time.time + 5f;
                healRings.Play();
                break;
            default:
                break;
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
