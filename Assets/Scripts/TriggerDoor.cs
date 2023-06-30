using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject[] openDoors;
    [SerializeField] private GameObject[] closedDoors;
    [SerializeField] private GameObject triggerDoor;
    private List<CharacterControlModule> enemyControllers = new List<CharacterControlModule>();
    private LevelInfo levelInfo;

    private void Start()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("LevelInfo");
        levelInfo = levelObject.GetComponent<LevelInfo>();
    }

    public void AddEnemy(CharacterControlModule controlModule)
    {
        enemyControllers.Add(controlModule);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            for (int i = 0; i < openDoors.Length; i++)
            {
                closedDoors[i].SetActive(true);
                openDoors[i].SetActive(false);
            }

            levelInfo.roomCounter--;
            foreach (CharacterControlModule controller in enemyControllers)
            {
                controller.EnableAI();
            }
            triggerDoor.SetActive(false);
        }
    }
}
