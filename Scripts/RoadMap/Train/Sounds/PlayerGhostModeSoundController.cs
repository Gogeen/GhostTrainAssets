using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerTrain))]
public class PlayerGhostModeSoundController : MonoBehaviour {

    public AudioSource source;
    public AudioClip startSound;
    public AudioClip cycleSound;
    public AudioClip endSound;
    bool startGM = false;

    /*IEnumerator PlaySound()
    {
        source.clip = startSound;
        source.Play();
        while (source.isPlaying)
            yield return null;
        source.clip = cycleSound;
        source.Play();
        source.loop = true;

    }*/

    void Update()
    {
        
    }

}
