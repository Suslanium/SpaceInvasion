using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class PlayerCreator : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject UIDocument;

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
        if (UIDocument != null)
        {
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = Time.deltaTime * Time.timeScale;

            playerInput.enabled = false;
            UIDocument.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}