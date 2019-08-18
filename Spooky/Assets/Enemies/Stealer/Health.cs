using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Health : MonoBehaviour, Damagable
{
    [Header("Health")]
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private float healthBarToggleTime;

    [Header("Damage")]
    [SerializeField]
    private GameObject damageVfx;
    [SerializeField]
    private AudioClip damageSfx;
    [SerializeField]
    private GameObject deathVfx;

    [Header("Possible drop")]
    [SerializeField]
    private Pickable drop;

    [SerializeField]
    private float currentHealth;
    private Character character;

    protected void Awake()
    {
        character = GetComponent<Character>();
        return;
    }

    protected void Start()
    {
        if (character.GetComponent<StatsComponent>())
        {
            maxHealth = character.GetComponent<StatsComponent>().MaxHealth;
            currentHealth = maxHealth;
        }
        return;
    }

    protected void OnEnable()
    {
        currentHealth = maxHealth;
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

        StartCoroutine(ToggleHealthBar());
        currentHealth -= _damage;

        VisualEffects.CreateVisualEffect(damageVfx, transform);
        PlayHitSfx();

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
        if (damageSfx != null)
        {
            SoundManager.Instance.PlaySfx(character.CharacterAudioSource, damageSfx);
        }
        return;
    }

    public IEnumerator Kill()
    {
        
        PoolableObject poolableCharacter = GetComponent<PoolableObject>();
        if (poolableCharacter != null)
        {
            yield return CharacterDeath();

            poolableCharacter.Release();

        }
        else if (poolableCharacter == null)
        {
            yield return CharacterDeath();

            Destroy(gameObject);
        }

        VisualEffects.CreateVisualEffect(deathVfx, transform);
        yield break;
    }

    private IEnumerator CharacterDeath()
    {
        if (character != null)
        {
            EventManager.TriggerEvent<CharacterEvent>(new CharacterEvent(CharacterEventType.Death, character));

            character.characterStateMachine.ChangeState(Character.CharacterState.Dead);
            yield return null;

            yield return new WaitForSecondsRealtime(
            character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + 0.15f
            );

            if (drop != null)
            {
                AICharacter isAICharacter = character as AICharacter;
                if (isAICharacter != null)
                {
                    Pickable _drop = Instantiate(drop, transform.position, transform.rotation);
                    _drop.InitializePickableValue(isAICharacter.Reward);
                }
            }
        }

        yield break;
    }

}
