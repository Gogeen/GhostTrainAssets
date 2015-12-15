using UnityEngine;
using System.Collections;
public class TownSoundController : MonoBehaviour {

    public AudioSource source;
    public AudioClip startSound;
    public AudioClip cycleSound;
    public AudioClip endSound;
    bool playSound = false;
    IEnumerator PlaySound()
    {
        source.clip = startSound;
        source.Play();
        while (source.isPlaying)
            yield return null;
        source.clip = cycleSound;
        source.Play();
        source.loop = true;

    }

    /*void Update()
    {
        if (GetComponent<TownUI>().townUI.activeSelf && !playSound)
        {
            playSound = true;
            StartCoroutine("PlaySound");
        }
        else if (!GetComponent<TownUI>().townUI.activeSelf && playSound)
        {
            playSound = false;
            source.loop = false;
            source.clip = endSound;
            source.Play();
        }
    }*/
}
