using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerTrain))]
public class PlayerMoveSoundController : MonoBehaviour {

    public AudioSource source;
    public AudioClip startSound;
    public AudioClip cycleSound;
    bool stopped = true;

    IEnumerator PlaySound()
    {
        source.clip = startSound;
        source.Play();
        while (source.isPlaying)
            yield return null;
        source.clip = cycleSound;
        source.Play();
        source.loop = true;
        /* while (true)
        {
            if (!source.isPlaying)
                source.Play();
            yield return null;
        }*/
    }

    void Update()
    {
        if (GetComponent<PlayerTrain>().speed == 0)
        {
            stopped = true;
            StopCoroutine("PlaySound");
            source.Stop();
            return;
        }
        if (stopped)
        {
            stopped = false;
            StartCoroutine("PlaySound");
        }
    }
}
