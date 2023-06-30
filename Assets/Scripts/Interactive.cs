using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactive : MonoBehaviour
{
    private LevelInfo levelInfo;
    [SerializeField] private GameObject player;

    private void Start()
    {
        GameObject levelObject = GameObject.FindGameObjectWithTag("LevelInfo");
        levelInfo = levelObject.GetComponent<LevelInfo>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, 4))
        {
            if ((hit.collider.tag == "Finish") && (levelInfo.roomCounter == 0))
            {
                if (levelInfo.levelCounter < 3)
                {
                    player.SetActive(false);
                    player.transform.position = new Vector3(0, 0, 4);
                    player.SetActive(true);
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
}
