using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : IDamagable
{
    //AIEffectsHandler effects;
    private Enemy enemy;
    private Settings settings;

    private float currentHealth;

    public EnemyHealth(Enemy _owner, Settings _settings)
    {
        enemy = _owner;
        settings = _settings;
    }

    public void Start()
    {
        RestartValues();
    }

    public void RestartValues()
    {
        currentHealth = settings.MaxHealth;
        // Need to cast to float because the result of "/" by two ints gives a int.
        settings.HealthBar.fillAmount = currentHealth / settings.MaxHealth;
        settings.HealthBar.gameObject.SetActive(false);
    }

    public void TakeDamage(float _damage)
    {
        //var feathersP = Instantiate(controller.feathersParticle, transform.position, Quaternion.Euler(-90, 0, 0));
        enemy.StartCoroutine(ToggleHealthBar());
        currentHealth = Mathf.Max(0, currentHealth - _damage);
        settings.HealthBar.fillAmount = currentHealth / settings.MaxHealth;
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
        public float MaxHealth;
        public float HealthBarToggleTime;
    }
}
