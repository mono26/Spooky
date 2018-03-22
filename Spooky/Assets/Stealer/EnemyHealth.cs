﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : IDamagable
{
    //AIEffectsHandler effects;
    private Enemy owner;
    private Settings settings;

    public float maxHealth;
    public float currentHealth;

    public EnemyHealth(Enemy _owner, int _maxHealth, Settings _settings)
    {
        owner = _owner;
        maxHealth = _maxHealth;
        settings = _settings;
    }

    public void Start()
    {
        RestartValues();
    }

    public void RestartValues()
    {
        currentHealth = maxHealth;
        // Need to cast to float because the result of "/" by two ints gives a int.
        settings.HealthBar.fillAmount = currentHealth / maxHealth;
        settings.HealthBar.gameObject.SetActive(false);
    }

    public void TakeDamage(float _damage)
    {
        //var feathersP = Instantiate(controller.feathersParticle, transform.position, Quaternion.Euler(-90, 0, 0));
        owner.StartCoroutine(ToggleHealthBar());
        currentHealth = Mathf.Max(0, currentHealth - _damage);
        settings.HealthBar.fillAmount = currentHealth / maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator ToggleHealthBar()
    {
        settings.HealthBar.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(settings.HealthBarToggleTime);

        settings.HealthBar.gameObject.SetActive(false);

    }

    [System.Serializable]
    public class Settings
    {
        public Image HealthBar;
        public float HealthBarToggleTime;
    }
}
