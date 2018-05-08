using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    protected Character character;
    [SerializeField]
    protected AudioClip startSfx;
    [SerializeField]
    protected AudioClip progressSfx;
    [SerializeField]
    protected AudioClip stopSfx;

    protected bool isInitialized;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public virtual void EveryFrame()
    {
        HandleInput();
    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void PlayAbilityStartSfx()
    {
        if (startSfx != null)
        {
            //SoundManager.Instance.PlaySound(AbilityStartSfx, transform.position);
        }
    }

    /// <summary>
    /// Plays the ability used sound effect
    /// </summary>
    protected virtual void PlayAbilityUsedSfx()
    {
        if (progressSfx != null)
        {
            if (character.CharacterAudioSource != null)
            {
                //character.CharacterAudioSource = SoundManager.Instance.PlaySound(AbilityInProgressSfx, transform.position, true);
            }
        }
    }

    /// <summary>
    /// Stops the ability used sound effect
    /// </summary>
    protected virtual void StopAbilityUsedSfx()
    {
        if (progressSfx != null)
        {
            //SoundManager.Instance.StopLoopingSound(_abilityInProgressSfx);
            //_abilityInProgressSfx = null;
        }
    }

    /// <summary>
    /// Plays the ability stop sound effect
    /// </summary>
    protected virtual void PlayAbilityStopSfx()
    {
        if (stopSfx != null)
        {
            //SoundManager.Instance.PlaySound(AbilityStopSfx, transform.position);
        }
    }

    protected virtual void HandleInput()
    {

    }
}
