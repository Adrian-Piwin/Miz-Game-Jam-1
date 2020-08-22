using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{

    public AudioSource audioSrc;
    public AudioSource audioSrcBoss;

    public void SwitchBossMusic(){
        StartCoroutine(StartFade(audioSrc, 4f, 0f, 0f, false));
        audioSrcBoss.time = 7f;
        StartCoroutine(StartFade(audioSrcBoss, 3f, PlayerPrefs.GetFloat("MusicVolume", 0.5f), 4f, true));
    }

    public void endBossMusic(){
        StartCoroutine(StartFade(audioSrcBoss, 10f, 0f, 0f, false));
    }

    private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, float delay, bool toggle)
    {
        yield return new WaitForSeconds(delay);

        if (toggle)
            audioSource.Play();

        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if (!toggle)
            audioSource.Stop();

        yield break;
        
    }
}

