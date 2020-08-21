using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{

    private static AudioClip[] musicClips = new AudioClip[2];
    private static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        musicClips[0] = Resources.Load<AudioClip> ("Sounds/music1");
        musicClips[1] = Resources.Load<AudioClip> ("Sounds/music1"); // boss song

        audioSrc = GetComponent<AudioSource>();
    }

    public void SwitchBossMusic(){
        StartCoroutine(StartFade(audioSrc, 3f, 0f, 0f));
        audioSrc.clip = musicClips[1];
        StartCoroutine(StartFade(audioSrc, 1f, 1f, 3f));
    }

    private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, float delay)
    {
        yield return new WaitForSeconds(delay);
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

