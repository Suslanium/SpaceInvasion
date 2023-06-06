using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputManager : MonoBehaviour
{
	[SerializeField] private StarterAssets.StarterAssetsInputs starterAssetsInputs;
	[SerializeField] private WeaponSway swayModule;
	[SerializeField] private WeaponManager weaponManager;
#if ENABLE_INPUT_SYSTEM
	[SerializeField] private PlayerInput playerInput;
	[SerializeField] private string fireActionName;
	private InputAction fireAction;
	private bool firePressedState = false;
	public void OnMove(InputValue value)
	{
		starterAssetsInputs.MoveInput(value.Get<Vector2>());
		if(swayModule != null)
        {
			swayModule.UpdateMoveInput(value.Get<Vector2>());
        }
	}

	public void OnLook(InputValue value)
	{
		starterAssetsInputs.LookInput(value.Get<Vector2>());
		if (swayModule != null)
		{
			swayModule.UpdateMouseInput(value.Get<Vector2>());
		}
	}

	public void OnJump(InputValue value)
	{
		starterAssetsInputs.JumpInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		starterAssetsInputs.SprintInput(value.isPressed);
	}

	public void OnFire()
    {
		if (weaponManager != null)
        {
			weaponManager.Fire();
        }
    }

	public void OnAim()
    {
		if (weaponManager != null)
        {
			weaponManager.Aim();
        }
    }

	public void OnReload()
    {
		if (weaponManager != null)
        {
			weaponManager.Reload();
        }
    }

	public void OnEquip()
    {
		if (weaponManager != null)
        {
			weaponManager.Equip();
        }
    }

	public void OnChangeWeapon(InputValue value)
    {
		if (weaponManager != null)
        {
			float input = value.Get<float>();
			if (input > 0)
            {
				weaponManager.NextWeapon();
            }
			else if (input < 0)
            {
				weaponManager.PreviousWeapon();
            }
        }
    }

    private void Start()
    {
		if (playerInput != null)
		{
			fireAction = playerInput.actions[fireActionName];
		}
    }

    private void Update()
    {
        if (fireAction != null && weaponManager != null && fireAction.IsPressed() != firePressedState)
        {
			firePressedState = fireAction.IsPressed();
			weaponManager.SetHoldingFireState(firePressedState);
        }
    }
#endif
}
