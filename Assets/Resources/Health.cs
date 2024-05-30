using UnityEngine;

public class Health : Resource
{
    [Header("REFERENCES")]
    [SerializeField] private HealthUI _healthUI;

    private void Start()
    {
        InitialiseValues();
        HealthInitialised(MaxValue);
        HealthChanged(CurrentValue);
    }

    public void Heal(float amount)
    {
        AddValue(amount);
        HealthChanged(CurrentValue);
    }

    public void TakeDamage(float amount)
    {
        RemoveValue(amount);

        //Debug.Log($"{gameObject.name} has taken {finalDamage} damage. Currenthealth: {CurrentValue}");

        if (AtMinValue())
        {
            Death();
        }
        else
        {
            HealthChanged(CurrentValue);
        }
    }

    protected virtual void HealthInitialised(float maxHealth)
    { 
        _healthUI.SetMaxHealth(maxHealth);
    }

    protected virtual void HealthChanged(float currentHealth)
    { 
        _healthUI.SetCurrentHealth(currentHealth); 
    }

    protected virtual void Death()
    { 
        Destroy(gameObject); 
    }
}

