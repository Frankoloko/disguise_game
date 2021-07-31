using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float initialHealth = 100;
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = initialHealth;
    }

    public virtual void TakeDamage(float damageTaken)
    {
        _currentHealth -= damageTaken;

        Debug.Log($"{gameObject.name} health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }        
    }

    public virtual void TakeFullDamage()
    {
        TakeDamage(_currentHealth);
    }
}
