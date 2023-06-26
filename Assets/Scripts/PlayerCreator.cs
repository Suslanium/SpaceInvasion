using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCreator : MonoBehaviour, IInteractable
{
    [SerializeField]
    public GameObject panel;
    
    private PlayerInput playerInput;

    private void Start()
    {
        GameObject[] playerCapsules;
        playerCapsules = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (var item in playerCapsules)
        {
            PlayerInput pi = item.GetComponent<PlayerInput>();
            if (pi != null)
            {
                playerInput = pi;
                break;
            }
        }
    }
    
    public void Interact()
    {
        if (panel != null)
        {
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = Time.deltaTime * Time.timeScale;

            playerInput.enabled = false;
            panel.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
