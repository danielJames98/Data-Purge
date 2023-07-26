using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlayerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> songs;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
        audioSource= this.GetComponent<AudioSource>();
        audioSource.clip= songs[Random.Range(0, songs.Count)];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.isPlaying==false) 
        { 
            audioSource.clip = songs[Random.Range(0, songs.Count)]; 
            audioSource.Play();
        }
    }
}