using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    private AudioSource source;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        source = Camera.main.GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip _audioClip)
    {
        source.PlayOneShot(_audioClip, 0.2f);
    }
}
