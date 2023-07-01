using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour, IInteractable
{
    private LevelInfo levelInfo;
    private GameObject playerCapsule;

    private void Start()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("LevelInfo");
        levelInfo = levelObject.GetComponent<LevelInfo>();
        playerCapsule = GameObject.Find("PlayerCapsule");
    }

    public void Interact()
    {
        if (levelInfo.roomCounter == 0)
        {
            if (levelInfo.levelCounter < 3)
            {
                playerCapsule.SetActive(false);
                playerCapsule.transform.position = new Vector3(0, 0, 4);
                playerCapsule.transform.rotation = Quaternion.identity;
                playerCapsule.SetActive(true);
                levelInfo.levelCounter++;
                levelInfo.roomCounter = 25;
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.Log("Ты зачистил корабль!");
            }
        }
    }
}
