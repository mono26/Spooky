using UnityEngine;

public class SoundManager : PersistenSingleton<SoundManager>
{                //Drag a reference to the audio source which will play the sound effects.
    [SerializeField]
    private AudioSource musicSource;                 //Drag a reference to the audio source which will play the music. 

    public void PlaySfx(AudioSource _source , AudioClip _audioClip)
    {
        // So we dont 
        if (_source.loop == true)
            _source.loop = false;

        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        _source.clip = _audioClip;

        //Play the clip.
        _source.Play();
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
        return;
    }
}
