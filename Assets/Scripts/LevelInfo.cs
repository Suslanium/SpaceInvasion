using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    public int roomCounter = 25;
    public int levelCounter = 0;
    [SerializeField] private int gameLevelSceneIndex;
    [SerializeField] private GameObject player;

    private void Start()
    {
        SceneManager.LoadScene(gameLevelSceneIndex);
    }

    public void EnablePlayer()
    {
        player.SetActive(true);
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
