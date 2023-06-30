using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject[] openDoors;
    [SerializeField] private GameObject[] closedDoors;
    [SerializeField] private GameObject triggerDoor;
    private LevelInfo levelInfo;

    private void Start()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("LevelInfo");
        levelInfo = levelObject.GetComponent<LevelInfo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < openDoors.Length; i++)
        {
            closedDoors[i].SetActive(true);
            openDoors[i].SetActive(false);
        }

        levelInfo.roomCounter--;
        triggerDoor.SetActive(false);
    }
}
