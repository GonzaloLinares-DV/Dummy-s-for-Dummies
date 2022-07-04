using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClip : MonoBehaviour
{
    public AudioSource playAudio;

    public void PlayAudioClip()
    {
        playAudio.Play();
    }
}
