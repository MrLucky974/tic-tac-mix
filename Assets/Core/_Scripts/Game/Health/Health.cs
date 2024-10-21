using System;
using UnityEngine;

/// <summary>
/// Manages health for an entity, allowing for damage, healing, and death events.
/// Implements IDamageable to enable interaction with any damageable object.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_maxHealth = 100;  // The maximum health of the entity.
    private int m_currentHealth;                       // The current health of the entity.

    // Event triggered when health changes, passing the current health value.
    public event Action<HealthChangedEvent> OnHealthChanged;

    // Event triggered when the entity dies.
    public event Action OnDeath;

    private void Awake()
    {
        // Initialize current health to maximum health.
        m_currentHealth = m_maxHealth;
    }

    /// <summary>
    /// Gets the current health of the entity.
    /// </summary>
    public int CurrentHealth => m_currentHealth;

    /// <summary>
    /// Gets the maximum health of the entity.
    /// </summary>
    public int MaxHealth => m_maxHealth;

    /// <summary>
    /// Checks if the entity is alive.
    /// </summary>
    public bool IsAlive => m_currentHealth > 0;

    /// <summary>
    /// Gets the health ratio, a value between 0 and 1 representing the current health 
    /// relative to the maximum health.
    /// </summary>
    public float Ratio => (float)m_currentHealth / m_maxHealth;

    /// <summary>
    /// Applies damage to the entity.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || !IsAlive) return;

        m_currentHealth -= amount;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        var @event = new HealthChangedEvent()
        {
            current = m_currentHealth,
            max = m_maxHealth,
            ratio = Ratio,
        };
        OnHealthChanged?.Invoke(@event);

        if (m_currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the entity by a specified amount.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        if (amount <= 0 || !IsAlive) return;

        m_currentHealth += amount;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        var @event = new HealthChangedEvent()
        {
            current = m_currentHealth,
            max = m_maxHealth,
            ratio = Ratio,
        };
        OnHealthChanged?.Invoke(@event);
    }

    /// <summary>
    /// Immediately reduces the entity's health to zero, triggering death.
    /// </summary>
    public void Kill()
    {
        if (IsAlive)
        {
            m_currentHealth = 0;

            var @event = new HealthChangedEvent()
            {
                current = m_currentHealth,
                max = m_maxHealth,
                ratio = Ratio,
            };
            OnHealthChanged?.Invoke(@event);
            Die();
        }
    }

    /// <summary>
    /// Handles the death of the entity, triggering the OnDeath event.
    /// </summary>
    private void Die()
    {
        OnDeath?.Invoke();
    }

    /// <summary>
    /// Sets a new maximum health value for the entity.
    /// Optionally adjusts current health to the new maximum.
    /// </summary>
    /// <param name="newMaxHealth">The new maximum health value.</param>
    /// <param name="adjustCurrentHealth">If true, sets current health to the new maximum.</param>
    public void SetMaxHealth(int newMaxHealth, bool adjustCurrentHealth = true)
    {
        m_maxHealth = newMaxHealth;
        if (adjustCurrentHealth)
        {
            m_currentHealth = m_maxHealth;

            var @event = new HealthChangedEvent()
            {
                current = m_currentHealth,
                max = m_maxHealth,
                ratio = Ratio,
            };
            OnHealthChanged?.Invoke(@event);
        }
    }
}
