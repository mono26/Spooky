using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundManager
{
    private AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    private AudioSource musicSource;                 //Drag a reference to the audio source which will play the music. 

    public SoundManager(AudioSource _efxSource, AudioSource _musicSource)
    {
        efxSource = _efxSource;
        musicSource = _musicSource;
    }

    public void PlayClip(AudioClip _audioClip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = _audioClip;

        //Play the clip.
        efxSource.Play();
        //efxSource.PlayOneShot(_audioClip, 0.2f);
    }

    public void PlayMusic(AudioClip _musicClip)
    {
        musicSource.clip = _musicClip;

        musicSource.Play();
    }
}
