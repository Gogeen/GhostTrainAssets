using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(PlayerTrain))]
public class PlayerMoveSoundController : MonoBehaviour {

    public AudioSource source;
    public AudioClip startSound;
    public AudioClip cycleSound;
    bool movedOnce = false;
    
    void Update()
    {
        if (GetComponent<PlayerTrain>().speed == 0)
        {
            source.Stop();
            return;
        }
        if (source.isPlaying)
            return;
        if (!movedOnce)
        {
            movedOnce = true;
            source.clip = startSound;
            source.Play();
        }
        else
        {
            source.clip = cycleSound;
            source.Play();
            source.loop = true;
        }
        
    }
}
