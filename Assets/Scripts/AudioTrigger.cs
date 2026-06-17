using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] public GameObject catModel;
    AudioSource audioSource;
    [SerializeField] public AudioClip soundClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (audioSource == null || soundClip == null)
            return;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}
