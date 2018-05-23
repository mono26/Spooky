using UnityEngine;

public class SoundManager : PersistenSingleton<SoundManager>
{
    [SerializeField]
    private AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    [SerializeField]
    private AudioSource musicSource;                 //Drag a reference to the audio source which will play the music. 

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

    public void StopSound()
    {
        musicSource.Stop();
        efxSource.Stop();
    }
}
