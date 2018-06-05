using UnityEngine;

public class SoundManager : PersistenSingleton<SoundManager>
{
    [SerializeField]
    private AudioSource musicSource;

    public void PlaySfx(AudioSource _source , AudioClip _audioClip)
    {
        if (_source.loop == true)
            _source.loop = false;

        _source.clip = _audioClip;

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
