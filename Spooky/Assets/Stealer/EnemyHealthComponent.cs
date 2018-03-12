using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : IDamagable
{
    //AIEffectsHandler effects;
    private Enemy owner;
    private Settings settings;
    private Coroutine healthTogle;

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
        settings.HealthBar.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(int _damage)
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
        var time = Time.timeSinceLevelLoad;
        Debug.Log(time + " health bar " + settings.HealthBar.gameObject);
        settings.HealthCanvas.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(settings.HealthBarToggleTime);
        settings.HealthCanvas.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class Settings
    {
        public Image HealthBar;
        public GameObject HealthCanvas;
        public float HealthBarToggleTime;
    }
}
