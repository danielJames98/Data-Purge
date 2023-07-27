using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundPlayerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public bool soundPlayed;

    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    void Update()
    {
        if(soundPlayed && audioSource.isPlaying==false)
        {
            Destroy(this.gameObject);
        }
    }

    void playSound()
    {
        if (audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
            soundPlayed = true;
        }
    }
}
