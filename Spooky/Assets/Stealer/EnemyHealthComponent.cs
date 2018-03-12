using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : IDamagable
{
    //AIEffectsHandler effects;
    private Enemy owner;
    private Settings settings;

    public int maxHealth;
    public int currentHealth;

    public EnemyHealthComponent(Enemy _owner, int _maxHealth, Settings _settings)
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
        settings.healthBar.fillAmount = maxHealth;
    }

    public void TakeDamage(int _damage)
    {
        //var feathersP = Instantiate(controller.feathersParticle, transform.position, Quaternion.Euler(-90, 0, 0));
        owner.StartCoroutine(ToggleHealthBar());
        currentHealth = Mathf.Max(0, currentHealth - _damage);
        settings.healthBar.fillAmount = currentHealth / maxHealth;
        //Debug.Log("Recibiendo daño" + controller.gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator ToggleHealthBar()
    {
        settings.healthBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(settings.HealthBarToggleTime);
        settings.healthBar.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class Settings
    {
        public Image healthBar;
        public float HealthBarToggleTime;
    }
}
