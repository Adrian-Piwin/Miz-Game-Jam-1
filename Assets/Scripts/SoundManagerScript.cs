using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    private static AudioClip[] audioClips = new AudioClip[7];
    private static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioClips[0] = Resources.Load<AudioClip> ("Sounds/enemyhit");
        audioClips[1] = Resources.Load<AudioClip> ("Sounds/heartpickup");
        audioClips[2] = Resources.Load<AudioClip> ("Sounds/jump");
        audioClips[3] = Resources.Load<AudioClip> ("Sounds/reflection");
        audioClips[4] = Resources.Load<AudioClip> ("Sounds/playerdeath");
        audioClips[5] = Resources.Load<AudioClip> ("Sounds/bosshit");
        audioClips[6] = Resources.Load<AudioClip> ("Sounds/bossphase");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip){
        switch (clip) {
            case "enemyhit":
                audioSrc.PlayOneShot(audioClips[0]);
                break;
            case "heartpickup":
                audioSrc.PlayOneShot(audioClips[1]);
                break;
            case "jump":
                audioSrc.PlayOneShot(audioClips[2]);
                break;
            case "reflection":
                audioSrc.PlayOneShot(audioClips[3]);
                break;
            case "playerdeath":
                audioSrc.PlayOneShot(audioClips[4]);
                break;
            case "bosshit":
                audioSrc.PlayOneShot(audioClips[5]);
                break;
            case "bossphase":
                audioSrc.PlayOneShot(audioClips[6]);
                break;
            default:
                break;

        }
    }
}
