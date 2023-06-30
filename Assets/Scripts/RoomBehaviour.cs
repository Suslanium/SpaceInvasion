using UnityEngine;
using UnityEngine.UI;

public class RoomBehaviour : MonoBehaviour
{
    // 0 - Up, 1 - Down, 2 - Right, 3 - Left
    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject[] openDoors;
    [SerializeField] private GameObject[] closedDoors;
    [SerializeField] private GameObject[] enemyShooter;
    [SerializeField] private GameObject[] enemyAttacking;
    [SerializeField] private GameObject[] enemyTurret;
    [SerializeField] private Transform[] spawnPointsShoot;
    [SerializeField] private Transform[] spawnPointsAttacking;
    [SerializeField] private Transform[] spawnPointsTurret;
    [SerializeField] private GameObject room;
    [SerializeField] private int corridorTriggerOffset;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private BoxCollider trigger;
    [SerializeField] private int enemyCounter = 0;
    private LevelInfo levelInfo;
    private TriggerDoor startTrigger;

    public void UpdateRoom(bool[] status)
    {
        startTrigger = gameObject.GetComponentInChildren<TriggerDoor>();
        GameObject levelObject = GameObject.FindGameObjectWithTag("LevelInfo");
        levelInfo = levelObject.GetComponent<LevelInfo>();

        int precent = 60;

        switch (levelInfo.levelCounter)
        {
            case 0:
                precent = 60;
                break;
            case 1:
                precent = 70;
                break;
            case 2:
                precent = 80;
                break;
            case 3:
                precent = 90;
                break;
            default:
                break;
        }

        for (int i = 0; i < spawnPointsShoot.Length; i++)
        {

            if (Random.Range(0, 101) <= precent)
            {
                GameObject newEnemy = Instantiate(enemyShooter[Random.Range(0, enemyShooter.Length)], spawnPointsShoot[i].position, spawnPointsShoot[i].rotation) as GameObject;
                newEnemy.transform.SetParent(spawnPointsShoot[i]);
                newEnemy.GetComponent<CharacterStats>().SetOwnerRoom(this);
                startTrigger.AddEnemy(newEnemy.GetComponent<CharacterControlModule>());
                enemyCounter++;
            }
        }

        for (int i = 0; i < spawnPointsAttacking.Length; i++)
        {
            if (Random.Range(0, 101) <= precent)
            {
                GameObject newEnemy = Instantiate(enemyAttacking[Random.Range(0, enemyAttacking.Length)], spawnPointsAttacking[i].position, spawnPointsAttacking[i].rotation) as GameObject;
                newEnemy.transform.SetParent(spawnPointsAttacking[i]);
                newEnemy.GetComponent<CharacterStats>().SetOwnerRoom(this);
                startTrigger.AddEnemy(newEnemy.GetComponent<CharacterControlModule>());
                enemyCounter++;
            }
        }

        for (int i = 0; i < spawnPointsTurret.Length; i++)
        {
            if (Random.Range(0, 101) <= precent)
            {
                GameObject newEnemy = Instantiate(enemyTurret[Random.Range(0, enemyTurret.Length)], spawnPointsAttacking[i].position, spawnPointsAttacking[i].rotation) as GameObject;
                newEnemy.transform.SetParent(spawnPointsAttacking[i]);
                newEnemy.GetComponent<CharacterStats>().SetOwnerRoom(this);
                startTrigger.AddEnemy(newEnemy.GetComponent<CharacterControlModule>());
                enemyCounter++;
            }
        }

        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
            if (status[i])
            {
                switch(i)
                {
                    case 0:
                        trigger.center = new Vector3(trigger.center.x, trigger.center.y, trigger.center.z + corridorTriggerOffset / 2);
                        trigger.size = new Vector3(trigger.size.x, trigger.size.y, trigger.size.z + corridorTriggerOffset);
                        break;
                    case 1:
                        trigger.center = new Vector3(trigger.center.x, trigger.center.y, trigger.center.z - corridorTriggerOffset / 2);
                        trigger.size = new Vector3(trigger.size.x, trigger.size.y, trigger.size.z + corridorTriggerOffset);
                        break;
                    case 2:
                        trigger.center = new Vector3(trigger.center.x + corridorTriggerOffset / 2, trigger.center.y, trigger.center.z);
                        trigger.size = new Vector3(trigger.size.x + corridorTriggerOffset, trigger.size.y, trigger.size.z);
                        break;
                    case 3:
                        trigger.center = new Vector3(trigger.center.x - corridorTriggerOffset / 2, trigger.center.y, trigger.center.z);
                        trigger.size = new Vector3(trigger.size.x + corridorTriggerOffset, trigger.size.y, trigger.size.z);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void DisableRoom()
    {
        room.SetActive(false);
    }

    public void deathEnemy()
    {
        enemyCounter--;

        if (enemyCounter == 0)
        {
            for (int i = 0; i < openDoors.Length; i++)
            {
                closedDoors[i].SetActive(false);
                openDoors[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag && !room.activeSelf)
        {
            room.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == playerTag && room.activeSelf)
        {
            room.SetActive(false);
        }
    }
}