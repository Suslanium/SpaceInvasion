using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs;
    private AudioSource audioSource;
    private int currentSongIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNextSong();
    }

    void PlayNextSong()
    {
        if (currentSongIndex >= songs.Length)
        {
            currentSongIndex = 0;
        }
        
        audioSource.PlayOneShot(songs[currentSongIndex]);

        currentSongIndex++;
        
        Invoke("PlayNextSong", audioSource.clip.length);
    }
}