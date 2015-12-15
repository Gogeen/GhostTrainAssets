using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBackgroundSoundController : MonoBehaviour {

    public AudioSource source;
    public List<AudioClip> sounds = new List<AudioClip>();

    IEnumerator PlaySound()
    {
        if (sounds.Count == 0)
            yield break;
        int currentSoundIndex = 0;
        source.clip = sounds[currentSoundIndex];
        source.Play();
        while (true)
        {
            if (!source.isPlaying)
            {
                currentSoundIndex += 1;
                if (currentSoundIndex >= sounds.Count)
                    currentSoundIndex = 0;
                source.clip = sounds[currentSoundIndex];
                source.Play();
            }
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine("PlaySound");
    }
}
