﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Health : MonoBehaviour, Damagable
{
    //AIEffectsHandler effects;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private float healthBarToggleTime;
    [SerializeField]
    private GameObject damageEffect;
    [SerializeField]
    private AudioClip damageSfx;

    [SerializeField]
    private float currentHealth;

    private Character character;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;
    public delegate void OnRespawnDelegate();
    public event OnRespawnDelegate OnRespawn;

    protected void Awake()
    {
        character = GetComponent<Character>();
        return;
    }

    protected void Start()
    {
        Enemy enemyCharacter = GetComponent<Enemy>();
        if (enemyCharacter)
        {
            maxHealth = enemyCharacter.StatsComponent.MaxHealth;
            currentHealth = maxHealth;
            return;
        }
        else return;
    }

    protected void OnEnable()
    {
        currentHealth = maxHealth;
        // Need to cast to float because the result of "/" by two ints gives a int.
        if(healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            healthBar.gameObject.SetActive(false);
        }
        return;
    }

    public void TakeDamage(float _damage)
    {
        // We are laready dead.
        if(currentHealth == 0) { return; }

        //var feathersP = Instantiate(controller.feathersParticle, transform.position, Quaternion.Euler(-90, 0, 0));
        StartCoroutine(ToggleHealthBar());
        currentHealth -= _damage;
        currentHealth = Mathf.Max(0, currentHealth);
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth == 0)
            StartCoroutine(Kill());
        return;
    }

    private IEnumerator ToggleHealthBar()
    {
        if(healthBar != null)
        {
            healthBar.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(healthBarToggleTime);

            healthBar.gameObject.SetActive(false);
        }
        yield break;
    }

    private void PlayHitSfx()
    {
        /*if (DamageSfx != null)
        {
            SoundManager.Instance.PlaySound(DamageSfx, transform.position);
        }*/
    }

    private IEnumerator Kill()
    {
        if (OnDeath != null)
            OnDeath();

        PoolableObject poolableCharacter = GetComponent<PoolableObject>();
        if (poolableCharacter != null)
        {
            // Start poolable death.
            yield return CharacterDeath();

            poolableCharacter.Release();

        }
        else if (poolableCharacter == null)
        {
            // Destroy object.
            yield return CharacterDeath();

            gameObject.SetActive(false);
        }
        yield break;
    }

    private IEnumerator CharacterDeath()
    {
        if (character != null)
        {
            character.characterStateMachine.ChangeState(Character.CharacterState.Dead);

            yield return 0;

            yield return new WaitForSecondsRealtime(
            character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + 0.15f
            );
        }
        yield break;
    }

}
