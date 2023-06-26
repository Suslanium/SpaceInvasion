using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    // 0 - Up, 1 - Down, 2 - Right, 3 - Left
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject room;
    public int corridorTriggerOffset;
    public string playerTag = "Player";
    public BoxCollider trigger;

    public void UpdateRoom(bool[] status)
    {
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